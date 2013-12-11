using System;
using Gtk;
using WebKit;
using fit.gui.common;

namespace fit.gui.gtk
{
	public partial class MainWindow: Gtk.Window
	{
		private static Configuration configuration = Configuration.Load();
		private FitTestContainer fitTestFolderContainer = new FitTestContainer(configuration);
		private FitTestRunner fitTestRunner = null;

		public MainWindow(): base (Gtk.WindowType.Toplevel)
		{
			Build();

			WebView webView = new WebView();
			webView.Open("about:blank");
			webView.Show();
			this.scrolledwindow1.Add(webView);


			fitTestRunner = new FitTestRunner(fitTestFolderContainer);
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
			fitTestFolderContainer.Load();
//		RedrawTreeView (fitTestFolderContainer);
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
			FitTestFolder fitTestFolder = fitTestFolderContainer.GetFolderByHashCode(fitTestFile.ParentHashCode);
//		UpdateFileNodeBeforeTestExecution (fitTestFolder, fitTestFile);
		}

		private void FitTestStoppedEventHandler(FitTestFile fitTestFile)
		{
			FitTestFolder fitTestFolder = fitTestFolderContainer.GetFolderByHashCode(fitTestFile.ParentHashCode);
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

		private void NewFolder()
		{
//		using (AddFolderForm addFolderForm = new AddFolderForm())
//		{
//			if (addFolderForm.ShowDialog () == DialogResult.OK)
//			{
//				FitTestFolder fitTestFolder = addFolderForm.FitTestFolder;
//				treeView.BeginUpdate ();
//				TreeNode parentTreeNode = new TreeNode (fitTestFolder.FolderName);
//				fitTestFolderContainer.Add (fitTestFolder);
//				parentTreeNode.Tag = fitTestFolder.GetHashCode ();
//				treeView.Nodes.Add (parentTreeNode);
//				treeView.SelectedNode = parentTreeNode;
//
//				for (int index = 0; index < fitTestFolder.Count; ++ index)
//				{
//					TreeNode childTreeNode = new TreeNode (fitTestFolder [index].FileName);
//					parentTreeNode.Nodes.Add (childTreeNode);
//					childTreeNode.Tag = fitTestFolder [index].GetHashCode ();
//				}
//				treeView.EndUpdate ();
//			}
//		}
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

				int x, y;
				GetPosition(out x, out y);

				int width, height;
				GetSize(out width, out height);

				configuration.WindowWidth = width;
				configuration.WindowHeight = height;
				configuration.WindowLocationX = x;
				configuration.WindowLocationY = y;

				// Set for compatability only 
				configuration.WindowState = "Normal";

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
			TestsFolder dialog = new TestsFolder();
			dialog.Run();
			dialog.Destroy();
		}
	}
}