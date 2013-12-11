using System;
using System.IO;
using fit.gui.common;
using Gtk;

namespace fit.gui.gtk
{
	public partial class TestsFolder : Gtk.Dialog
	{
		public TestsFolder()
		{
			Build();
		}

		private FitTestFolder fitTestFolder = null;

		public FitTestFolder FitTestFolder
		{
			get
			{
				return fitTestFolder;
			}
		}

//		private bool ShowFolderBrowserDialog(string description, ref string selectedPath)
//		{
//			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
//			{
//				folderBrowserDialog.Description = description;
//				folderBrowserDialog.SelectedPath = selectedPath;
//
//				if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
//				{
//					selectedPath = folderBrowserDialog.SelectedPath;
//					return true;
//				}
//				return false;
//			}
//		}
//
//		private void buttonBrowseInputWindowsFolder_Click(object sender, System.EventArgs e)
//		{
//			string inputWindowsFolder = textBoxInputFolder.Text;
//			if (ShowFolderBrowserDialog("Select Fit Test Specification Path", ref inputWindowsFolder))
//			{
//				textBoxInputFolder.Text = inputWindowsFolder;
//			}
//		}
//
//		private void buttonBrowseOutputWindowsFolder_Click(object sender, System.EventArgs e)
//		{
//			string outputFolder = textBoxOutputFolder.Text;
//			if (ShowFolderBrowserDialog("Select Fit Test Result Path", ref outputFolder))
//			{
//				textBoxOutputFolder.Text = outputFolder;
//			}
//		}
//
//		private void buttonOK_Click(object sender, System.EventArgs e)
//		{
//			DirectoryInfo inputFolderDirectoryInfo = new DirectoryInfo(textBoxInputFolder.Text);
//			if (!inputFolderDirectoryInfo.Exists)
//			{
//				textBoxInputFolder.Focus();
//				return;
//			}
//
//			DirectoryInfo outputFolderDirectoryInfo = new DirectoryInfo(textBoxOutputFolder.Text);
//			if (!outputFolderDirectoryInfo.Exists)
//			{
//				textBoxOutputFolder.Focus();
//				return;
//			}
//
//			DirectoryInfo fixturePathDirectoryInfo = new DirectoryInfo(textBoxFixturePath.Text);
//			if (!fixturePathDirectoryInfo.Exists)
//			{
//				textBoxFixturePath.Focus();
//				return;
//			}
//
//			FitTestFolder fitTestFolder = new FitTestFolder();
//			fitTestFolder.FolderName = textBoxName.Text;
//			fitTestFolder.InputFolder = textBoxInputFolder.Text;
//			fitTestFolder.OutputFolder = textBoxOutputFolder.Text;
//			fitTestFolder.FixturePath = textBoxFixturePath.Text;
//			this.fitTestFolder = fitTestFolder;
//			this.DialogResult = DialogResult.OK;
//		}
//
//		private void buttonBrowseFixturePath_Click(object sender, System.EventArgs e)
//		{
//			string fixturePath = textBoxFixturePath.Text;
//			if (ShowFolderBrowserDialog("Select Fit Test Fixture Path", ref fixturePath))
//			{
//				textBoxFixturePath.Text = fixturePath;
//			}
//		}

		protected void OnSpecificationsDirectoryPathSelectionChanged(object sender, EventArgs eventArgs)
		{
			entrySpecificationsDirectoryPath.Text = ((Gtk.FileChooserButton)sender).Filename;
		}

		protected void OnSpecificationsDirectoryPathFocusOutEvent(object sender, FocusOutEventArgs eventArgs)
		{
			var fileName = ((Gtk.Entry)sender).Text;
			if (Directory.Exists(fileName))
			{
				if (!filechooserbuttonSpecificationsDirectoryPath.SelectFilename(fileName))
					throw new Exception("Something wrong!"); 
			}
			else
				entrySpecificationsDirectoryPath.GrabFocus();
		}

		protected void OnResultsDirectoryPathFocusOutEvent(object sender, FocusOutEventArgs eventArgs)
		{
			var fileName = ((Gtk.Entry)sender).Text;
			if (Directory.Exists(fileName))
			{
				if (!filechooserbuttonResultsDirectoryPath.SelectFilename(fileName))
					throw new Exception("Something wrong!"); 
			}
			else
				entryResultsDirectoryPath.GrabFocus();
		}		

		protected void OnResultsDirectoryPathSelectionChanged(object sender, EventArgs eventArgs)
		{
			entryResultsDirectoryPath.Text = ((Gtk.FileChooserButton)sender).Filename;
		}

		protected void OnFixturesDirectoryPathSelectionChanged(object sender, EventArgs eventArgs)
		{
			entryFixturesDirectoryPath.Text = ((Gtk.FileChooserButton)sender).Filename;
		}		

		protected void OnFixturesDirectoryPathFocusOutevent(object sender, FocusOutEventArgs eventArgs)
		{
			var fileName = ((Gtk.Entry)sender).Text;
			if (Directory.Exists(fileName))
			{
				if (!filechooserbuttonFixturesDirectoryPath.SelectFilename(fileName))
					throw new Exception("Something wrong!"); 
			}
			else
				entryFixturesDirectoryPath.GrabFocus();
		}
	}
}