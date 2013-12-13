using System;
using System.IO;
using Gtk;
using WebKit;
using fit.gui.common;

namespace fit.gui.gtk
{
	public partial class MainWindow: Gtk.Window
	{
		private static Configuration configuration = Configuration.Load();
		private FitTestContainer _fitTestFolderContainer = new FitTestContainer(configuration);
		private FitTestRunner fitTestRunner = null;
		private string _currentDirectory;

		public MainWindow(): base (Gtk.WindowType.Toplevel)
		{
			Build();

			_currentDirectory = Directory.GetCurrentDirectory();

			WebView webView = new WebView();
			webView.Open("about:blank");
			webView.Show();
			this.scrolledwindow1.Add(webView);

			InitializeTreeView();


			fitTestRunner = new FitTestRunner(_fitTestFolderContainer);
			fitTestRunner.FitTestRunStartedEventSink += new FitTestRunStartedEventDelegate(FitTestRunStartedEventHandler);
			fitTestRunner.FitTestRunStoppedEventSink += new FitTestRunStoppedEventDelegate(FitTestRunStoppedEventHandler);
			fitTestRunner.FitTestStartedEventSink += new FitTestStartedEventDelegate(FitTestStartedEventHandler);
			fitTestRunner.FitTestStoppedEventSink += new FitTestStoppedEventDelegate(FitTestStoppedEventHandler);
			fitTestRunner.ErrorEvent += OnErrorEvent;

			if (configuration.mainFormPropertiesLoaded)
			{
				this.Resize(configuration.WindowWidth, configuration.WindowHeight);

//			this.WindowPosition = Gtk.WindowPosition.None;
				this.Move(configuration.WindowLocationX, configuration.WindowLocationY);
//			WindowState = (FormWindowState)Enum.Parse (typeof(FormWindowState), configuration.WindowState);
//			treeView.Size = new Size (configuration.mainFormTreeViewSizeWidth,
//					treeView.Size.Height);
			}
			else
			{
				FillLocationAndSize(configuration);
				configuration.mainFormTreeViewSizeWidth = 123; //treeView.Size.Width; 
			}

			_fitTestFolderContainer.Load();
			RedrawTreeView(_fitTestFolderContainer);

//		ShowInputFileWebPage (@"about:blank");
//		ShowOutputFileWebPage (@"about:blank");

			this.Show();
		}
	
		protected void OnDeleteEvent(object sender, DeleteEventArgs a)
		{
			CleanUp();

			Application.Quit();
			a.RetVal = true;
		}

		private void FitTestRunStartedEventHandler(int numberOfTestsToDo)
		{
//		startToolBarButton.Text = "Stop";
//		startToolBarButton.ToolTipText = "Stop test(s)";
//		startToolBarButton.ImageIndex = 3;
//		startMenuItem.Text = "Stop";

//		RedrawTreeViewBeforeTestRun (fitTestFolderContainer);
//		mainProgressBar.Color = Color.LimeGreen;

//		mainProgressBar.Minimum = 0;
//		mainProgressBar.Maximum = numberOfTestsToDo;
//		mainProgressBar.Value = 0;
//		mainProgressBar.Step = 1;
		}

		private void FitTestRunStoppedEventHandler(bool isAborted)
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

		private void FitTestStartedEventHandler(FitTestFile fitTestFile)
		{
			FitTestFolder fitTestFolder = _fitTestFolderContainer.GetFolderByHashCode(fitTestFile.ParentHashCode);
//		UpdateFileNodeBeforeTestExecution (fitTestFolder, fitTestFile);
		}

		private void FitTestStoppedEventHandler(FitTestFile fitTestFile)
		{
			FitTestFolder fitTestFolder = _fitTestFolderContainer.GetFolderByHashCode(fitTestFile.ParentHashCode);
//		UpdateFileNodeAfterTestExecution (fitTestFolder, fitTestFile);
//		if (treeView.SelectedNode == GetTreeNodeByHashCode (treeView.Nodes, fitTestFile.GetHashCode ())) {
//			UpdatePanesForTreeNode (treeView.SelectedNode);
//		}
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
//		TreeNode selectedNode = treeView.SelectedNode;
//
//		if (selectedNode == null)
//			return;
//
//		if (selectedNode.Parent == null)
//		{
//			int folderHashCode = (int)selectedNode.Tag;
//			fitTestRunner.RunFolder (fitTestFolderContainer.GetFolderByHashCode (folderHashCode));
//		} else
//		{
//			int fileHashCode = (int)selectedNode.Tag;
//			fitTestRunner.RunFile (fitTestFolderContainer.GetFileByHashCode (fileHashCode));
//		}
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
		}

		private void CleanUp()
		{
			try
			{

				fitTestRunner.ErrorEvent -= OnErrorEvent;
				fitTestRunner.FitTestStoppedEventSink += new FitTestStoppedEventDelegate(FitTestStoppedEventHandler);
				fitTestRunner.FitTestStartedEventSink += new FitTestStartedEventDelegate(FitTestStartedEventHandler);
				fitTestRunner.FitTestRunStoppedEventSink += new FitTestRunStoppedEventDelegate(FitTestRunStoppedEventHandler);
				fitTestRunner.FitTestRunStartedEventSink += new FitTestRunStartedEventDelegate(FitTestRunStartedEventHandler);
				fitTestRunner = null;

				FillLocationAndSize(configuration);
				configuration.mainFormTreeViewSizeWidth = 123; //treeView.Size.Width; 

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
			TreeIter iter = _treeStore.AppendValues(false, fitTestFolder.FolderName, "", fitTestFolder.GetHashCode(), "", fitTestFolder.GetHashCode());
			for (int fileIndex = 0; fileIndex < fitTestFolder.Count; ++fileIndex)
			{
				FitTestFile fitTestFile = fitTestFolder[fileIndex];
				_treeStore.AppendValues(iter, false, System.IO.Path.GetFileNameWithoutExtension(fitTestFile.FileName), "", fitTestFile.GetHashCode());
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

		TreeStore _treeStore = new Gtk.TreeStore(typeof(bool), typeof(string), typeof(string), typeof(int));

		private void InitializeTreeView()
		{
			CellRendererToggle cellRendererToggle = new CellRendererToggle();
			cellRendererToggle.Activatable = true;
			cellRendererToggle.Toggled += OnToggled;

			TreeViewColumn checkBoxColumn = new TreeViewColumn() { Title = "State" };
			checkBoxColumn.PackStart(cellRendererToggle, true);
			treeview1.AppendColumn(checkBoxColumn);
			checkBoxColumn.SetCellDataFunc(cellRendererToggle, new TreeCellDataFunc(RenderCheckBox));
//			checkBoxColumn.AddAttribute(cellRendererToggle, "acive", 0);

			treeview1.AppendColumn("Folder / Test", new CellRendererText(), "text", 1);
			treeview1.AppendColumn("Results", new CellRendererText(), "text", 2);

			treeview1.Model = _treeStore;
		}

		private void OnToggled(object o, ToggledArgs args)
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

		private void RenderCheckBox(TreeViewColumn column, CellRenderer cellRenderer, TreeModel model, TreeIter iter)
		{
			var cellRendererToggle = (CellRendererToggle)cellRenderer;

			bool currentState = (bool)_treeStore.GetValue(iter, 0);

			TreeIter parentIter;
			if (_treeStore.IterParent(out parentIter, iter))
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
	}
}