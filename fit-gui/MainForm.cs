using System;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Windows.Forms;
using AxSHDocVw;
using fit.gui.common;

// Show assembly installed in GAC:
// http://support.microsoft.com/default.aspx?scid=kb;en-us;Q306149

// Dynamic properties discussion
// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dv_vstechart/html/vbtchcreateyourowndynamicpropertiespreservepropertysettingsinvisualbasicnet.asp

namespace fit.gui
{
	public class MainForm : Form
	{
		private const string FIT_GUI_RUNNER_NAME = "FitGuiRunner.exe";
		private CommonData commonData = new CommonData();
		private FitTestContainer fitTestFolderContainer = new FitTestContainer();
		private Thread workerThread = null;
		private ManualResetEvent executeJobEvent = new ManualResetEvent(false);
		private ManualResetEvent exitThreadEvent = new ManualResetEvent(false);
		private FitTestFolder folderToDo = null;
		private FitTestFile fileToDo = null;

		private TreeView treeView;
		private Panel panel2;
		private Splitter splitter1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.Panel panel4;
		private AxSHDocVw.AxWebBrowser inputFileWebBrowser;
		private AxSHDocVw.AxWebBrowser outputFileWebBrowser;
		private System.Windows.Forms.StatusBar statusBar1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.ImageList mainToolBarImageList;
		private System.Windows.Forms.ToolBar mainToolBar;
		private System.Windows.Forms.ToolBarButton toolBarButtonRunTests;
		private System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
		private System.Windows.Forms.ToolBarButton toolBarButtonNewFolder;
		private System.Windows.Forms.ToolBarButton toolBarButtonEditFolder;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.ToolBarButton toolBarButtonRemoveFolder;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.ImageList treeViewImageList;
		private System.Windows.Forms.MainMenu mainMenu;

		public MainForm()
		{
			workerThread = new Thread(new ThreadStart(WorkerThreadProc));
			workerThread.Start();
			InitializeComponent();
			RegisterCommonDataAsRemotingServer();
		}

