using System;
using System.Data;

namespace cons2db
{
	public class ConsumptionDbAccess
	{
		public ConsumptionDbAccess ()
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

		public static ConsumptionDbAccess CreateConsumptionDbAccess(ConsumptionDbAccessKind kind)
		{
			switch (kind)
			{
			case ConsumptionDbAccessKind.Npgsql:
				return new ConsumptionDbAccessPostgre();
			}
			return null;
		}
		
		public virtual void OpenConnection(string Hostname, string Database, string Username, string Password)
		{
			throw new NotImplementedException("OpenConnection");
		}

		public enum ConsumptionDbAccessKind
		{
			Npgsql
		}
	}
}