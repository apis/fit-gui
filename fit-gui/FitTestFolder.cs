using System;
using System.Collections;

namespace fit.gui
{
	[Serializable]
	public class FitTestFolder
	{
		private ArrayList fitTestFiles = new ArrayList();

		public string FolderName;
		public string InputFolder;
		public string OutputFolder;
		public string FixturePath;
		public string FileMask = "*.htm";

		public int Add(FitTestFile fitTestFile)
		{
			return fitTestFiles.Add(fitTestFile);
		}

		public int Count 
		{
			get
			{
				return fitTestFiles.Count;
			}
		}

		public FitTestFile this[int fileIndex]
		{
			get
			{
				return (FitTestFile)fitTestFiles[fileIndex];
			}

			set
			{
				fitTestFiles[fileIndex] = value;
			}
		}

		public override int GetHashCode()
		{
			return FolderName.GetHashCode();
		}
	}
}
