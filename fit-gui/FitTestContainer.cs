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
		private ArrayList fitTestFolders = new ArrayList();
		private const string FILE_NAME = "fit-gui.sav";

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
			string currentAssemblyLocation = Assembly.GetExecutingAssembly().Location;
			string currentDirectory = Path.GetDirectoryName(currentAssemblyLocation);
			string fileName = Path.Combine(currentDirectory, FILE_NAME);

			using (TextWriter textWriter = new StreamWriter(fileName))
			{
				XmlSerializer xmlSerializer = 
					new XmlSerializer(typeof(FitTestContainerSerializationClass));
				FitTestContainerSerializationClass fitTestContainerSerializationClass =
					new FitTestContainerSerializationClass();

				fitTestContainerSerializationClass.FitTestFolders = 
					new FitTestFolderSerializationClass[Count];

				for (int folderIndex = 0; folderIndex < Count; ++ folderIndex)
				{
					FitTestFolderSerializationClass fitTestFolderSerializationClass =
						new FitTestFolderSerializationClass();
					fitTestFolderSerializationClass.Name = this[folderIndex].FolderName;
					fitTestFolderSerializationClass.SpecificationPath = this[folderIndex].InputFolder;
					fitTestFolderSerializationClass.ResultPath = this[folderIndex].OutputFolder;
					fitTestFolderSerializationClass.FixturePath = this[folderIndex].FixturePath;
					fitTestContainerSerializationClass.FitTestFolders[folderIndex] =
						fitTestFolderSerializationClass;
				}

				xmlSerializer.Serialize(textWriter, fitTestContainerSerializationClass);
			}
		}

		public void Clear()
		{
			fitTestFolders.Clear();
		}

		public void Load()
		{
			string currentAssemblyLocation = Assembly.GetExecutingAssembly().Location;
			string currentDirectory = Path.GetDirectoryName(currentAssemblyLocation);
			string fileName = Path.Combine(currentDirectory, FILE_NAME);

			Clear();
			if (!new FileInfo(fileName).Exists) return;

			using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(FitTestContainerSerializationClass));
				FitTestContainerSerializationClass fitTestContainerSerializationClass;
				fitTestContainerSerializationClass = 
					(FitTestContainerSerializationClass) xmlSerializer.Deserialize(fileStream);

				for (int folderIndex = 0; folderIndex < fitTestContainerSerializationClass.FitTestFolders.Length; ++ folderIndex)
				{
					FitTestFolder fitTestFolder = new FitTestFolder();
					fitTestFolder.FolderName = 
						fitTestContainerSerializationClass.FitTestFolders[folderIndex].Name;
					fitTestFolder.InputFolder = 
						fitTestContainerSerializationClass.FitTestFolders[folderIndex].SpecificationPath;
					fitTestFolder.OutputFolder = 
						fitTestContainerSerializationClass.FitTestFolders[folderIndex].ResultPath;
					fitTestFolder.FixturePath = 
						fitTestContainerSerializationClass.FitTestFolders[folderIndex].FixturePath;
					InternalAdd(fitTestFolder);
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
