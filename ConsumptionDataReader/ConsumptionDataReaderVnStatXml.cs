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
			var systemId = ConsumptionDataDestination.GetSystemId(Environment.MachineName);
			var deviceId = ConsumptionDataDestination.GetDeviceId(systemId, interfaceId);
			foreach (XmlNode hourNode in hourNodes)
			{
				var hourId = hourNode.GetNodeIntValueDefensive("id");
				var occured = ParseDate(hourNode.SelectSingleNode("date"));
				occured = occured.AddHours(hourId);
				int rx = hourNode.SelectSingleNode("rx").GetNodeInnerTextAsInt();
				int tx = hourNode.SelectSingleNode("tx").GetNodeInnerTextAsInt();
				int ra = ConsumptionDataDestination.UpdateConsumptionData(deviceId, occured, rx, tx);
				Console.WriteLine (Environment.MachineName + "." + interfaceId + " " + occured.ToLongDateString() + " inserted (" + ra + ")");
			}
			//Console.WriteLine("hourNodes has " + hourNodes.Count + " child nodes");
			return -1;
		}

		private DateTime ParseDate(XmlNode DateNode)
		{
			var yearNode = DateNode.SelectSingleNode("year");
			var monthNode = DateNode.SelectSingleNode("month");
			var dayNode = DateNode.SelectSingleNode("day");
			int dateYear = yearNode.GetNodeInnerTextAsInt();
			int dateMonth = monthNode.GetNodeInnerTextAsInt();
			int dateDay = dayNode.GetNodeInnerTextAsInt();
			return new DateTime(dateYear, dateMonth, dateDay);
		}
	}
}

