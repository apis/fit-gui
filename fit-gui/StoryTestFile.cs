using System;

using fit.gui.common;

namespace fit.gui
{
	[Serializable]
	public class StoryTestFile
	{
		public string FileName;
		public int ParentHashCode;
		public TestRunProperties TestRunProperties;
		public bool isExecuted;

		public override int GetHashCode()
		{
			return ParentHashCode ^ FileName.GetHashCode();
		}
	}
}