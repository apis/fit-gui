using System;
using System.Collections;

namespace fit.gui
{
	[Serializable]
	public class FitTestFolder
	{
		private ArrayList storyTestFiles = new ArrayList();

		public string FolderName;
		public string InputFolder;
		public string OutputFolder;
		public string FixturePath;
		public string FileMask = "*.htm";

		public int Add(StoryTestFile storyTestFile)
		{
			return storyTestFiles.Add(storyTestFile);
		}

		public int Count 
		{
			get
			{
				return storyTestFiles.Count;
			}
		}

		public StoryTestFile this[int index]
		{
			get
			{
				return (StoryTestFile)storyTestFiles[index];
			}

			set
			{
				storyTestFiles[index] = value;
			}
		}

		public override int GetHashCode()
		{
			return FolderName.GetHashCode();
		}
	}
}
