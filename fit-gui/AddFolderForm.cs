using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using fit.gui.common;

namespace fit.gui
{
	public class AddFolderForm : Form
	{
		private Label labelName;
		private TextBox textBoxName;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private Container components = null;
		private FitTestFolder fitTestFolder = null;
		private System.Windows.Forms.Label labelInputFolder;
		private System.Windows.Forms.Label labelOutputFolder;
		private System.Windows.Forms.Label labelFixturePath;
		private System.Windows.Forms.TextBox textBoxFixturePath;
		private System.Windows.Forms.Button buttonBrowseOutputFolder;
		private System.Windows.Forms.Button buttonBrowseFixturePath;
		private System.Windows.Forms.Button buttonBrowseInputFolder;
		private System.Windows.Forms.TextBox textBoxInputFolder;
		private System.Windows.Forms.TextBox textBoxOutputFolder;

		public FitTestFolder FitTestFolder
		{
			get
			{
				return fitTestFolder;
			}
		}

		public AddFolderForm()
		{
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
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.labelName = new System.Windows.Forms.Label();
			this.textBoxName = new System.Windows.Forms.TextBox();
			this.buttonBrowseInputFolder = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelInputFolder = new System.Windows.Forms.Label();
			this.labelOutputFolder = new System.Windows.Forms.Label();
			this.textBoxInputFolder = new System.Windows.Forms.TextBox();
			this.textBoxOutputFolder = new System.Windows.Forms.TextBox();
			this.buttonBrowseOutputFolder = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelFixturePath = new System.Windows.Forms.Label();
			this.textBoxFixturePath = new System.Windows.Forms.TextBox();
			this.buttonBrowseFixturePath = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// labelName
			// 
			this.labelName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.labelName.Location = new System.Drawing.Point(32, 18);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(96, 20);
			this.labelName.TabIndex = 0;
			this.labelName.Text = "Folder Name";
			// 
			// textBoxName
			// 
			this.textBoxName.Location = new System.Drawing.Point(136, 16);
			this.textBoxName.Name = "textBoxName";
			this.textBoxName.Size = new System.Drawing.Size(384, 20);
			this.textBoxName.TabIndex = 1;
			this.textBoxName.Text = "Fit Tests";
			// 
			// buttonBrowseInputFolder
			// 
			this.buttonBrowseInputFolder.Location = new System.Drawing.Point(528, 46);
			this.buttonBrowseInputFolder.Name = "buttonBrowseInputFolder";
			this.buttonBrowseInputFolder.TabIndex = 3;
			this.buttonBrowseInputFolder.Text = "Browse...";
			this.buttonBrowseInputFolder.Click += new System.EventHandler(this.buttonBrowseInputWindowsFolder_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(448, 152);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 8;
			this.buttonOK.Text = "OK";
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// labelInputFolder
			// 
			this.labelInputFolder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.labelInputFolder.Location = new System.Drawing.Point(32, 50);
			this.labelInputFolder.Name = "labelInputFolder";
			this.labelInputFolder.Size = new System.Drawing.Size(96, 20);
			this.labelInputFolder.TabIndex = 0;
			this.labelInputFolder.Text = "Specification path";
			// 
			// labelOutputFolder
			// 
			this.labelOutputFolder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.labelOutputFolder.Location = new System.Drawing.Point(32, 82);
			this.labelOutputFolder.Name = "labelOutputFolder";
			this.labelOutputFolder.Size = new System.Drawing.Size(96, 20);
			this.labelOutputFolder.TabIndex = 0;
			this.labelOutputFolder.Text = "Result path";
			// 
			// textBoxInputFolder
			// 
			this.textBoxInputFolder.Location = new System.Drawing.Point(136, 48);
			this.textBoxInputFolder.Name = "textBoxInputFolder";
			this.textBoxInputFolder.Size = new System.Drawing.Size(384, 20);
			this.textBoxInputFolder.TabIndex = 2;
			this.textBoxInputFolder.Text = "";
			// 
			// textBoxOutputFolder
			// 
			this.textBoxOutputFolder.Location = new System.Drawing.Point(136, 80);
			this.textBoxOutputFolder.Name = "textBoxOutputFolder";
			this.textBoxOutputFolder.Size = new System.Drawing.Size(384, 20);
			this.textBoxOutputFolder.TabIndex = 4;
			this.textBoxOutputFolder.Text = "";
			// 
			// buttonBrowseOutputFolder
			// 
			this.buttonBrowseOutputFolder.Location = new System.Drawing.Point(528, 78);
			this.buttonBrowseOutputFolder.Name = "buttonBrowseOutputFolder";
			this.buttonBrowseOutputFolder.TabIndex = 5;
			this.buttonBrowseOutputFolder.Text = "Browse...";
			this.buttonBrowseOutputFolder.Click += new System.EventHandler(this.buttonBrowseOutputWindowsFolder_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Location = new System.Drawing.Point(528, 152);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.TabIndex = 9;
			this.buttonCancel.Text = "Cancel";
			// 
			// labelFixturePath
			// 
			this.labelFixturePath.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.labelFixturePath.Location = new System.Drawing.Point(32, 114);
			this.labelFixturePath.Name = "labelFixturePath";
			this.labelFixturePath.Size = new System.Drawing.Size(96, 20);
			this.labelFixturePath.TabIndex = 0;
			this.labelFixturePath.Text = "Fixture path";
			// 
			// textBoxFixturePath
			// 
			this.textBoxFixturePath.Location = new System.Drawing.Point(136, 112);
			this.textBoxFixturePath.Name = "textBoxFixturePath";
			this.textBoxFixturePath.Size = new System.Drawing.Size(384, 20);
			this.textBoxFixturePath.TabIndex = 6;
			this.textBoxFixturePath.Text = "";
			// 
			// buttonBrowseFixturePath
			// 
			this.buttonBrowseFixturePath.Location = new System.Drawing.Point(528, 110);
			this.buttonBrowseFixturePath.Name = "buttonBrowseFixturePath";
			this.buttonBrowseFixturePath.TabIndex = 7;
			this.buttonBrowseFixturePath.Text = "Browse...";
			this.buttonBrowseFixturePath.Click += new System.EventHandler(this.buttonBrowseFixturePath_Click);
			// 
			// AddFolderForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(616, 188);
			this.Controls.Add(this.buttonBrowseFixturePath);
			this.Controls.Add(this.textBoxFixturePath);
			this.Controls.Add(this.labelFixturePath);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonBrowseOutputFolder);
			this.Controls.Add(this.textBoxOutputFolder);
			this.Controls.Add(this.textBoxInputFolder);
			this.Controls.Add(this.textBoxName);
			this.Controls.Add(this.labelOutputFolder);
			this.Controls.Add(this.labelInputFolder);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.buttonBrowseInputFolder);
			this.Controls.Add(this.labelName);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AddFolderForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Add new Fit test folder";
			this.ResumeLayout(false);

		}
		#endregion

