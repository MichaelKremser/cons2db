using System;
using System.Data;

namespace cons2db
{
	public class ConsumptionDataDbAccess
	{
		public ConsumptionDataDbAccess ()
		{
		}

		/// <summary>
		/// Gets or sets the verbosity, which is a value between 0 and 3.
		/// </summary>
		/// <description>
		/// 0 = be quiet
		/// 1 = tell important messages only
		/// 2 = give feedback about almost every action
		/// 3 = give detailed feedback about every action
		/// </description>
		/// <value>The verbosity.</value>
		public byte Verbosity {
			get;
			set;
		}

		public static ConsumptionDataDbAccess CreateConsumptionDbAccess(ConsumptionDbAccessKind kind)
		{
			switch (kind)
			{
			case ConsumptionDbAccessKind.Npgsql:
				return new ConsumptionDataDbAccessPostgre();
			}
			return null;
		}
		
		public virtual void OpenConnection(string Hostname, string Database, string Username, string Password)
		{
			throw new NotImplementedException("OpenConnection");
		}

		public virtual long GetSystemId(string SystemName)
		{
			throw new NotImplementedException("GetSystemId");
		}

		public virtual long GetDeviceId(long SystemId, string DeviceName)
		{
			throw new NotImplementedException("GetDeviceId");
		}

		public virtual int UpdateConsumptionData(long DeviceId, DateTime ConsumptionOccured, long Received, long Sent)
		{
			throw new NotImplementedException("UpdateConsumptionData");
		}
	}
	
	public enum ConsumptionDbAccessKind
	{
		Unknown,
		Npgsql
	}
}