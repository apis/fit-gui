using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace fit.gui
{
	[Serializable]
	public class FitTestContainer
	{
		private ArrayList fitTestFolders = new ArrayList();
		private const string FILE_NAME = "fit-gui.sav";

		public int Add(FitTestFolder fitTestFolder)
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

		public static void Save(FitTestContainer fitTestContainer)
		{
			string currentAssemblyLocation = Assembly.GetExecutingAssembly().Location;
			string currentDirectory = Path.GetDirectoryName(currentAssemblyLocation);
			string fileName = Path.Combine(currentDirectory, FILE_NAME);

			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fileStream, fitTestContainer);
			}
		}

		public static void Load(ref FitTestContainer fitTestContainer)
		{
			string currentAssemblyLocation = Assembly.GetExecutingAssembly().Location;
			string currentDirectory = Path.GetDirectoryName(currentAssemblyLocation);
			string fileName = Path.Combine(currentDirectory, FILE_NAME);

			if (new FileInfo(fileName).Exists)
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					fitTestContainer = (FitTestContainer) binaryFormatter.Deserialize(fileStream);
				}
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
