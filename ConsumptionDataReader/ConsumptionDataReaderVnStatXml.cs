using System;
using System.Xml;

namespace cons2db
{
	public class ConsumptionDataReaderVnStatXml : ConsumptionDataReader
	{
		public ConsumptionDataReaderVnStatXml ()
		{
		}

		public override int ProcessInputData (string InputData)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(InputData);
			Console.WriteLine("XML has " + xmlDoc.ChildNodes.Count + " child nodes");
			return -1;
		}
	}
}

