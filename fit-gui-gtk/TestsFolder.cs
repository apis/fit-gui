using System;
using System.IO;
using fit.gui.common;
using Gtk;

namespace fit.gui.gtk
{
	public partial class TestsFolder : Gtk.Dialog
	{
		public string CurrentDirectory
		{
			get;
			private set;
		}

		public FitTestFolder FitTestFolder
		{
			get;
			private set;
		}

		public TestsFolder(string currentDirectory)
		{
			Build();
			CurrentDirectory = currentDirectory;
		}

		private void SelectFolder(string title, Gtk.Entry entry, Window parent)
		{
			var dialog = new Gtk.FileChooserDialog(title, parent, 
			                                  FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel,
			                                  "Select", ResponseType.Accept); 
			try
			{
				if (Directory.Exists(entry.Text))
				{
					dialog.SetCurrentFolder(entry.Text);
				}
				else
				{
					dialog.SetCurrentFolder(CurrentDirectory);
				}

				if (dialog.Run() == (int)ResponseType.Accept)
				{
					entry.Text = dialog.Filename;
					CurrentDirectory = dialog.CurrentFolder;
				}
			}
			finally
			{
				dialog.Destroy();
			}
		}

		protected void OnButtonSpecificationsDirectoryPathClicked(object sender, EventArgs eventArgs)
		{
			SelectFolder("Select Specifications Directory", entrySpecificationsDirectoryPath, this);
		}

		protected void OnButtonResultsDirectoryPathClicked(object sender, EventArgs eventArgs)
		{
			SelectFolder("Select Results Directory", entryResultsDirectoryPath, this);
		}

		protected void OnButtonFixturesDirectoryPathClicked(object sender, EventArgs eventArgs)
		{
			SelectFolder("Select Fixtures Directory", entryFixturesDirectoryPath, this);
		}

		protected void OnButtonOkClicked(object sender, EventArgs eventArgs)
		{
			if (string.IsNullOrEmpty(entryFolderName.Text))
			{
				entryFolderName.GrabFocus();
				return;
			}

			if (!Directory.Exists(entrySpecificationsDirectoryPath.Text))
			{
				entrySpecificationsDirectoryPath.GrabFocus();
				return;
			}

			if (!Directory.Exists(entryResultsDirectoryPath.Text))
			{
				entryResultsDirectoryPath.GrabFocus();
				return;
			}

			if (!Directory.Exists(entryFixturesDirectoryPath.Text))
			{
				entryFixturesDirectoryPath.GrabFocus();
				return;
			}

			FitTestFolder fitTestFolder = new FitTestFolder();
			fitTestFolder.FolderName = entryFolderName.Text;
			fitTestFolder.InputFolder = entrySpecificationsDirectoryPath.Text;
			fitTestFolder.OutputFolder = entryResultsDirectoryPath.Text;
			fitTestFolder.FixturePath = entryFixturesDirectoryPath.Text;
			FitTestFolder = fitTestFolder;

			Respond(ResponseType.Ok);
		}
	}
}