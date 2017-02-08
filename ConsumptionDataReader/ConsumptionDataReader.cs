using System;

namespace cons2db
{
	public class ConsumptionDataReader
	{
		public ConsumptionDataReader ()
		{
		}

		public ConsumptionDataDbAccess ConsumptionDataDestination {
			get;
			set;
		}

		public static ConsumptionDataReader CreateConsumptionDataReader(ConsumptionDataReaderKind kind)
		{
			switch (kind)
			{
			case ConsumptionDataReaderKind.VnStatXml:
				return new ConsumptionDataReaderVnStatXml();
			}
			return null;
		}

		public virtual int ProcessInputData(string InputData)
		{
			throw new NotImplementedException("ProcessInputFile");
		}
	}

	public enum ConsumptionDataReaderKind
	{
		VnStatXml
	}
}

