using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace fit.gui
{
	[Serializable]
	public class StoryTestContainer
	{
		private ArrayList fitTestFolders = new ArrayList();
		private const string FILE_NAME = "StoryTestContainer.sav";

		public int Add(FitTestFolder fitTestFolder)
		{
			string[] filePatterns = fitTestFolder.FileMask.Split(';');
			
			foreach (string filePattern in filePatterns)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(fitTestFolder.InputFolder);
				FileInfo[] files  = directoryInfo.GetFiles(filePattern);
				for (int index = 0; index < files.Length; ++ index)
				{
					StoryTestFile storyTestFile = new StoryTestFile();
					storyTestFile.FileName = files[index].Name;
					storyTestFile.ParentHashCode = fitTestFolder.GetHashCode();
					fitTestFolder.Add(storyTestFile);
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

		public static void Save(StoryTestContainer storyTestContainer)
		{
			string currentAssemblyLocation = Assembly.GetExecutingAssembly().Location;
			string currentDirectory = Path.GetDirectoryName(currentAssemblyLocation);
			string fileName = Path.Combine(currentDirectory, FILE_NAME);

			using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.Serialize(fileStream, storyTestContainer);
			}
		}

		public static void Load(ref StoryTestContainer storyTestContainer)
		{
			string currentAssemblyLocation = Assembly.GetExecutingAssembly().Location;
			string currentDirectory = Path.GetDirectoryName(currentAssemblyLocation);
			string fileName = Path.Combine(currentDirectory, FILE_NAME);

			if (new FileInfo(fileName).Exists)
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
				{
					BinaryFormatter binaryFormatter = new BinaryFormatter();
					storyTestContainer = (StoryTestContainer) binaryFormatter.Deserialize(fileStream);
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

		public StoryTestFile GetFileByHashCode(int hashCode)
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
