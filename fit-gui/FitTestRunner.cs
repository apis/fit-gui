using System;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Collections;
using fit.gui.common;

namespace fit.gui
{
	public delegate void FitTestRunStartedEventDelegate(int numberOfTestsToDo);
	public delegate void FitTestRunStoppedEventDelegate(bool isAborted);
	public delegate void FitTestStartedEventDelegate(FitTestFile fitTestFile);
	public delegate void FitTestStoppedEventDelegate(FitTestFile fitTestFile);

	public class FitTestRunner
	{
		private const string FIT_GUI_RUNNER_NAME = "Runner.exe";
		private Thread workerThread = null;
		private FitTestContainer fitTestContainer = null;
		private ManualResetEvent executeJobEvent = new ManualResetEvent(false);
		private AutoResetEvent exitThreadEvent = new AutoResetEvent(false);
		private AutoResetEvent stopJobEvent = new AutoResetEvent(false);
		private FitTestFolder folderToDo = null;
		private FitTestFile fileToDo = null;
		private CommonData commonData = new CommonData();

		public event FitTestRunStartedEventDelegate FitTestRunStartedEventSink;
		public event FitTestRunStoppedEventDelegate FitTestRunStoppedEventSink;
		public event FitTestStartedEventDelegate FitTestStartedEventSink;
		public event FitTestStoppedEventDelegate FitTestStoppedEventSink;

		public FitTestRunner(FitTestContainer fitTestContainer)
		{
			this.fitTestContainer = fitTestContainer;
			RegisterCommonDataAsRemotingServer();
			StartWorkerThread();
		}

		private void StartWorkerThread()
		{
			workerThread = new Thread(new ThreadStart(WorkerThreadProc));
			workerThread.Start();
		}

		private void RegisterCommonDataAsRemotingServer()
		{
			IDictionary channelProperties = new Hashtable();
			channelProperties["name"] = string.Empty;
			channelProperties["port"] = 8765;
			TcpChannel tcpChannel = new TcpChannel(channelProperties, null, null);
			ChannelServices.RegisterChannel(tcpChannel);
			WellKnownServiceTypeEntry wellKnownServiceTypeEntry = new WellKnownServiceTypeEntry(
				typeof (CommonData), typeof (CommonData).Name, WellKnownObjectMode.Singleton);
			RemotingConfiguration.RegisterWellKnownServiceType(wellKnownServiceTypeEntry);
			RemotingServices.Marshal(commonData, typeof (CommonData).Name);
		}

		public void WorkerThreadProc()
		{
			try
			{
				bool isAborted = false;
				WaitHandle[] waitHandles = new WaitHandle[2];
				waitHandles[0] = executeJobEvent;
				waitHandles[1] = exitThreadEvent;
				while (true)
				{
					int waitHandleIndex = WaitHandle.WaitAny(waitHandles);
					if (waitHandleIndex == 1) break;

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
					executeJobEvent.Reset();
					FitTestRunStoppedEventSink(isAborted);
				}
			}
			catch (Exception exception)
			{
				MainForm.OnFatalError(exception);
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
			executeJobEvent.Set();
		}

		public void RunFolder(FitTestFolder fitTestFolder)
		{
			folderToDo = fitTestFolder;
			fileToDo = null;
			executeJobEvent.Set();
		}

		public void Shutdown()
		{
			stopJobEvent.Set();
			exitThreadEvent.Set();
		}

		public bool IsRunning
		{
			get
			{
				return executeJobEvent.WaitOne(0, false);
			}
		}

		public void Stop()
		{
			stopJobEvent.Set();
		}
	}
}
