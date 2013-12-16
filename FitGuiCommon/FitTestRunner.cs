using System;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using System.Collections.Generic;

namespace fit.gui.common
{
	public delegate void FitTestRunStartedEventDelegate(int numberOfTestsToDo);

	public delegate void FitTestRunStoppedEventDelegate(bool isAborted);

	public delegate void FitTestStartedEventDelegate(FitTestFile fitTestFile);

	public delegate void FitTestStoppedEventDelegate(FitTestFile fitTestFile);

	public enum FitTestRunnerStates
	{
		Idle,
		Running,
		Stopping
	}

	public class ErrorEventArgs : EventArgs
	{
		public ErrorEventArgs(Exception exception)
		{
			Exception = exception;
		}

		public Exception Exception
		{
			get;
			private set;
		}
	}

	public class FitTestRunner
	{
		private const string Runner = "Runner.exe";
		private Thread _workerThread = null;
		private FitTestContainer _fitTestContainer = null;
		private ManualResetEvent _stopJobEvent = new ManualResetEvent(false);
		private FitTestFile[] _fitTestFiles = null;
		private CommonData _commonData = new CommonData();
		private FitTestRunnerStates _state = FitTestRunnerStates.Idle;

		public FitTestRunnerStates State
		{
			get
			{
				lock (this)
				{
					return _state;
				}
			}
			set
			{
				lock (this)
				{
					_state = value;
				}
			}
		}

		public event FitTestRunStartedEventDelegate FitTestRunStartedEventSink;
		public event FitTestRunStoppedEventDelegate FitTestRunStoppedEventSink;
		public event FitTestStartedEventDelegate FitTestStartedEventSink;
		public event FitTestStoppedEventDelegate FitTestStoppedEventSink;
		public event EventHandler<ErrorEventArgs> ErrorEvent;

		public FitTestRunner(FitTestContainer fitTestContainer)
		{
			this._fitTestContainer = fitTestContainer;
			RegisterCommonDataAsRemotingServer();
		}

		private void StartWorkerThread()
		{
			_stopJobEvent.Reset();
			State = FitTestRunnerStates.Running;
			_workerThread = new Thread(new ThreadStart(WorkerThreadProc));
			_workerThread.Start();
		}

		private void RegisterCommonDataAsRemotingServer()
		{
			IDictionary channelProperties = new Hashtable();
			channelProperties["name"] = string.Empty;
			channelProperties["port"] = 8765;
			TcpChannel tcpChannel = new TcpChannel(channelProperties, null, null);
			ChannelServices.RegisterChannel(tcpChannel, false);
			RemotingServices.Marshal(_commonData, typeof(CommonData).Name);
		}

		public void WorkerThreadProc()
		{
			try
			{
				bool isAborted = false;
				FitTestRunStartedEventSink(_fitTestFiles.Length);

				for (int fileIndex = 0; fileIndex < _fitTestFiles.Length; ++ fileIndex)
				{
					FitTestFile fitTestFile = _fitTestFiles[fileIndex];
					FitTestStartedEventSink(fitTestFile);
					RunFitTest(_fitTestContainer.GetFolderByHashCode(fitTestFile.ParentHashCode), fitTestFile);
					FitTestStoppedEventSink(fitTestFile);
					if (_stopJobEvent.WaitOne(0, false))
					{
						isAborted = true;
						break;
					}
				}

				State = FitTestRunnerStates.Idle;
				FitTestRunStoppedEventSink(isAborted);
			}
			catch (Exception exception)
			{
				if (ErrorEvent != null)
				{
					ErrorEvent(this, new ErrorEventArgs(exception));
				}
			}
		}

		private void RunFitTest(FitTestFolder fitTestFolder, FitTestFile fitTestFile)
		{
			fitTestFile.TestRunProperties = ExecuteFit(
				Path.Combine(fitTestFolder.InputFolder, fitTestFile.FileName),
				Path.Combine(fitTestFolder.OutputFolder, fitTestFile.FileName),
				fitTestFolder.FixturePath);
			fitTestFile.isExecuted = true;
		}

		private TestRunProperties ExecuteFit(string inputFile, string outputFile, string fixturePath)
		{
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = fixturePath.Split(';')[0];
			AppDomain fitGuiRunnerDomain = AppDomain.CreateDomain("RunnerDomain", null, setup);
			try
			{
				string executingAssemblyPath = Path.GetDirectoryName(
				Assembly.GetExecutingAssembly().CodeBase.Replace("file://", ""));
				string[] args = {inputFile, outputFile, fixturePath};
				fitGuiRunnerDomain.ExecuteAssembly(Path.Combine(executingAssemblyPath, Runner), args);
			}
			catch
			{
				AppDomain.Unload(fitGuiRunnerDomain);
			}

			return _commonData.TestRunProperties;
		}

		public void Run(FitTestFile[] fitTestFiles)
		{
			_fitTestFiles = fitTestFiles;
			StartWorkerThread();
		}

		public void RunFile(FitTestFile fitTestFile)
		{
			Run(new FitTestFile[] { fitTestFile });
		}

		public void RunFolder(FitTestFolder fitTestFolder)
		{
			var fitTestFiles = new List<FitTestFile>();
			for (int fileIndex = 0; fileIndex < fitTestFolder.Count; ++ fileIndex)
			{
				fitTestFiles.Add(fitTestFolder[fileIndex]);	
			}
			Run(fitTestFiles.ToArray());
		}

		public void Stop()
		{
			if (State == FitTestRunnerStates.Running)
			{
				State = FitTestRunnerStates.Stopping;
				_stopJobEvent.Set();
			}
		}
	}
}