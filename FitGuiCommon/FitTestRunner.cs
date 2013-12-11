using System;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;

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

        public Exception Exception {
			get;
			private set;
		}
    }

	public class FitTestRunner
	{
		private const string FIT_GUI_RUNNER_NAME = "Runner.exe";
		private Thread workerThread = null;
		private FitTestContainer fitTestContainer = null;
		private ManualResetEvent stopJobEvent = new ManualResetEvent(false);
		private FitTestFolder folderToDo = null;
		private FitTestFile fileToDo = null;
		private CommonData commonData = new CommonData();
		private FitTestRunnerStates state = FitTestRunnerStates.Idle;

		public FitTestRunnerStates State
		{
			get
			{
				lock (this)
				{
					return state;
				}
			}
			set
			{
				lock (this)
				{
					state = value;
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
			this.fitTestContainer = fitTestContainer;
			RegisterCommonDataAsRemotingServer();
		}

		private void StartWorkerThread()
		{
			stopJobEvent.Reset();
			State = FitTestRunnerStates.Running;
			workerThread = new Thread(new ThreadStart(WorkerThreadProc));
			workerThread.Start();
		}

		private void RegisterCommonDataAsRemotingServer()
		{
			IDictionary channelProperties = new Hashtable();
			channelProperties["name"] = string.Empty;
			channelProperties["port"] = 8765;
			TcpChannel tcpChannel = new TcpChannel(channelProperties, null, null);
			ChannelServices.RegisterChannel(tcpChannel, false);
//			WellKnownServiceTypeEntry wellKnownServiceTypeEntry = new WellKnownServiceTypeEntry(
//				typeof (CommonData), typeof (CommonData).Name, WellKnownObjectMode.Singleton);
//			RemotingConfiguration.RegisterWellKnownServiceType(wellKnownServiceTypeEntry);
			RemotingServices.Marshal(commonData, typeof (CommonData).Name);
		}

		public void WorkerThreadProc()
		{
			try
			{
				bool isAborted = false;

				if (fileToDo != null)
				{
					FitTestRunStartedEventSink(1);
					folderToDo = fitTestContainer.GetFolderByHashCode(fileToDo.ParentHashCode);
					FitTestStartedEventSink(fileToDo);
					RunFitTest(folderToDo, fileToDo);
					FitTestStoppedEventSink(fileToDo);
				}
				else
				{
					FitTestRunStartedEventSink(folderToDo.Count);
					for (int fileIndex = 0; fileIndex < folderToDo.Count; ++ fileIndex)
					{
						FitTestFile fitTestFile = folderToDo[fileIndex];
						FitTestStartedEventSink(fitTestFile);
						RunFitTest(folderToDo, fitTestFile);
						FitTestStoppedEventSink(fitTestFile);
						if (stopJobEvent.WaitOne(0, false))
						{
							isAborted = true;
							break;
						}
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
			string executingAssemblyPath = Path.GetDirectoryName(
				Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", ""));
			string[] args = {inputFile, outputFile, fixturePath};
			fitGuiRunnerDomain.ExecuteAssembly(Path.Combine(executingAssemblyPath, FIT_GUI_RUNNER_NAME), null, args);
			AppDomain.Unload(fitGuiRunnerDomain);
			return commonData.TestRunProperties;
		}

		public void RunFile(FitTestFile fitTestFile)
		{
			folderToDo = null;
			fileToDo = fitTestFile;
			StartWorkerThread();
		}

		public void RunFolder(FitTestFolder fitTestFolder)
		{
			folderToDo = fitTestFolder;
			fileToDo = null;
			StartWorkerThread();
		}

		public void Stop()
		{
			if (State == FitTestRunnerStates.Running)
			{
				State = FitTestRunnerStates.Stopping;
				stopJobEvent.Set();
			}
		}
	}
}