		private void RegisterCommonDataAsRemotingServer()
		{
			ChannelServices.RegisterChannel(new TcpChannel(8765));
			WellKnownServiceTypeEntry wellKnownServiceTypeEntry = new WellKnownServiceTypeEntry(
				typeof (CommonData), typeof (CommonData).Name, WellKnownObjectMode.Singleton);
			RemotingConfiguration.RegisterWellKnownServiceType(wellKnownServiceTypeEntry);
			RemotingServices.Marshal(commonData, typeof (CommonData).Name);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.treeView = new System.Windows.Forms.TreeView();
			this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel4 = new System.Windows.Forms.Panel();
			this.outputFileWebBrowser = new AxSHDocVw.AxWebBrowser();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.panel3 = new System.Windows.Forms.Panel();
			this.inputFileWebBrowser = new AxSHDocVw.AxWebBrowser();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.statusBar1 = new System.Windows.Forms.StatusBar();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItem21 = new System.Windows.Forms.MenuItem();
			this.menuItem22 = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.mainToolBar = new System.Windows.Forms.ToolBar();
			this.toolBarButtonNewFolder = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonEditFolder = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonRemoveFolder = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
			this.toolBarButtonRunTests = new System.Windows.Forms.ToolBarButton();
			this.mainToolBarImageList = new System.Windows.Forms.ImageList(this.components);
			this.panel2.SuspendLayout();
			this.panel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.outputFileWebBrowser)).BeginInit();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputFileWebBrowser)).BeginInit();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView.Font = new System.Drawing.Font("Tahoma", 10F);
			this.treeView.ForeColor = System.Drawing.SystemColors.WindowText;
			this.treeView.HideSelection = false;
			this.treeView.ImageIndex = 3;
			this.treeView.ImageList = this.treeViewImageList;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 3;
			this.treeView.Size = new System.Drawing.Size(168, 408);
			this.treeView.TabIndex = 0;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// treeViewImageList
			// 
			this.treeViewImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImageList.ImageStream")));
			this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.panel4);
			this.panel2.Controls.Add(this.splitter2);
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.treeView);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 30);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(648, 408);
			this.panel2.TabIndex = 9;
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.outputFileWebBrowser);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel4.Location = new System.Drawing.Point(171, 187);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(477, 221);
			this.panel4.TabIndex = 4;
			// 
			// outputFileWebBrowser
			// 
			this.outputFileWebBrowser.ContainingControl = this;
			this.outputFileWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outputFileWebBrowser.Enabled = true;
			this.outputFileWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.outputFileWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("outputFileWebBrowser.OcxState")));
			this.outputFileWebBrowser.Size = new System.Drawing.Size(477, 221);
			this.outputFileWebBrowser.TabIndex = 1;
			// 
			// splitter2
			// 
			this.splitter2.Cursor = System.Windows.Forms.Cursors.HSplit;
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(171, 184);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(477, 3);
			this.splitter2.TabIndex = 3;
			this.splitter2.TabStop = false;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.inputFileWebBrowser);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel3.Location = new System.Drawing.Point(171, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(477, 184);
			this.panel3.TabIndex = 2;
			// 
			// inputFileWebBrowser
			// 
			this.inputFileWebBrowser.ContainingControl = this;
			this.inputFileWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputFileWebBrowser.Enabled = true;
			this.inputFileWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.inputFileWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("inputFileWebBrowser.OcxState")));
			this.inputFileWebBrowser.Size = new System.Drawing.Size(477, 184);
			this.inputFileWebBrowser.TabIndex = 1;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(168, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 408);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// statusBar1
			// 
			this.statusBar1.Location = new System.Drawing.Point(0, 438);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(648, 24);
			this.statusBar1.TabIndex = 10;
			this.statusBar1.Text = "statusBar1";
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					 this.menuItem1,
																					 this.menuItem3,
																					 this.menuItem20});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem4,
																					  this.menuItem2,
																					  this.menuItem5,
																					  this.menuItem6,
																					  this.menuItem12,
																					  this.menuItem13,
																					  this.menuItem14,
																					  this.menuItem15});
			this.menuItem1.Text = "&File";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 0;
			this.menuItem4.Text = "&New";
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.Text = "&Open...";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 2;
			this.menuItem5.Text = "&Save";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 3;
			this.menuItem6.Text = "Save &As...";
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 4;
			this.menuItem12.Text = "-";
			// 
			// menuItem13
			// 
			this.menuItem13.Enabled = false;
			this.menuItem13.Index = 5;
			this.menuItem13.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem16,
																					   this.menuItem17,
																					   this.menuItem18,
																					   this.menuItem19});
			this.menuItem13.Text = "&Recent";
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 0;
			this.menuItem16.Text = "1";
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 1;
			this.menuItem17.Text = "2";
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 2;
			this.menuItem18.Text = "3";
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 3;
			this.menuItem19.Text = "4";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 6;
			this.menuItem14.Text = "-";
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 7;
			this.menuItem15.Text = "E&xit";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.menuItem7,
																					  this.menuItem8,
																					  this.menuItem9,
																					  this.menuItem10,
																					  this.menuItem11});
			this.menuItem3.Text = "&Tests";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 0;
			this.menuItem7.Text = "&New folder...";
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.Text = "&Edit folder...";
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 2;
			this.menuItem9.Text = "Re&move folder";
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 3;
			this.menuItem10.Text = "-";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 4;
			this.menuItem11.Text = "&Run";
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 2;
			this.menuItem20.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.menuItem21,
																					   this.menuItem22,
																					   this.menuItem23});
			this.menuItem20.Text = "&Help";
			// 
			// menuItem21
			// 
			this.menuItem21.Enabled = false;
			this.menuItem21.Index = 0;
			this.menuItem21.Text = "&Contents...";
			// 
			// menuItem22
			// 
			this.menuItem22.Index = 1;
			this.menuItem22.Text = "-";
			// 
			// menuItem23
			// 
			this.menuItem23.Index = 2;
			this.menuItem23.Text = "&About fit-gui";
			// 
			// mainToolBar
			// 
			this.mainToolBar.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
			this.mainToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.toolBarButtonNewFolder,
																						   this.toolBarButtonEditFolder,
																						   this.toolBarButtonRemoveFolder,
																						   this.toolBarButtonSeparator,
																						   this.toolBarButtonRunTests});
			this.mainToolBar.ButtonSize = new System.Drawing.Size(20, 20);
			this.mainToolBar.Divider = false;
			this.mainToolBar.DropDownArrows = true;
			this.mainToolBar.ImageList = this.mainToolBarImageList;
			this.mainToolBar.Location = new System.Drawing.Point(0, 0);
			this.mainToolBar.Name = "mainToolBar";
			this.mainToolBar.ShowToolTips = true;
			this.mainToolBar.Size = new System.Drawing.Size(648, 30);
			this.mainToolBar.TabIndex = 11;
			this.mainToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.mainToolBar_ButtonClick);
			// 
			// toolBarButtonNewFolder
			// 
			this.toolBarButtonNewFolder.ImageIndex = 3;
			this.toolBarButtonNewFolder.ToolTipText = "New tests folder";
			// 
			// toolBarButtonEditFolder
			// 
			this.toolBarButtonEditFolder.ImageIndex = 5;
			this.toolBarButtonEditFolder.ToolTipText = "Edit tests folder";
			// 
			// toolBarButtonRemoveFolder
			// 
			this.toolBarButtonRemoveFolder.ImageIndex = 4;
			this.toolBarButtonRemoveFolder.ToolTipText = "Remove tests folder";
			// 
			// toolBarButtonSeparator
			// 
			this.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// toolBarButtonRunTests
			// 
			this.toolBarButtonRunTests.ImageIndex = 1;
			this.toolBarButtonRunTests.ToolTipText = "Run test(s)";
			// 
			// mainToolBarImageList
			// 
			this.mainToolBarImageList.ImageSize = new System.Drawing.Size(20, 20);
			this.mainToolBarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mainToolBarImageList.ImageStream")));
			this.mainToolBarImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(648, 462);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.statusBar1);
			this.Controls.Add(this.mainToolBar);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "fit-gui";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Closed += new System.EventHandler(this.MainForm_Closed);
			this.panel2.ResumeLayout(false);
			this.panel4.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.outputFileWebBrowser)).EndInit();
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.inputFileWebBrowser)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private static void OnFatalError(Exception exception)
		{
			MessageBox.Show(null, 
				string.Format("Source: {0}\nMessage: {1}\nStack Trace:\n{2}", 
				exception.Source, exception.Message, exception.StackTrace), 
				"Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Environment.Exit(-1);
		}

		[STAThread]
		private static void Main()
		{
			try
			{
				Application.Run(new MainForm());
			}
			catch (Exception exception)
			{
				OnFatalError(exception);
			}
		}

		private void NewFolder()
		{
			using (AddFolderForm addFolderForm = new AddFolderForm())
			{
				if (addFolderForm.ShowDialog() == DialogResult.OK)
				{
					FitTestFolder fitTestFolder = addFolderForm.FitTestFolder;
					treeView.BeginUpdate();
					TreeNode parentTreeNode = new TreeNode(fitTestFolder.FolderName);
					fitTestFolderContainer.Add(fitTestFolder);
					parentTreeNode.Tag = fitTestFolder.GetHashCode();
					treeView.Nodes.Add(parentTreeNode);
					treeView.SelectedNode = parentTreeNode;

					for (int index = 0; index < fitTestFolder.Count; ++ index)
					{
						TreeNode childTreeNode = new TreeNode(fitTestFolder[index].FileName);
						parentTreeNode.Nodes.Add(childTreeNode);
						childTreeNode.Tag = fitTestFolder[index].GetHashCode();
					}
					treeView.EndUpdate();
				}
			}
		}

		private void RunTests()
		{
			TreeNode selectedNode = treeView.SelectedNode;

			if (selectedNode == null) return;

			if (selectedNode.Parent == null)
			{
				int folderHashCode = (int) selectedNode.Tag;
				RunFolder(fitTestFolderContainer.GetFolderByHashCode(folderHashCode));
			}
			else
			{
				int fileHashCode = (int) selectedNode.Tag;
				RunFile(fitTestFolderContainer.GetFileByHashCode(fileHashCode));
			}
		}

		private void RunFile(FitTestFile fitTestFile)
		{
			folderToDo = null;
			fileToDo = fitTestFile;
			executeJobEvent.Set();
		}

		private void RunFolder(FitTestFolder fitTestFolder)
		{
			folderToDo = fitTestFolder;
			fileToDo = null;
			executeJobEvent.Set();
		}

		private void MainForm_Load(object sender, EventArgs eventArgs)
		{
			FitTestContainer.Load(ref fitTestFolderContainer);
			RedrawTreeView(fitTestFolderContainer);
			ShowInputFileWebPage(@"about:blank");
			ShowOutputFileWebPage(@"about:blank");
		}

		private void RedrawTreeView(FitTestContainer fitTestContainer)
		{
			treeView.BeginUpdate();
			for (int folderIndex = 0; folderIndex < fitTestFolderContainer.Count; ++ folderIndex)
			{
				FitTestFolder fitTestFolder = fitTestFolderContainer[folderIndex];
				TreeNode parentTreeNode = new TreeNode(fitTestFolder.FolderName);
				parentTreeNode.Tag = fitTestFolder.GetHashCode();
				treeView.Nodes.Add(parentTreeNode);

				for (int fileIndex = 0; fileIndex < fitTestFolder.Count; ++ fileIndex)
				{
					FitTestFile fitTestFile = fitTestFolder[fileIndex];
					TreeNode childTreeNode = new TreeNode(fitTestFile.FileName);
					parentTreeNode.Nodes.Add(childTreeNode);
					childTreeNode.Tag = fitTestFolder[fileIndex].GetHashCode();
				}
			}
			treeView.EndUpdate();
		}

		private void MainForm_Closed(object sender, EventArgs eventArgs)
		{
			exitThreadEvent.Set();
			FitTestContainer.Save(fitTestFolderContainer);
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs eventArgs)
		{
			TreeNode selectedNode = eventArgs.Node;

			if (selectedNode == null || selectedNode.Parent == null)
			{
				ShowInputFileWebPage(@"about:blank");
				ShowOutputFileWebPage(@"about:blank");
			}
			else
			{
				int fileHashCode = (int) selectedNode.Tag;
				FitTestFile fitTestFile = fitTestFolderContainer.GetFileByHashCode(fileHashCode);
				int folderHashCode = fitTestFile.ParentHashCode;
				FitTestFolder fitTestFolder = fitTestFolderContainer.GetFolderByHashCode(folderHashCode);

				string fileName = fitTestFile.FileName;
				string inputFolder = fitTestFolder.InputFolder;
				string outputFolder = fitTestFolder.OutputFolder;

				ShowInputFileWebPage(Path.Combine(inputFolder, fileName));
				ShowOutputFileWebPage(Path.Combine(outputFolder, fileName));
			}
		}

		private void ShowInputFileWebPage(string uri)
		{
			ShowWebPageInSpecificControl(inputFileWebBrowser, uri);
		}

		private void ShowOutputFileWebPage(string uri)
		{
			ShowWebPageInSpecificControl(outputFileWebBrowser, uri);
		}

		private void ShowWebPageInSpecificControl(AxWebBrowser webBrowser, string uri)
		{
			object flags = null;
			object targetFrameName = null;
			object postData = null;
			object headers = null;

			webBrowser.Navigate(uri, ref flags, ref targetFrameName, ref postData, ref headers);
		}

		private void mainToolBar_ButtonClick(object sender, ToolBarButtonClickEventArgs eventArgs)
		{
			switch(mainToolBar.Buttons.IndexOf(eventArgs.Button))
			{
				case 0:
					NewFolder();
					break; 
				case 1:
					break; 
				case 2:
					break; 
				case 3:
					break; 
				case 4:
					RunTests();
					break; 
			}
		}

		public void WorkerThreadProc()
		{
			try
			{
				ManualResetEvent[] waitHandles = new ManualResetEvent[2];
				waitHandles[0] = executeJobEvent;
				waitHandles[1] = exitThreadEvent;
				while (true)
				{
					int waitHandleIndex = WaitHandle.WaitAny(waitHandles);
					if (waitHandleIndex == 1) break;
					if (fileToDo != null)
					{
						folderToDo = fitTestFolderContainer.GetFolderByHashCode(fileToDo.ParentHashCode);
						RunFitTest(folderToDo, fileToDo);
					}
					else
					{
						for (int fileIndex = 0; fileIndex < folderToDo.Count; ++ fileIndex)
						{
							FitTestFile fitTestFile = folderToDo[fileIndex];
							RunFitTest(folderToDo, fitTestFile);
							if (exitThreadEvent.WaitOne(0, false)) break;
						}
					}
					executeJobEvent.Reset();
				}
			}
			catch (Exception exception)
			{
				OnFatalError(exception);
			}
		}

		private void RunFitTest(FitTestFolder fitTestFolder, FitTestFile fitTestFile)
		{
			UpdateFileNodeBeforeTestExecution(fitTestFolder, fitTestFile);
			fitTestFile.TestRunProperties = ExecuteFit(
				Path.Combine(fitTestFolder.InputFolder, fitTestFile.FileName),
				Path.Combine(fitTestFolder.OutputFolder, fitTestFile.FileName),
				fitTestFolder.FixturePath);
			fitTestFile.isExecuted = true;
			UpdateFileNodeAfterTestExecution(fitTestFolder, fitTestFile);
		}

		private void UpdateFileNodeBeforeTestExecution(FitTestFolder fitTestFolder, FitTestFile fitTestFile)
		{
			TreeNode fileTreeNode = GetTreeNodeByHashCode(treeView.Nodes, fitTestFile.GetHashCode());
			TreeNode folderTreeNode = GetTreeNodeByHashCode(treeView.Nodes, fitTestFolder.GetHashCode());
			treeView.BeginUpdate();
			folderTreeNode.ImageIndex = 2;
			folderTreeNode.SelectedImageIndex = 2;
			fileTreeNode.ImageIndex = 2;
			fileTreeNode.SelectedImageIndex = 2;
			fileTreeNode.Text = fitTestFile.FileName + " ...";
			treeView.EndUpdate();
		}

		private void UpdateFileNodeAfterTestExecution(FitTestFolder fitTestFolder, FitTestFile fitTestFile)
		{
			TreeNode fileTreeNode = GetTreeNodeByHashCode(treeView.Nodes, fitTestFile.GetHashCode());
			TreeNode folderTreeNode = GetTreeNodeByHashCode(treeView.Nodes, fitTestFolder.GetHashCode());
			treeView.BeginUpdate();
			folderTreeNode.ImageIndex = 0;
			folderTreeNode.SelectedImageIndex = 0;

			if (GetFitTestFileFailedState(fitTestFile))
			{
				fileTreeNode.ImageIndex = 0;
				fileTreeNode.SelectedImageIndex = 0;
			}
			else
			{
				fileTreeNode.ImageIndex = 1;
				fileTreeNode.SelectedImageIndex = 1;
			}
			fileTreeNode.Text = string.Format("{0} ({1})", fitTestFile.FileName, fitTestFile.TestRunProperties.Counts);
			treeView.EndUpdate();
		}

		private TestRunProperties ExecuteFit(string inputFile, string outputFile, string fixturePath)
		{
			AppDomainSetup setup = new AppDomainSetup();
			setup.ApplicationBase = fixturePath.Split(';')[0];
			AppDomain fitGuiRunnerDomain = AppDomain.CreateDomain("FitGuiRunnerDomain", null, setup);
			string executingAssemblyPath = Path.GetDirectoryName(
				Assembly.GetExecutingAssembly().CodeBase.Replace("file:///", ""));
			string[] args = {inputFile, outputFile, fixturePath};
			fitGuiRunnerDomain.ExecuteAssembly(Path.Combine(executingAssemblyPath, FIT_GUI_RUNNER_NAME), null, args);
			AppDomain.Unload(fitGuiRunnerDomain);
			return commonData.TestRunProperties;
		}

		private TreeNode GetTreeNodeByHashCode(TreeNodeCollection nodes, int hashCode)
		{
			foreach (TreeNode node in nodes)
			{
				if ((int)node.Tag == hashCode)
				{
					return node;
				}
				else
				{
					if (node.Nodes.Count > 0)
					{
						TreeNode otherNode = GetTreeNodeByHashCode(node.Nodes, hashCode);
						if (node != null) return otherNode;
					}
				}
			}
			return null;
		}

		bool GetFitTestFolderFailedState(FitTestFolder fitTestFolder)
		{
			for (int fileIndex = 0; fileIndex < fitTestFolder.Count; ++ fileIndex)
			{
				if (GetFitTestFileFailedState(fitTestFolder[fileIndex]))
				{
					return true;
				}
			}
			return false;
		}

		bool GetFitTestFileFailedState(FitTestFile fitTestFile)
		{
			if (fitTestFile.isExecuted)
			{ 
				int[] integerCounts = GetIntegerCounts(fitTestFile.TestRunProperties.Counts);
				if (integerCounts[1] > 0 || integerCounts[3] > 0)
				{
					return true;
				}
			}
			return false;
		}

		int[] GetIntegerCounts(string counts)
		{
			int[] integerCounts = new int[4];
			string[] countsSplit = counts.Split(',');
			integerCounts[0] = Convert.ToInt32(countsSplit[0].Replace("right", ""));
			integerCounts[1] = Convert.ToInt32(countsSplit[1].Replace("wrong", ""));
			integerCounts[2] = Convert.ToInt32(countsSplit[2].Replace("ignored", ""));
			integerCounts[3] = Convert.ToInt32(countsSplit[3].Replace("exceptions", ""));
			return integerCounts;
		}
	}
}