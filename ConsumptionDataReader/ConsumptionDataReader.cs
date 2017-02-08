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

		public virtual int ProcessInputData(string InputData)
		{
			throw new NotImplementedException("ProcessInputFile");
		}
	}
}

