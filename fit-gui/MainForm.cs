using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Threading;
using System.Windows.Forms;
//using Mono.WebBrowser;
using AxSHDocVw;
using fit.gui.common;

// Show assembly installed in GAC:
// http://support.microsoft.com/default.aspx?scid=kb;en-us;Q306149

// Dynamic properties discussion
// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dv_vstechart/html/vbtchcreateyourowndynamicpropertiespreservepropertysettingsinvisualbasicnet.asp

// TODO: Implement Stop functionality

namespace fit.gui
{
	public class MainForm : Form
	{
		private static Configuration configuration = Configuration.Load();
		private FitTestContainer fitTestFolderContainer = new FitTestContainer(configuration);
		private FitTestRunner fitTestRunner = null;
		private Rectangle normalStateBounds = Rectangle.Empty;
		
		private TreeView treeView;
		private Panel panel2;
		private Splitter splitter1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.ImageList treeViewImageList;
		private System.Windows.Forms.MenuItem newFitTestFolderMenuItem;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.MenuItem exitMenuItem;
		private System.Windows.Forms.MenuItem aboutMenuItem;
		private System.Windows.Forms.StatusBar mainStatusBar;
		private System.Windows.Forms.MenuItem startMenuItem;
		private AxSHDocVw.AxWebBrowser inputFileWebBrowser;
		private AxSHDocVw.AxWebBrowser outputFileWebBrowser;
//		private WebBrowser inputFileWebBrowser;
//		private WebBrowser outputFileWebBrowser;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.ToolBar mainToolBar;
		private System.Windows.Forms.ToolBarButton showResultPaneToolBarButton;
		private System.Windows.Forms.ToolBarButton showSpecificationPaneToolBarButton;
		private System.Windows.Forms.ImageList mainToolbarImageList;
		private System.Windows.Forms.ToolBarButton SeparatorToolBarButton;
		private System.Windows.Forms.ToolBarButton startToolBarButton;
		private System.Windows.Forms.MenuItem removeFolderMenuItem;
		private fit.gui.ProgressBar mainProgressBar;
		private System.Windows.Forms.ContextMenu treeViewContextMenu;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MainMenu mainMenu;

