using System;
using System.Xml;

namespace cons2db
{
	public static class Xml
	{
		public static string GetNodeValueDefensive(this XmlNode Node, string AttributeName, string DefaultValue = "")
		{
			var xmlAttribute = Node.Attributes[AttributeName];
			if (xmlAttribute == null)
				return DefaultValue;
			else
				return xmlAttribute.InnerText;
		}

		public static int GetNodeIntValueDefensive(this XmlNode Node, string AttributeName, int DefaultValue = -1)
		{
			var attributeValue = Node.GetNodeValueDefensive(AttributeName);
			if (attributeValue == string.Empty)
				return DefaultValue;
			else
			{
				int result;
				if (int.TryParse(attributeValue, out result))
				{
					return result;
				}
				else
				{
					return DefaultValue;
				}
			}
		}

		public static int GetNodeInnerTextAsInt(this XmlNode Node, int DefaultValue = -1)
		{
			if (Node == null)
				return DefaultValue;
			else
			{
				int intValue;
				if (!int.TryParse(Node.InnerText, out intValue))
				{
					intValue = -1;
				}
				return intValue;
			}
		}
	}
}