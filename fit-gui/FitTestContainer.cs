using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace fit.gui
{
	[Serializable]
	public class FitTestContainer
	{
		private Configuration configuration = null;
		private ArrayList fitTestFolders = new ArrayList();

		public FitTestContainer(Configuration configuration)
		{
			this.configuration = configuration;
		}

		public void Remove(FitTestFolder fitTestFolder)
		{
			fitTestFolders.Remove(fitTestFolder);
			Save();
		}

		private int InternalAdd(FitTestFolder fitTestFolder)
		{
			string[] filePatterns = fitTestFolder.FileMask.Split(';');
			
			foreach (string filePattern in filePatterns)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(fitTestFolder.InputFolder);
				FileInfo[] files  = directoryInfo.GetFiles(filePattern);
				for (int index = 0; index < files.Length; ++ index)
				{
					FitTestFile fitTestFile = new FitTestFile();
					fitTestFile.FileName = files[index].Name;
					fitTestFile.ParentHashCode = fitTestFolder.GetHashCode();
					fitTestFolder.Add(fitTestFile);
				}
			}
			return fitTestFolders.Add(fitTestFolder);
		}

		public int Add(FitTestFolder fitTestFolder)
		{
			int folderIndex = InternalAdd(fitTestFolder);
			Save();
			return folderIndex;
		}

		public FitTestFolder this[int folderIndex]
		{
			get
			{
				return (FitTestFolder)fitTestFolders[folderIndex];
			}

			set
			{
				fitTestFolders[folderIndex] = value;
			}
		}

		public int Count 
		{
			get
			{
				return fitTestFolders.Count;
			}
		}

		public void ResetExecutedFlag()
		{
			for (int folderIndex = 0; folderIndex < Count; ++ folderIndex)
			{
				for (int fileIndex = 0; fileIndex < this[folderIndex].Count; ++ fileIndex)
				{
					this[folderIndex][fileIndex].isExecuted = false;
				}
			}
		}

		public void Save()
		{
			configuration.fitTestFolders = fitTestFolders;
			Configuration.Save(configuration);
		}

		public void Clear()
		{
			fitTestFolders.Clear();
		}

		public void Load()
		{
			Clear();
			foreach (FitTestFolder fitTestFolder in configuration.fitTestFolders)
			{
				InternalAdd(fitTestFolder);
			}
		}

		public FitTestFolder GetFolderByHashCode(int hashCode)
		{
			foreach (FitTestFolder fitTestFolder in fitTestFolders)
			{
				if (fitTestFolder.GetHashCode() == hashCode)
				{
					return fitTestFolder;
				}
			}
			return null;
		}

		public FitTestFile GetFileByHashCode(int hashCode)
		{
			foreach (FitTestFolder fitTestFolder in fitTestFolders)
			{
				for (int fileIndex = 0; fileIndex < fitTestFolder.Count; ++ fileIndex)
				{
					if (fitTestFolder[fileIndex].GetHashCode() == hashCode)
					{
						return fitTestFolder[fileIndex];
					}
				}
			}
			return null;
		}
	}
}
