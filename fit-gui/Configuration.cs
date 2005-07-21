using System;
using System.Collections;
using System.Reflection;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace fit.gui
{
	public class Configuration
	{
		private const string CONFIGURATION_SCHEMA_FILE_NAME = "Configuration.xsd";
		private const string CONFIGURATION_XML_FILE_NAME = "fit-gui.sav";
		public ArrayList fitTestFolders = new ArrayList();

		private Configuration()
		{
		}

		private static string ExecutingPath
		{
			get
			{
				string executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
				return Path.GetDirectoryName(executingAssemblyLocation);
			}
		}

		public static Configuration Load()
		{
			string configurationXmlFileName = Path.Combine(ExecutingPath, CONFIGURATION_XML_FILE_NAME);
			string configurationSchemaFileName = Path.Combine(ExecutingPath, CONFIGURATION_SCHEMA_FILE_NAME);
			XmlTextReader xmlTextReader = null;
			XmlValidatingReader xmlValidatingReader = null;
			XmlSchemaCollection xmlSchemaCollection = null;
			try
			{
				Configuration configuration = new Configuration();
				if (!File.Exists(configurationXmlFileName))
				{
					return configuration;
				}
				xmlTextReader = new XmlTextReader(configurationXmlFileName);
				xmlValidatingReader = new XmlValidatingReader(xmlTextReader);
				xmlSchemaCollection = new XmlSchemaCollection();
				xmlSchemaCollection.Add("", configurationSchemaFileName);
				xmlValidatingReader.Schemas.Add(xmlSchemaCollection);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(xmlValidatingReader);
				configuration.fitTestFolders = GetFitTestFoldersFromXmlDocument(xmlDocument);
				return configuration;
			}
			finally
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
				}
				if (xmlValidatingReader != null)
				{
					xmlValidatingReader.Close();
				}
			}
		}

		public static void Save(Configuration configuration)
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlRootElement = xmlDocument.CreateElement("FitTestContainer");
			xmlDocument.AppendChild(xmlRootElement);

			XmlElement xmlFitTestFoldersElement = CreateElement(xmlRootElement, "FitTestFolders"); 
			foreach (FitTestFolder fitTestFolder in configuration.fitTestFolders)
			{
					XmlElement xmlFitTestFolderElement = CreateElement(xmlFitTestFoldersElement, "FitTestFolder"); 
					CreateElement(xmlFitTestFolderElement, "Name", fitTestFolder.FolderName); 
					CreateElement(xmlFitTestFolderElement, "SpecificationPath", fitTestFolder.InputFolder); 
					CreateElement(xmlFitTestFolderElement, "ResultPath", fitTestFolder.OutputFolder);
					CreateElement(xmlFitTestFolderElement, "FixturePath", fitTestFolder.FixturePath);
			}

			string configurationXmlFileName = Path.Combine(ExecutingPath, CONFIGURATION_XML_FILE_NAME);
			string configurationSchemaFileName = Path.Combine(ExecutingPath, CONFIGURATION_SCHEMA_FILE_NAME);
			SaveXmlDocumentToFile(xmlDocument, configurationXmlFileName, configurationSchemaFileName);
		}

		private static ArrayList GetFitTestFoldersFromXmlDocument(XmlDocument xmlDocument)
		{
			ArrayList fitTestFolders = new ArrayList();
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/FitTestContainer/FitTestFolders/*");
			foreach (XmlNode xmlNode in xmlNodeList)
			{
				FitTestFolder fitTestFolder = new FitTestFolder();
				fitTestFolder.FolderName = xmlNode.SelectSingleNode("Name").InnerText;
				fitTestFolder.InputFolder = xmlNode.SelectSingleNode("SpecificationPath").InnerText;
				fitTestFolder.OutputFolder = xmlNode.SelectSingleNode("ResultPath").InnerText;
				fitTestFolder.FixturePath = xmlNode.SelectSingleNode("FixturePath").InnerText;
				fitTestFolders.Add(fitTestFolder);
			}
			return fitTestFolders;
		}

		private static XmlElement CreateElement(XmlElement xmlRootElement, string element, string elementValue)
		{
			XmlElement xmlElement = xmlRootElement.OwnerDocument.CreateElement(element);
			xmlElement.InnerText = elementValue;
			xmlRootElement.AppendChild(xmlElement);
			return xmlElement;
		}

		private static XmlElement CreateElement(XmlElement xmlRootElement, string element)
		{
			XmlElement xmlElement = xmlRootElement.OwnerDocument.CreateElement(element);
			xmlRootElement.AppendChild(xmlElement);
			return xmlElement;
		}

		private static void SaveXmlDocumentToFile(XmlDocument xmlDocument, string xmlDocumentFileName, string xmlSchemaFileName)
		{
			StringReader stringReader = null;
			XmlTextReader xmlTextReader = null;
			XmlValidatingReader xmlValidatingReader = null;
			XmlSchemaCollection xmlSchemaCollection = null;
			try
			{
				stringReader = new StringReader(xmlDocument.InnerXml);
				xmlTextReader = new XmlTextReader(stringReader);
				xmlValidatingReader = new XmlValidatingReader(xmlTextReader);
				xmlSchemaCollection = new XmlSchemaCollection();
				xmlSchemaCollection.Add("", xmlSchemaFileName);
				xmlValidatingReader.Schemas.Add(xmlSchemaCollection);
				XmlDocument tempXmlDocument = new XmlDocument();
				tempXmlDocument.Load(xmlValidatingReader);
			}
			finally
			{
				if (xmlValidatingReader != null)
				{
					xmlValidatingReader.Close();
				}
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
				}
				if (stringReader != null)
				{
					stringReader.Close();
				}
			}

			using (StreamWriter streamWriter = new StreamWriter(xmlDocumentFileName, false, System.Text.Encoding.UTF8))
			{
				xmlDocument.Save(streamWriter);
			}
		}

		private static XmlDocument LoadXmlDocumentFromFile(string xmlDocumentFileName, string xmlSchemaFileName)
		{
			XmlTextReader xmlTextReader = null;
			XmlValidatingReader xmlValidatingReader = null;
			XmlSchemaCollection xmlSchemaCollection = null;
			try
			{
				xmlTextReader = new XmlTextReader(xmlDocumentFileName);
				xmlValidatingReader = new XmlValidatingReader(xmlTextReader);
				xmlSchemaCollection = new XmlSchemaCollection();
				xmlSchemaCollection.Add("", xmlSchemaFileName);
				xmlValidatingReader.Schemas.Add(xmlSchemaCollection);
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(xmlValidatingReader);
				return xmlDocument;
			}
			finally
			{
				if (xmlTextReader != null)
				{
					xmlTextReader.Close();
				}
				if (xmlValidatingReader != null)
				{
					xmlValidatingReader.Close();
				}
			}
		}
	}
}
