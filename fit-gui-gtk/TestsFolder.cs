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

		public FitTestFolder FitTestFolder
		{
			get;
			private set;
		}

		private static void SelectFolder(Gtk.Entry entry, Window parent)
		{
			var dialog = new Gtk.FileChooserDialog("Select directory", parent, 
			                                  FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel,
			                                  "Select", ResponseType.Accept); 
			try
			{
				if (dialog.Run() == (int)ResponseType.Accept)
					entry.Text = dialog.Filename;			
			}
			finally
			{
				dialog.Destroy();
			}
		}

		protected void OnButtonSpecificationsDirectoryPathClicked(object sender, EventArgs eventArgs)
		{
			SelectFolder(entrySpecificationsDirectoryPath, this);
		}

		protected void OnButtonResultsDirectoryPathClicked(object sender, EventArgs eventArgs)
		{
			SelectFolder(entryResultsDirectoryPath, this);
		}

		protected void OnButtonFixturesDirectoryPathClicked(object sender, EventArgs eventArgs)
		{
			SelectFolder(entryFixturesDirectoryPath, this);
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