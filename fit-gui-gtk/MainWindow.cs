using System;
using System.IO;
using Gtk;
using WebKit;
using fit.gui.common;
using System.Collections.Generic;
using System.Globalization;

namespace fit.gui.gtk
{
	public partial class MainWindow: Window
	{
		private static Configuration configuration = Configuration.Load();
		private FitTestContainer _fitTestFolderContainer = new FitTestContainer(configuration);
		private FitTestRunner fitTestRunner = null;
		private string _currentDirectory;
		private WebView _webView;
		private int _runTestsCount;
		private int _runTestsFailed;
//		private int _numberOfTests;
		private delegate void IterateTreeDelegate(TreeIter iter);

		TreeStore _treeStore = new TreeStore(typeof(bool), typeof(string), typeof(string), typeof(int), typeof(int));

		private enum TestState
		{
			NotExecuted = 0,
			Running,
			Failed,
			Passed
		}

		public MainWindow(): base (WindowType.Toplevel)
		{
			Build();

			_currentDirectory = Directory.GetCurrentDirectory();

			_webView = new WebView();
			_webView.Show();
			scrolledwindow1.Add(_webView);

			var style = progressbarMain.Style;
			progressbarMain.ModifyFg(StateType.Prelight, style.Foreground(StateType.Normal));

			SetButtonToStart();
			SetButton(buttonAddFolder, "folder_add.png", "Add", "Add new test folder");
			SetButton(buttonEditFolder, "folder.png", "Edit", "Edit test folder");
			SetButton(buttonDeleteFolder, "folder_delete.png", "Delete", "Delete test folder");

			InitializeTreeView();

			fitTestRunner = new FitTestRunner(_fitTestFolderContainer);
			fitTestRunner.FitTestRunStartedEventSink += OnFitTestRunStartedEvent;
			fitTestRunner.FitTestRunStoppedEventSink += OnFitTestRunStoppedEvent;
			fitTestRunner.FitTestStartedEventSink += OnFitTestStartedEvent;
			fitTestRunner.FitTestStoppedEventSink += OnFitTestStoppedEvent;
			fitTestRunner.ErrorEvent += OnErrorEvent;

			if (configuration.MainFormPropertiesLoaded)
			{
				Resize(configuration.WindowWidth, configuration.WindowHeight);
				Move(configuration.WindowLocationX, configuration.WindowLocationY);
				hpaned1.Position = configuration.MainFormTreeViewSizeWidth; 
			}
			else
			{
				FillLocationAndSize(configuration);
			}

			_fitTestFolderContainer.Load();
			RedrawTreeView(_fitTestFolderContainer);
			treeview1.Selection.Changed += OnTreeViewSelectionChanged;

			TreeIter treeIter;
			if (_treeStore.GetIterFirst(out treeIter))
				treeview1.Selection.SelectIter(treeIter);

			Show();
		}

		private void OnFitTestRunStartedEvent(int numberOfTestsToDo)
		{
			Gtk.Application.Invoke((sender, eventArgs) =>
			{
				SetButtonToStop();
				ProgressBarOnFitTestRunStarted(numberOfTestsToDo);
				RedrawTreeViewBeforeTestRun(_fitTestFolderContainer);
			}
			);
		}

		private void OnFitTestRunStoppedEvent(bool isAborted)
		{
			Gtk.Application.Invoke((sender, eventArgs) =>
			{
				ProgressBarOnFitTestRunStopped();
				SetButtonToStart();
			}
			);
		}

		private void OnFitTestStartedEvent(FitTestFile fitTestFile)
		{
			Gtk.Application.Invoke((sender, eventArgs) =>
			{
				ProgressBarOnFitTestStarted(fitTestFile);
				ExecuteFuncOnTreeTestFileIter(
				(foundIter) =>
				{
					var hash = (int)_treeStore.GetValue(foundIter, 3);
					if (fitTestFile.GetHashCode() == hash)
					{
						_treeStore.SetValue(foundIter, 2, "Running...");
						_treeStore.SetValue(foundIter, 4, (int)TestState.Running);
					}
				}
				);
			}
			);		
		}