		private bool ShowFolderBrowserDialog(string description, ref string selectedPath)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.Description = description;
				folderBrowserDialog.SelectedPath = selectedPath;

				if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
				{
					selectedPath = folderBrowserDialog.SelectedPath;
					return true;
				}
				return false;
			}
		}

		private void buttonBrowseInputWindowsFolder_Click(object sender, System.EventArgs e)
		{
			string inputWindowsFolder = textBoxInputFolder.Text;
			if (ShowFolderBrowserDialog("Select Fit Test Specification Path", ref inputWindowsFolder))
			{
				textBoxInputFolder.Text = inputWindowsFolder;
			}
		}

		private void buttonBrowseOutputWindowsFolder_Click(object sender, System.EventArgs e)
		{
			string outputFolder = textBoxOutputFolder.Text;
			if (ShowFolderBrowserDialog("Select Fit Test Result Path", ref outputFolder))
			{
				textBoxOutputFolder.Text = outputFolder;
			}
		}

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			DirectoryInfo inputFolderDirectoryInfo = new DirectoryInfo(textBoxInputFolder.Text);
			if (!inputFolderDirectoryInfo.Exists)
			{
				textBoxInputFolder.Focus();
				return;
			}

			DirectoryInfo outputFolderDirectoryInfo = new DirectoryInfo(textBoxOutputFolder.Text);
			if (!outputFolderDirectoryInfo.Exists)
			{
				textBoxOutputFolder.Focus();
				return;
			}

			DirectoryInfo fixturePathDirectoryInfo = new DirectoryInfo(textBoxFixturePath.Text);
			if (!fixturePathDirectoryInfo.Exists)
			{
				textBoxFixturePath.Focus();
				return;
			}

			FitTestFolder fitTestFolder = new FitTestFolder();
			fitTestFolder.FolderName = textBoxName.Text;
			fitTestFolder.InputFolder = textBoxInputFolder.Text;
			fitTestFolder.OutputFolder = textBoxOutputFolder.Text;
			fitTestFolder.FixturePath = textBoxFixturePath.Text;
			this.fitTestFolder = fitTestFolder;
			this.DialogResult = DialogResult.OK;
		}

		private void buttonBrowseFixturePath_Click(object sender, System.EventArgs e)
		{
			string fixturePath = textBoxFixturePath.Text;
			if (ShowFolderBrowserDialog("Select Fit Test Fixture Path", ref fixturePath))
			{
				textBoxFixturePath.Text = fixturePath;
			}
		}
	}
}
