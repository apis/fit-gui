using System;
using System.IO;
using Gtk;
using WebKit;
using fit.gui.common;
using System.Collections.Generic;

namespace fit.gui.gtk
{
	public partial class MainWindow: Window
	{
		private static Configuration configuration = Configuration.Load();
		private FitTestContainer _fitTestFolderContainer = new FitTestContainer(configuration);
		private FitTestRunner fitTestRunner = null;
		private string _currentDirectory;
		private WebView _webView;

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

			InitializeTreeView();

			fitTestRunner = new FitTestRunner(_fitTestFolderContainer);
			fitTestRunner.FitTestRunStartedEventSink += new FitTestRunStartedEventDelegate(OnFitTestRunStartedEvent);
			fitTestRunner.FitTestRunStoppedEventSink += new FitTestRunStoppedEventDelegate(OnFitTestRunStoppedEvent);
			fitTestRunner.FitTestStartedEventSink += new FitTestStartedEventDelegate(OnFitTestStartedEvent);
			fitTestRunner.FitTestStoppedEventSink += new FitTestStoppedEventDelegate(OnFitTestStoppedEvent);
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

		private void OnTreeViewSelectionChanged(object sender, EventArgs eventArgs)
		{
			UpdateSelectedView();
		}

		private void UpdateSelectedView()
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

		private void OnFitTestRunStartedEvent(int numberOfTestsToDo)
		{
//		startToolBarButton.Text = "Stop";
//		startToolBarButton.ToolTipText = "Stop test(s)";
//		startToolBarButton.ImageIndex = 3;
//		startMenuItem.Text = "Stop";

			RedrawTreeViewBeforeTestRun(_fitTestFolderContainer);
//		mainProgressBar.Color = Color.LimeGreen;

//		mainProgressBar.Minimum = 0;
//		mainProgressBar.Maximum = numberOfTestsToDo;
//		mainProgressBar.Value = 0;
//		mainProgressBar.Step = 1;
		}

		private void OnFitTestRunStoppedEvent(bool isAborted)
		{
//		// TODO: If menu is open it doesn't update Text for item right away ?
//		if (isAborted) {
//			mainProgressBar.Value = mainProgressBar.Maximum;
//		}
//		startToolBarButton.Text = "Start";
//		startToolBarButton.ToolTipText = "Start test(s)";
//		startToolBarButton.ImageIndex = 2;
//		startMenuItem.Text = "Start";
		}

		private void OnFitTestStartedEvent(FitTestFile fitTestFile)
		{
			UpdateFileNodeBeforeTestExecution(fitTestFile);
		}

		private void OnFitTestStoppedEvent(FitTestFile fitTestFile)
		{
			UpdateFileNodeAfterTestExecution(fitTestFile);
		}

		private void OnErrorEvent(object sender, fit.gui.common.ErrorEventArgs args)
		{
			OnFatalError(args.Exception);
		}

		public void OnFatalError(System.Exception exception)
		{
			MessageDialog md = new MessageDialog(this, 
                DialogFlags.DestroyWithParent, MessageType.Error, 
                ButtonsType.Ok, "Fatal error occured!\n\nException: {0}\nMessage: {1}\nSource: {2}\nStack Trace:\n{3}", 
		        exception.GetType().FullName, exception.Message, exception.Source, exception.StackTrace);
			md.Run();
			md.Destroy();

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
				fitTestRunner.FitTestStoppedEventSink += new FitTestStoppedEventDelegate(OnFitTestStoppedEvent);
				fitTestRunner.FitTestStartedEventSink += new FitTestStartedEventDelegate(OnFitTestStartedEvent);
				fitTestRunner.FitTestRunStoppedEventSink += new FitTestRunStoppedEventDelegate(OnFitTestRunStoppedEvent);
				fitTestRunner.FitTestRunStartedEventSink += new FitTestRunStartedEventDelegate(OnFitTestRunStartedEvent);
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

		protected void OnButton18Clicked(object sender, EventArgs args)
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

		TreeStore _treeStore = new TreeStore(typeof(bool), typeof(string), typeof(string), typeof(int), typeof(int));

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

			TreeIter parentIter;
			SetCellProperties(_treeStore.IterParent(out parentIter, iter), TestState.NotExecuted, cellRenderer);

			var cellRendererText = (CellRendererText)cellRenderer;
			cellRendererText.Text = results;
		}

		private void SetCellProperties(bool testFile, TestState testState, CellRenderer cellRenderer)
		{
			if (testFile)
			{
				cellRenderer.CellBackgroundGdk = new Gdk.Color(255, 255, 255);
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

			TreeIter parentIter;
			SetCellProperties(_treeStore.IterParent(out parentIter, iter), TestState.NotExecuted, cellRenderer);

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

			TreeIter parentIter;
			var testFile = _treeStore.IterParent(out parentIter, iter);

			SetCellProperties(testFile, TestState.NotExecuted, cellRenderer);

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
			UpdateSelectedView();
		}

		protected void OnTogglebuttonShowResultClicked(object sender, EventArgs eventArgs)
		{
			togglebuttonShowSpecification.Active = !togglebuttonShowResult.Active;
			UpdateSelectedView();
		}

		private void RedrawTreeViewBeforeTestRun(FitTestContainer fitTestContainer)
		{
			fitTestContainer.ResetExecutedFlag();
			// Image ???
			ExecuteFuncOnTreeTestFileIter(
				(foundIter) =>
			{
				_treeStore.SetValue(foundIter, 2, "");
			}
			);
		}

		protected void OnButton454Clicked(object sender, EventArgs eventArgs)
		{
			RunTests();
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

		private delegate void IterateTreeDelegate(TreeIter iter);

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

		private void UpdateFileNodeBeforeTestExecution(FitTestFile fitTestFile)
		{
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

		private	string GetCountsString(TestRunProperties testRunProperties)
		{
			return string.Format("{0} right, {1} wrong, {2} ignored, {3} exceptions, {4} time", 
				testRunProperties.countsRight, testRunProperties.countsWrong,
				testRunProperties.countsIgnores, testRunProperties.countsExceptions,
				testRunProperties.RunElapsedTime);
		}

		private void UpdateFileNodeAfterTestExecution(FitTestFile fitTestFile)
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
						_treeStore.SetValue(foundIter, 4, (int)TestState.Passed);
					}
					else
					{
						_treeStore.SetValue(foundIter, 4, (int)TestState.Failed);
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

//					mainProgressBar.PerformStep();
				}
			}
			);
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
	}
}