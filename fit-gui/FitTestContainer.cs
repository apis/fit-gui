using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

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

			using (TextWriter textWriter = new StreamWriter(fileName))
			{
				XmlSerializer xmlSerializer = 
					new XmlSerializer(typeof(FitTestContainerSerializationClass));
				FitTestContainerSerializationClass fitTestContainerSerializationClass =
					new FitTestContainerSerializationClass();

				fitTestContainerSerializationClass.FitTestFolders = 
					new FitTestFolderSerializationClass[fitTestContainer.Count];

				for (int folderIndex = 0; folderIndex < fitTestContainer.Count; ++ folderIndex)
				{
					FitTestFolderSerializationClass fitTestFolderSerializationClass =
						new FitTestFolderSerializationClass();
					fitTestFolderSerializationClass.Name = fitTestContainer[folderIndex].FolderName;
					fitTestFolderSerializationClass.SpecificationPath = fitTestContainer[folderIndex].InputFolder;
					fitTestFolderSerializationClass.ResultPath = fitTestContainer[folderIndex].OutputFolder;
					fitTestFolderSerializationClass.FixturePath = fitTestContainer[folderIndex].FixturePath;
					fitTestContainerSerializationClass.FitTestFolders[folderIndex] =
						fitTestFolderSerializationClass;
				}

				xmlSerializer.Serialize(textWriter, fitTestContainerSerializationClass);
			}
		}

		public static void Load(ref FitTestContainer fitTestContainer)
		{
			string currentAssemblyLocation = Assembly.GetExecutingAssembly().Location;
			string currentDirectory = Path.GetDirectoryName(currentAssemblyLocation);
			string fileName = Path.Combine(currentDirectory, FILE_NAME);

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
					fitTestContainer.Add(fitTestFolder);
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