		private void OnFitTestStoppedEvent(FitTestFile fitTestFile)
		{
			Gtk.Application.Invoke((sender, eventArgs) =>
			{
				ExecuteFuncOnTreeTestFileIter(
				(foundIter) =>
				{
					var hash = (int)_treeStore.GetValue(foundIter, 3);
					if (fitTestFile.GetHashCode() == hash)
					{
						_treeStore.SetValue(foundIter, 2, GetCountsString(fitTestFile.TestRunProperties));

						if (GetFitTestFileFailedState(fitTestFile))
						{
							_treeStore.SetValue(foundIter, 4, (int)TestState.Failed);
							_runTestsFailed ++;
						}
						else
						{
							_treeStore.SetValue(foundIter, 4, (int)TestState.Passed);
						}

						TreeIter selectedIter;
						if (treeview1.Selection.GetSelected(out selectedIter))
						{
							var selectedHash = (int)_treeStore.GetValue(selectedIter, 3);
							if (selectedHash == hash)
							{
								UpdatePanesForTreeNode(selectedHash);
							}
						}
					}
				}
				);
				ProgressBarOnFitTestStopped();
			}
			);
		}

		private void OnErrorEvent(object sender, fit.gui.common.ErrorEventArgs args)
		{
			Gtk.Application.Invoke((s, eventArgs) =>
			{
				OnFatalError(args.Exception);
			}
			);
		}

		private void SetButton(Gtk.Button button, string iconFileName, string labelText, string tooltipText)
		{
			button.Remove(button.Child);
			VBox box = new VBox(false, 0);
			box.BorderWidth = 2;
			Gtk.Image image = new Gtk.Image();
			image.Pixbuf = new Gdk.Pixbuf(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "./" + iconFileName));
			Label label = new Label(labelText);
			box.PackStart(image, false, false, 1);
			box.PackStart(label, false, false, 1);
			image.Show();
			label.Show();
			box.Show();
			button.Add(box);
			button.TooltipText = tooltipText;
		}

		private void SetButtonToStart()
		{
			SetButton(buttonStartStop, "control_play_blue.png", "Start", "Start test run");
		}

		private void SetButtonToStop()
		{
			SetButton(buttonStartStop, "control_stop_blue.png", "Stop", "Stop test run");
		}

		private void ProgressBarOnFitTestRunStarted(int numberOfTestsToDo)
		{
			progressbarMain.Fraction = 0;
			progressbarMain.PulseStep = 1 / (double)(numberOfTestsToDo);
			_runTestsFailed = 0;
			_runTestsCount = 0;
			progressbarMain.Text = "";
//			_numberOfTests = numberOfTestsToDo;
			ProgressBarSetColor();
		}

		private void ProgressBarOnFitTestRunStopped()
		{
			if (_runTestsFailed == 0)
				progressbarMain.Text = string.Format(CultureInfo.InvariantCulture, "All test(s) completed successfully!");
			else
				progressbarMain.Text = string.Format(CultureInfo.InvariantCulture, "Failed {0} test(s)! ", _runTestsFailed);
			progressbarMain.Fraction = 1;
		}

		private void ProgressBarOnFitTestStarted(FitTestFile fitTestFile)
		{
//			progressbarMain.Text = string.Format(CultureInfo.InvariantCulture, "{0}/{1} ...", _runTestsCount + 1, _numberOfTests);
		}

		private void ProgressBarOnFitTestStopped()
		{
			_runTestsCount ++;
			progressbarMain.Fraction += progressbarMain.PulseStep;
			ProgressBarSetColor();
		}

		private void ProgressBarSetColor()
		{
			if (_runTestsFailed == 0)
				progressbarMain.ModifyBg(StateType.Selected, new Gdk.Color(80, 255, 80));
			else
				progressbarMain.ModifyBg(StateType.Selected, new Gdk.Color(255, 120, 120));
		}

		private void OnTreeViewSelectionChanged(object sender, EventArgs eventArgs)
		{
			UpdateHtmlView();
		}

		private void UpdateHtmlView()
		{
			TreeIter iter;

			if (!treeview1.Selection.GetSelected(out iter))
				return;

			TreeIter parentIter;
			if (_treeStore.IterParent(out parentIter, iter))
			{
				int hash = (int)_treeStore.GetValue(iter, 3);
				UpdatePanesForTreeNode(hash);
			}
			else
			{
				_webView.Open("about:blank");
			}
		}
	
		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			CleanUp();

