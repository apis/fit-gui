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
		private ArrayList storyTestFolders = new ArrayList();
		private const string FILE_NAME = "StoryTestContainer.sav";

		public int Add(StoryTestFolder storyTestFolder)
		{
			string[] filePatterns = storyTestFolder.FileMask.Split(';');
			
			foreach (string filePattern in filePatterns)
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(storyTestFolder.InputFolder);
				FileInfo[] files  = directoryInfo.GetFiles(filePattern);
				for (int index = 0; index < files.Length; ++ index)
				{
					StoryTestFile storyTestFile = new StoryTestFile();
					storyTestFile.FileName = files[index].Name;
					storyTestFile.ParentHashCode = storyTestFolder.GetHashCode();
					storyTestFolder.Add(storyTestFile);
				}
			}
			return storyTestFolders.Add(storyTestFolder);
		}

		public StoryTestFolder this[int folderIndex]
		{
			get
			{
				return (StoryTestFolder)storyTestFolders[folderIndex];
			}

			set
			{
				storyTestFolders[folderIndex] = value;
			}
		}

		public int Count 
		{
			get
			{
				return storyTestFolders.Count;
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

		public StoryTestFolder GetFolderByHashCode(int hashCode)
		{
			foreach (StoryTestFolder storyTestFolder in storyTestFolders)
			{
				if (storyTestFolder.GetHashCode() == hashCode)
				{
					return storyTestFolder;
				}
			}
			return null;
		}

		public StoryTestFile GetFileByHashCode(int hashCode)
		{
			foreach (StoryTestFolder storyTestFolder in storyTestFolders)
			{
				for (int fileIndex = 0; fileIndex < storyTestFolder.Count; ++ fileIndex)
				{
					if (storyTestFolder[fileIndex].GetHashCode() == hashCode)
					{
						return storyTestFolder[fileIndex];
					}
				}
			}
			return null;
		}
	}
}
