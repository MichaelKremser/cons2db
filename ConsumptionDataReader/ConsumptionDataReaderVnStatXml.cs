using System;
using System.Xml;

namespace cons2db
{
	public class ConsumptionDataReaderVnStatXml : ConsumptionDataReader
	{
		public ConsumptionDataReaderVnStatXml()
		{
		}

		public override int ProcessInputData(string InputData)
		{
			var xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(InputData);
			var interfaceNode = xmlDoc.SelectSingleNode("/vnstat/interface");
			var interfaceId = interfaceNode.GetNodeValueDefensive("id");
			var hourNodes = interfaceNode.SelectNodes("traffic/hours/hour");
			int dateYear, dateMonth, dateDay;
			foreach (XmlNode hourNode in hourNodes)
			{
				var hourId = hourNode.GetNodeIntValueDefensive("id");
				ParseDate(hourNode.SelectSingleNode("date"), out dateYear, out dateMonth, out dateDay);
				Console.WriteLine (Environment.MachineName + "." + interfaceId + " " + hourId + " " + dateYear + "-" + dateMonth + "-" + dateDay);
			}
			//Console.WriteLine("hourNodes has " + hourNodes.Count + " child nodes");
			return -1;
		}

		private void ParseDate(XmlNode DateNode, out int dateYear, out int dateMonth, out int dateDay)
		{
			var yearNode = DateNode.SelectSingleNode("year");
			var monthNode = DateNode.SelectSingleNode("month");
			var dayNode = DateNode.SelectSingleNode("day");
			dateYear = yearNode.GetNodeInnerTextAsInt();
			dateMonth = monthNode.GetNodeInnerTextAsInt();
			dateDay = dayNode.GetNodeInnerTextAsInt();
		}
	}
}