			Application.Quit();
			a.RetVal = true;
		}

		public void OnFatalError(System.Exception exception)
		{
			MessageDialog md = new MessageDialog(this, 
	                DialogFlags.DestroyWithParent, MessageType.Error, 
	                ButtonsType.Ok, "Fatal error occured!\n\nException: {0}\nMessage: {1}\nSource: {2}\nStack Trace:\n{3}", 
			        exception.GetType().FullName, exception.Message, exception.Source, exception.StackTrace);
			try
			{
				md.Run();
			}
			finally
			{
				md.Destroy();
			}
			

			Environment.Exit(-1);
		}

		private void RunTests()
		{
			var fitTestFiles = GetFitTestsToRun();
			fitTestRunner.Run(fitTestFiles); 
		}

		private void FillLocationAndSize(Configuration configuration)
		{
			int x, y;
			GetPosition(out x, out y);

			configuration.WindowLocationX = x;
			configuration.WindowLocationY = y;

			int width, height;
			GetSize(out width, out height);

			configuration.WindowWidth = width;
			configuration.WindowHeight = height;

			// Set for compatability only 
			configuration.WindowState = "Normal";

			configuration.MainFormTreeViewSizeWidth = hpaned1.Position;
		}

		private void CleanUp()
		{
			try
			{
				fitTestRunner.ErrorEvent -= OnErrorEvent;
				fitTestRunner.FitTestStoppedEventSink += OnFitTestStoppedEvent;
				fitTestRunner.FitTestStartedEventSink += OnFitTestStartedEvent;
				fitTestRunner.FitTestRunStoppedEventSink += OnFitTestRunStoppedEvent;
				fitTestRunner.FitTestRunStartedEventSink += OnFitTestRunStartedEvent;
				fitTestRunner = null;

				FillLocationAndSize(configuration);

				PrintConfiguration(configuration);

				Configuration.Save(configuration);
			}
			catch (Exception exception)
			{
				OnFatalError(exception);
			}
		}

		private void PrintConfiguration(Configuration configuration)
		{
			Console.WriteLine("Width: {0}, Height: {1}, X: {2}, Y: {3}", configuration.WindowWidth, configuration.WindowHeight, configuration.WindowLocationX,
			configuration.WindowLocationY);
		}

		private void AddTestFolderToTreeStore(FitTestFolder fitTestFolder)
		{
			TreeIter iter = _treeStore.AppendValues(false, fitTestFolder.FolderName, "", fitTestFolder.GetHashCode(), (int)TestState.NotExecuted);
			for (int fileIndex = 0; fileIndex < fitTestFolder.Count; ++fileIndex)
			{
				FitTestFile fitTestFile = fitTestFolder[fileIndex];
				_treeStore.AppendValues(iter, false, System.IO.Path.GetFileNameWithoutExtension(fitTestFile.FileName), "", fitTestFile.GetHashCode(), (int)TestState.NotExecuted);
			}
		}

		private void RedrawTreeView(FitTestContainer fitTestContainer)
		{
			for (int folderIndex = 0; folderIndex < _fitTestFolderContainer.Count; ++ folderIndex)
			{
				FitTestFolder fitTestFolder = _fitTestFolderContainer[folderIndex];
				AddTestFolderToTreeStore(fitTestFolder);
			}
			treeview1.ExpandAll();
		}

		private void InitializeTreeView()
		{
			CellRendererToggle checkBoxCellRenderer = new CellRendererToggle();
			checkBoxCellRenderer.Activatable = true;
			checkBoxCellRenderer.Toggled += OnCheckBoxToggled;

			TreeViewColumn checkBoxColumn = new TreeViewColumn() { Title = "State" };
			checkBoxColumn.PackStart(checkBoxCellRenderer, true);
			treeview1.AppendColumn(checkBoxColumn);
			checkBoxColumn.SetCellDataFunc(checkBoxCellRenderer, new TreeCellDataFunc(OnRenderCheckBox));

			TreeViewColumn folderOrTestColumn = new TreeViewColumn() { Title = "Folder / Test" };
			CellRendererText folderOrTestCellRenderer = new CellRendererText();
			folderOrTestColumn.PackStart(folderOrTestCellRenderer, true);
			treeview1.AppendColumn(folderOrTestColumn);
			folderOrTestColumn.SetCellDataFunc(folderOrTestCellRenderer, new TreeCellDataFunc(OnRenderFolderOrTest));

			TreeViewColumn resultsColumn = new TreeViewColumn() { Title = "Run results" };
			CellRendererText resultsCellRenderer = new CellRendererText();
			resultsColumn.PackStart(resultsCellRenderer, true);
			treeview1.AppendColumn(resultsColumn);
			resultsColumn.SetCellDataFunc(resultsCellRenderer, new TreeCellDataFunc(OnRenderResults));

			treeview1.Model = _treeStore;
		}

		private void OnRenderResults(TreeViewColumn column, CellRenderer cellRenderer, TreeModel model, TreeIter iter)
		{
			var results = (string)_treeStore.GetValue(iter, 2);
			var testState = (TestState)_treeStore.GetValue(iter, 4);

			TreeIter parentIter;
			SetCellProperties(_treeStore.IterParent(out parentIter, iter), testState, cellRenderer);

			var cellRendererText = (CellRendererText)cellRenderer;
			cellRendererText.Text = results;
		}

		private void SetCellProperties(bool testFile, TestState testState, CellRenderer cellRenderer)
		{
			if (testFile)
			{
				switch (testState)
				{
					case TestState.NotExecuted:
					case TestState.Running:
						cellRenderer.CellBackgroundGdk = new Gdk.Color(255, 255, 255);
						break;

					case TestState.Failed:
						cellRenderer.CellBackgroundGdk = new Gdk.Color(255, 180, 180);
						break;

					case TestState.Passed:
						cellRenderer.CellBackgroundGdk = new Gdk.Color(180, 255, 180);
						break;

					default:
						break;
				} 

				var cellRendererText = cellRenderer as CellRendererText;
				if (cellRendererText != null)
					cellRendererText.Weight = 400;
			}
			else
			{
				cellRenderer.CellBackgroundGdk = new Gdk.Color(244, 244, 244);
				var cellRendererText = cellRenderer as CellRendererText;
				if (cellRendererText != null)
					cellRendererText.Weight = 700;
			}
		}

		private void OnRenderFolderOrTest(TreeViewColumn column, CellRenderer cellRenderer, TreeModel model, TreeIter iter)
		{
			var folderOrTestName = (string)_treeStore.GetValue(iter, 1);
			var testState = (TestState)_treeStore.GetValue(iter, 4);

			TreeIter parentIter;
			SetCellProperties(_treeStore.IterParent(out parentIter, iter), testState, cellRenderer);

			var cellRendererText = (CellRendererText)cellRenderer;
			cellRendererText.Text = folderOrTestName;
		}

		private void OnCheckBoxToggled(object o, ToggledArgs args)
		{
			TreeIter iter;
			if (!_treeStore.GetIter(out iter, new TreePath(args.Path)))
				throw new Exception("Path is wrong!");

			TreeIter parentIter;
			if (_treeStore.IterParent(out parentIter, iter))
			{
				bool oldState = (bool)_treeStore.GetValue(iter, 0);
				_treeStore.SetValue(iter, 0, !oldState);
			}
			else
			{
				TreeIter childIter;
				if (_treeStore.IterChildren(out childIter, iter))
				{
					var cellRendererToggle = (CellRendererToggle)o;

					do
					{
						_treeStore.SetValue(childIter, 0, 
						                   cellRendererToggle.Inconsistent || !cellRendererToggle.Active);
					}
					while (_treeStore.IterNext(ref childIter));
				}
			}
		}

		private void OnRenderCheckBox(TreeViewColumn column, CellRenderer cellRenderer, TreeModel model, TreeIter iter)
		{
			var cellRendererToggle = (CellRendererToggle)cellRenderer;

			bool currentState = (bool)_treeStore.GetValue(iter, 0);
			var testState = (TestState)_treeStore.GetValue(iter, 4);

			TreeIter parentIter;
			var testFile = _treeStore.IterParent(out parentIter, iter);

			SetCellProperties(testFile, testState, cellRenderer);

			if (testFile)
			{
				cellRendererToggle.Inconsistent = false;
				cellRendererToggle.Active = currentState;

				_treeStore.EmitRowChanged(_treeStore.GetPath(parentIter), parentIter);
			}
			else
			{
				int activeCount = 0;
				int inactiveCount = 0;

				TreeIter childIter;
				if (_treeStore.IterChildren(out childIter, iter))
				{
					do
					{
						if ((bool)_treeStore.GetValue(childIter, 0))
							activeCount++;
						else
							inactiveCount++;
					}
					while (_treeStore.IterNext(ref childIter));
				}

				if (activeCount > 0 && inactiveCount > 0)
				{
					cellRendererToggle.Inconsistent = true;
					cellRendererToggle.Active = false;
				}
				else if (activeCount > 0 && inactiveCount == 0)
					{
						cellRendererToggle.Inconsistent = false;
						cellRendererToggle.Active = true;
					}
					else if (activeCount == 0 && inactiveCount >= 0)
						{
							cellRendererToggle.Inconsistent = false;
							cellRendererToggle.Active = false;
						}
			}
		}

		private void UpdatePanesForTreeNode(int fileHashCode)
		{
			FitTestFile fitTestFile = _fitTestFolderContainer.GetFileByHashCode(fileHashCode);
			int folderHashCode = fitTestFile.ParentHashCode;
			FitTestFolder fitTestFolder = _fitTestFolderContainer.GetFolderByHashCode(folderHashCode);

			string fileName = fitTestFile.FileName;
			string folder = togglebuttonShowSpecification.Active ? fitTestFolder.InputFolder : fitTestFolder.OutputFolder;

			_webView.Open(System.IO.Path.Combine(folder, fileName));
		}

		protected void OnTogglebuttonShowSpecificationClicked(object sender, EventArgs eventArgs)
		{
			togglebuttonShowResult.Active = (!togglebuttonShowSpecification.Active);
			UpdateHtmlView();
		}

		protected void OnTogglebuttonShowResultClicked(object sender, EventArgs eventArgs)
		{
			togglebuttonShowSpecification.Active = !togglebuttonShowResult.Active;
			UpdateHtmlView();
		}

		private void RedrawTreeViewBeforeTestRun(FitTestContainer fitTestContainer)
		{
			fitTestContainer.ResetExecutedFlag();
			ExecuteFuncOnTreeTestFileIter(
				(foundIter) =>
			{
				_treeStore.SetValue(foundIter, 2, "");
				_treeStore.SetValue(foundIter, 4, (int)TestState.NotExecuted);
			}
			);
		}

		private FitTestFile[] GetFitTestsToRun()
		{
			List<FitTestFile> fitTestFiles = new List<FitTestFile>();
			ExecuteFuncOnTreeTestFileIter(
				(foundIter) =>
			{
				bool state = (bool)_treeStore.GetValue(foundIter, 0);
				if (state)
				{
					var hash = (int)_treeStore.GetValue(foundIter, 3);
					var fitTestFile = _fitTestFolderContainer.GetFileByHashCode(hash);
					fitTestFiles.Add(fitTestFile);
				}
			}
			);
			return fitTestFiles.ToArray();
		}

		private void IterateTree(TreeIter iter, IterateTreeDelegate func)
		{
			do
			{
				TreeIter childIter;
				if (_treeStore.IterChildren(out childIter, iter))
				{
					IterateTree(childIter, func);
				}
				else
				{
					func(iter);
				}
			}
			while (_treeStore.IterNext(ref iter));
		}

		private void ExecuteFuncOnTreeTestFileIter(IterateTreeDelegate func)
		{
			TreeIter iter;
			if (_treeStore.GetIterFirst(out iter))
			{
				IterateTree(iter, func);
			}
		}

		private	string GetCountsString(TestRunProperties testRunProperties)
		{
			return string.Format("{0} right, {1} wrong, {2} ignored, {3} exceptions {4}", 
				testRunProperties.countsRight, testRunProperties.countsWrong,
				testRunProperties.countsIgnores, testRunProperties.countsExceptions,
				testRunProperties.RunElapsedTime);
		}

		bool GetFitTestFileFailedState(FitTestFile fitTestFile)
		{
			if (fitTestFile.isExecuted)
			{ 
				if (fitTestFile.TestRunProperties.countsWrong > 0 || 
					fitTestFile.TestRunProperties.countsExceptions > 0)
				{
					return true;
				}
			}
			return false;
		}

		protected void OnButtonStartStopClicked(object sender, EventArgs eventArgs)
		{
			if (fitTestRunner.State == FitTestRunnerStates.Running)
			{
				fitTestRunner.Stop();
			}
			else
			{
				RunTests();
			}
		}

		protected void OnButtonAddFolderClicked(object sender, EventArgs e)
		{
			TestsFolder dialog = new TestsFolder(_currentDirectory);
			try
			{
				dialog.Title = "Add Tests Folder";
				if (dialog.Run() == (int)ResponseType.Ok)
				{
					_currentDirectory = dialog.CurrentDirectory;
					FitTestFolder fitTestFolder = dialog.FitTestFolder;
					_fitTestFolderContainer.Add(fitTestFolder);
					AddTestFolderToTreeStore(fitTestFolder);
				}
			}
			finally
			{
				dialog.Destroy();
			}
		}
	}
}