		public MainForm()
		{
			fitTestRunner = new FitTestRunner(fitTestFolderContainer);
			fitTestRunner.FitTestRunStartedEventSink += new FitTestRunStartedEventDelegate(FitTestRunStartedEventHandler);
			fitTestRunner.FitTestRunStoppedEventSink += new FitTestRunStoppedEventDelegate(FitTestRunStoppedEventHandler);
			fitTestRunner.FitTestStartedEventSink += new FitTestStartedEventDelegate(FitTestStartedEventHandler);
			fitTestRunner.FitTestStoppedEventSink += new FitTestStoppedEventDelegate(FitTestStoppedEventHandler);
			fitTestRunner.ErrorEvent += OnErrorEvent;
			InitializeComponent();
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

		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainForm));
			this.treeView = new System.Windows.Forms.TreeView();
			this.treeViewContextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
			this.panel2 = new System.Windows.Forms.Panel();
			this.panel3 = new System.Windows.Forms.Panel();
//			this.inputFileWebBrowser = new WebBrowser();
//			this.outputFileWebBrowser = new WebBrowser();
			this.inputFileWebBrowser = new AxSHDocVw.AxWebBrowser();
			this.outputFileWebBrowser = new AxSHDocVw.AxWebBrowser();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.mainStatusBar = new System.Windows.Forms.StatusBar();
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.exitMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.startMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.newFitTestFolderMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.removeFolderMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.aboutMenuItem = new System.Windows.Forms.MenuItem();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.mainProgressBar = new fit.gui.ProgressBar();
			this.mainToolBar = new System.Windows.Forms.ToolBar();
			this.showSpecificationPaneToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.showResultPaneToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.SeparatorToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.startToolBarButton = new System.Windows.Forms.ToolBarButton();
			this.mainToolbarImageList = new System.Windows.Forms.ImageList(this.components);
			this.panel2.SuspendLayout();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.inputFileWebBrowser)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.outputFileWebBrowser)).BeginInit();
			this.panel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.ContextMenu = this.treeViewContextMenu;
			this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.treeView.ForeColor = System.Drawing.SystemColors.WindowText;
			this.treeView.HideSelection = false;
			this.treeView.ImageIndex = 3;
			this.treeView.ImageList = this.treeViewImageList;
			this.treeView.Location = new System.Drawing.Point(0, 0);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 3;
			this.treeView.Size = new System.Drawing.Size(235, 426);
			this.treeView.TabIndex = 0;
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// treeViewContextMenu
			// 
			this.treeViewContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																								this.menuItem2,
																								this.menuItem4,
																								this.menuItem6,
																								this.menuItem5,
																								this.menuItem7,
																								this.menuItem9,
																								this.menuItem11,
																								this.menuItem12});
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 0;
			this.menuItem2.Text = "&Start";
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "-";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 2;
			this.menuItem6.Text = "Show s&pecification pane";
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 3;
			this.menuItem5.Text = "Show &result pane";
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 4;
			this.menuItem7.Text = "-";
			// 
			// menuItem9
			// 
			this.menuItem9.Index = 5;
			this.menuItem9.Text = "&New folder...";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 6;
			this.menuItem11.Text = "&Edit folder...";
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 7;
			this.menuItem12.Text = "&Remove folder...";
			// 
			// treeViewImageList
			// 
			this.treeViewImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImageList.ImageStream")));
			this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.panel3);
			this.panel2.Controls.Add(this.splitter1);
			this.panel2.Controls.Add(this.treeView);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 111);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(905, 426);
			this.panel2.TabIndex = 9;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.inputFileWebBrowser);
			this.panel3.Controls.Add(this.outputFileWebBrowser);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel3.Location = new System.Drawing.Point(239, 0);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(666, 426);
			this.panel3.TabIndex = 16;
			// 
			// inputFileWebBrowser
			// 
			this.inputFileWebBrowser.ContainingControl = this;
			this.inputFileWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.inputFileWebBrowser.Enabled = true;
			this.inputFileWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.inputFileWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("inputFileWebBrowser.OcxState")));
			this.inputFileWebBrowser.Size = new System.Drawing.Size(666, 426);
			this.inputFileWebBrowser.TabIndex = 14;
			// 
			// outputFileWebBrowser
			// 
			this.outputFileWebBrowser.ContainingControl = this;
			this.outputFileWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outputFileWebBrowser.Enabled = true;
			this.outputFileWebBrowser.Location = new System.Drawing.Point(0, 0);
			this.outputFileWebBrowser.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("outputFileWebBrowser.OcxState")));
			this.outputFileWebBrowser.Size = new System.Drawing.Size(666, 426);
			this.outputFileWebBrowser.TabIndex = 15;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(235, 0);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(4, 426);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// mainStatusBar
			// 
			this.mainStatusBar.Location = new System.Drawing.Point(0, 537);
			this.mainStatusBar.Name = "mainStatusBar";
			this.mainStatusBar.Size = new System.Drawing.Size(905, 30);
			this.mainStatusBar.TabIndex = 10;
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
																					  this.exitMenuItem});
			this.menuItem1.Text = "&File";
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Index = 0;
			this.exitMenuItem.Text = "E&xit";
			this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 1;
			this.menuItem3.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.startMenuItem,
																					  this.menuItem10,
																					  this.menuItem14,
																					  this.menuItem15,
																					  this.menuItem13,
																					  this.newFitTestFolderMenuItem,
																					  this.menuItem8,
																					  this.removeFolderMenuItem});
			this.menuItem3.Text = "&Tests";
			// 
			// startMenuItem
			// 
			this.startMenuItem.Index = 0;
			this.startMenuItem.Text = "&Start";
			this.startMenuItem.Click += new System.EventHandler(this.StartMenuItem_Click);
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 1;
			this.menuItem10.Text = "-";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 2;
			this.menuItem14.Text = "Show s&pecification pane";
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 3;
			this.menuItem15.Text = "Show &result pane";
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 4;
			this.menuItem13.Text = "-";
			// 
			// newFitTestFolderMenuItem
			// 
			this.newFitTestFolderMenuItem.Index = 5;
			this.newFitTestFolderMenuItem.Text = "&New folder...";
			this.newFitTestFolderMenuItem.Click += new System.EventHandler(this.NewFitTestFolderMenuItem_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Enabled = false;
			this.menuItem8.Index = 6;
			this.menuItem8.Text = "&Edit folder...";
			// 
			// removeFolderMenuItem
			// 
			this.removeFolderMenuItem.Index = 7;
			this.removeFolderMenuItem.Text = "&Remove folder...";
			this.removeFolderMenuItem.Click += new System.EventHandler(this.RemoveFolderMenuItem_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 2;
			this.menuItem20.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					   this.aboutMenuItem});
			this.menuItem20.Text = "&Help";
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Index = 0;
			this.aboutMenuItem.Text = "&About fit-gui...";
			this.aboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.groupBox1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 32);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(905, 79);
			this.panel1.TabIndex = 11;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.mainProgressBar);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(905, 79);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// mainProgressBar
			// 
			this.mainProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.mainProgressBar.Color = System.Drawing.Color.Navy;
			this.mainProgressBar.Location = new System.Drawing.Point(22, 36);
			this.mainProgressBar.Maximum = 100;
			this.mainProgressBar.Minimum = 0;
			this.mainProgressBar.Name = "mainProgressBar";
			this.mainProgressBar.Size = new System.Drawing.Size(862, 23);
			this.mainProgressBar.Step = 10;
			this.mainProgressBar.TabIndex = 0;
			this.mainProgressBar.Value = 0;
			// 
			// mainToolBar
			// 
			this.mainToolBar.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
																						   this.showSpecificationPaneToolBarButton,
																						   this.showResultPaneToolBarButton,
																						   this.SeparatorToolBarButton,
																						   this.startToolBarButton});
			this.mainToolBar.DropDownArrows = true;
			this.mainToolBar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.mainToolBar.ImageList = this.mainToolbarImageList;
			this.mainToolBar.Location = new System.Drawing.Point(0, 0);
			this.mainToolBar.Name = "mainToolBar";
			this.mainToolBar.ShowToolTips = true;
			this.mainToolBar.Size = new System.Drawing.Size(905, 32);
			this.mainToolBar.TabIndex = 12;
			this.mainToolBar.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
			this.mainToolBar.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.mainToolBar_ButtonClick);
			// 
			// showSpecificationPaneToolBarButton
			// 
			this.showSpecificationPaneToolBarButton.ImageIndex = 0;
			this.showSpecificationPaneToolBarButton.Pushed = true;
			this.showSpecificationPaneToolBarButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.showSpecificationPaneToolBarButton.ToolTipText = "Show specification pane";
			// 
			// showResultPaneToolBarButton
			// 
			this.showResultPaneToolBarButton.ImageIndex = 1;
			this.showResultPaneToolBarButton.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
			this.showResultPaneToolBarButton.ToolTipText = "Show result pane";
			// 
			// SeparatorToolBarButton
			// 
			this.SeparatorToolBarButton.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
			// 
			// startToolBarButton
			// 
			this.startToolBarButton.ImageIndex = 2;
			this.startToolBarButton.Text = "Start";
			this.startToolBarButton.ToolTipText = "Start test(s)";
			// 
			// mainToolbarImageList
			// 
			this.mainToolbarImageList.ImageSize = new System.Drawing.Size(20, 20);
			this.mainToolbarImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("mainToolbarImageList.ImageStream")));
			this.mainToolbarImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 16);
			this.ClientSize = new System.Drawing.Size(905, 567);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.mainStatusBar);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.mainToolBar);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "fit-gui";
			this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.LocationChanged += new System.EventHandler(this.MainForm_LocationChanged);
			this.Closed += new System.EventHandler(this.MainForm_Closed);
			this.panel2.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.inputFileWebBrowser)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.outputFileWebBrowser)).EndInit();
			this.panel1.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		public static void OnFatalError(System.Exception exception)
		{
			MessageBox.Show(null, 
				string.Format("Exception: {0}\nMessage: {1}\nSource: {2}\nStack Trace:\n{3}", 
				exception.GetType().FullName, exception.Message, exception.Source, exception.StackTrace), 
				"Fatal error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			Environment.Exit(-1);
		}

		[STAThread]
		private static void Main()
		{
			try
			{
				Application.Run(new MainForm());
				Environment.Exit(0);
			}
			catch (System.Exception exception)
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
				fitTestRunner.RunFolder(fitTestFolderContainer.GetFolderByHashCode(folderHashCode));
			}
			else
			{
				int fileHashCode = (int) selectedNode.Tag;
				fitTestRunner.RunFile(fitTestFolderContainer.GetFileByHashCode(fileHashCode));
			}
		}

		private void MainForm_Load(object sender, EventArgs eventArgs)
		{
			if (configuration.mainFormPropertiesLoaded)
			{
				Size.Width = configuration.WindowWidth;
				Size.Height = configuration.WindowHeight;
				Location.X = configuration.WindowLocationX;
				Location.Y = configuration.WindowLocationY;
				WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), configuration.WindowState);
				treeView.Size = new Size(configuration.mainFormTreeViewSizeWidth,
					treeView.Size.Height);
			}
			fitTestFolderContainer.Load();
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
			treeView.ExpandAll();
			treeView.EndUpdate();
		}

		private void RedrawTreeViewBeforeTestRun(FitTestContainer fitTestContainer)
		{
			// TODO: Optimize executed flag handling
			fitTestContainer.ResetExecutedFlag();
			treeView.BeginUpdate();
			for (int folderIndex = 0; folderIndex < fitTestFolderContainer.Count; ++ folderIndex)
			{
				FitTestFolder fitTestFolder = fitTestFolderContainer[folderIndex];
				TreeNode parentTreeNode = GetTreeNodeByHashCode(treeView.Nodes, fitTestFolder.GetHashCode());
				parentTreeNode.Text = fitTestFolder.FolderName;
				parentTreeNode.ImageIndex = 3;
				parentTreeNode.SelectedImageIndex = 3;

				for (int fileIndex = 0; fileIndex < fitTestFolder.Count; ++ fileIndex)
				{
					FitTestFile fitTestFile = fitTestFolder[fileIndex];
					TreeNode childTreeNode = GetTreeNodeByHashCode(treeView.Nodes, fitTestFile.GetHashCode());
					childTreeNode.Text = fitTestFile.FileName;
					childTreeNode.ImageIndex = 3;
					childTreeNode.SelectedImageIndex = 3;
				}
			}
			treeView.EndUpdate();
		}

		private void treeView_AfterSelect(object sender, TreeViewEventArgs eventArgs)
		{
			TreeNode selectedNode = eventArgs.Node;

			UpdatePanesForTreeNode(selectedNode);
		}

		private void UpdatePanesForTreeNode(TreeNode selectedNode)
		{
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

		private void ShowWebPageInSpecificControl(WebBrowser webBrowser, string uri)
		{
			object flags = null;
			object targetFrameName = null;
			object postData = null;
			object headers = null;

			webBrowser.Navigate(uri, ref flags, ref targetFrameName, ref postData, ref headers);
//			webBrowser.Navigate(uri);
		}

		private void UpdateFileNodeBeforeTestExecution(FitTestFolder fitTestFolder, FitTestFile fitTestFile)
		{
			TreeNode fileTreeNode = GetTreeNodeByHashCode(treeView.Nodes, fitTestFile.GetHashCode());
			TreeNode folderTreeNode = GetTreeNodeByHashCode(treeView.Nodes, fitTestFolder.GetHashCode());
			fileTreeNode.Text = fitTestFile.FileName + " ...";
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

			if (GetFitTestFolderFailedState(fitTestFolder))
			{
				folderTreeNode.ImageIndex = 0;
				folderTreeNode.SelectedImageIndex = 0;
				mainProgressBar.Color = Color.Red;
			}
			else
			{
				folderTreeNode.ImageIndex = 1;
				folderTreeNode.SelectedImageIndex = 1;
			}

			mainProgressBar.PerformStep();

			fileTreeNode.Text = string.Format("{0} ({1})", fitTestFile.FileName, 
				GetCountsString(fitTestFile.TestRunProperties));
			treeView.EndUpdate();
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
						if (otherNode != null) return otherNode;
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
				if (fitTestFile.TestRunProperties.countsWrong > 0 || 
					fitTestFile.TestRunProperties.countsExceptions > 0)
				{
					return true;
				}
			}
			return false;
		}

		string GetCountsString(TestRunProperties testRunProperties)
		{
			return string.Format("{0} right, {1} wrong, {2} ignored, {3} exceptions, {4} time", 
				testRunProperties.countsRight, testRunProperties.countsWrong,
				testRunProperties.countsIgnores, testRunProperties.countsExceptions,
				testRunProperties.RunElapsedTime);
		}

		private void NewFitTestFolderMenuItem_Click(object sender, System.EventArgs e)
		{
			NewFolder();
		}

		private void StartButton_Click(object sender, System.EventArgs e)
		{
			RunTests();
		}

		private void ExitMenuItem_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void AboutMenuItem_Click(object sender, System.EventArgs e)
		{
			using (AboutForm aboutForm = new AboutForm())
			{
				aboutForm.ShowDialog(this);
			}
		}

		private void StartMenuItem_Click(object sender, System.EventArgs e)
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

		private void mainToolBar_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs eventArgs)
		{
			switch (mainToolBar.Buttons.IndexOf(eventArgs.Button))
			{
				case 0:
					outputFileWebBrowser.SendToBack();
					showSpecificationPaneToolBarButton.Pushed = true;
					showResultPaneToolBarButton.Pushed = false;
					break;

				case 1:
					inputFileWebBrowser.SendToBack();
					showSpecificationPaneToolBarButton.Pushed = false;
					showResultPaneToolBarButton.Pushed = true;
					break;

				case 3:
					if (fitTestRunner.State == FitTestRunnerStates.Running)
					{
						fitTestRunner.Stop();
					}
					else
					{
						RunTests();
					}
					break;
			}
		}

		private void RemoveFolderMenuItem_Click(object sender, EventArgs eventArgs)
		{
			TreeNode selectedNode = treeView.SelectedNode;

			if (selectedNode.Parent == null)
			{
				FitTestFolder fitTestFolder = fitTestFolderContainer.GetFolderByHashCode((int)selectedNode.Tag);
				DialogResult dialogResult = MessageBox.Show(this, "Remove '" + fitTestFolder.FolderName + "' Fit Test Folder?", 
					"Remove Fit Test Folder", MessageBoxButtons.OKCancel, MessageBoxIcon.Question,
					MessageBoxDefaultButton.Button2);
				if (dialogResult == DialogResult.OK)
				{
					fitTestFolderContainer.Remove(fitTestFolder);
					treeView.Nodes.Remove(selectedNode);
				}
			}
		}

		private void FitTestRunStartedEventHandler(int numberOfTestsToDo)
		{
			startToolBarButton.Text = "Stop";
			startToolBarButton.ToolTipText = "Stop test(s)";
			startToolBarButton.ImageIndex = 3;
			startMenuItem.Text = "Stop";

			RedrawTreeViewBeforeTestRun(fitTestFolderContainer);
			mainProgressBar.Color = Color.LimeGreen;

			mainProgressBar.Minimum = 0;
			mainProgressBar.Maximum = numberOfTestsToDo;
			mainProgressBar.Value = 0;
			mainProgressBar.Step = 1;
		}

		private void FitTestRunStoppedEventHandler(bool isAborted)
		{
			// TODO: If menu is open it doesn't update Text for item right away ?
			if (isAborted)
			{
				mainProgressBar.Value = mainProgressBar.Maximum;
			}
			startToolBarButton.Text = "Start";
			startToolBarButton.ToolTipText = "Start test(s)";
			startToolBarButton.ImageIndex = 2;
			startMenuItem.Text = "Start";
		}

		private void FitTestStartedEventHandler(FitTestFile fitTestFile)
		{
			FitTestFolder fitTestFolder = fitTestFolderContainer.GetFolderByHashCode(fitTestFile.ParentHashCode);
			UpdateFileNodeBeforeTestExecution(fitTestFolder, fitTestFile);
		}

		private void FitTestStoppedEventHandler(FitTestFile fitTestFile)
		{
			FitTestFolder fitTestFolder = fitTestFolderContainer.GetFolderByHashCode(fitTestFile.ParentHashCode);
			UpdateFileNodeAfterTestExecution(fitTestFolder, fitTestFile);
			if (treeView.SelectedNode == GetTreeNodeByHashCode(treeView.Nodes, fitTestFile.GetHashCode()))
			{
				UpdatePanesForTreeNode(treeView.SelectedNode);
			}
		}

		private void OnErrorEvent(object sender, fit.gui.common.ErrorEventArgs args)
		{
			OnFatalError(args.Exception);
		}

		private void MainForm_Closed(object sender, System.EventArgs e)
		{
			fitTestRunner.ErrorEvent -= OnErrorEvent;
			fitTestRunner.FitTestStoppedEventSink += new FitTestStoppedEventDelegate(FitTestStoppedEventHandler);
			fitTestRunner.FitTestStartedEventSink += new FitTestStartedEventDelegate(FitTestStartedEventHandler);
			fitTestRunner.FitTestRunStoppedEventSink += new FitTestRunStoppedEventDelegate(FitTestRunStoppedEventHandler);
			fitTestRunner.FitTestRunStartedEventSink += new FitTestRunStartedEventDelegate(FitTestRunStartedEventHandler);
			fitTestRunner = null;

			configuration.WindowWidth = normalStateBounds.Size.Width;
			configuration.WindowHeight = normalStateBounds.Size.Height;
			configuration.WindowLocationX = normalStateBounds.Location.X;
			configuration.WindowLocationY = normalStateBounds.Location.Y;
			configuration.WindowState = WindowState.ToString();
			configuration.mainFormTreeViewSizeWidth = treeView.Size.Width; 
			Configuration.Save(configuration);
		}

		private void MainForm_SizeChanged(object sender, System.EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
			{
				normalStateBounds.Size = Size;
			}
		}

		private void MainForm_LocationChanged(object sender, System.EventArgs e)
		{
			if (WindowState == FormWindowState.Normal)
			{
				normalStateBounds.Location = Location;
			}
		}
	}
}