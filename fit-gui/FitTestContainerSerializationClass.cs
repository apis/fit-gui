using System;
using System.Xml.Serialization;

namespace fit.gui
{
	[XmlRootAttribute("FitTestContainer")]
	public class FitTestContainerSerializationClass
	{
		public FitTestFolderSerializationClass[] FitTestFolders;
	}

	[XmlTypeAttribute("FitTestFolder")]
	public class FitTestFolderSerializationClass
	{
		public string Name;
		public string SpecificationPath;
		public string ResultPath;
		public string FixturePath;
	}
}
