using System;
using System.Data;
using Npgsql;

namespace cons2db
{
	public class ConsumptionDataDbAccessPostgre : ConsumptionDataDbAccess
	{
		public ConsumptionDataDbAccessPostgre()
		{
		}

		private readonly string ConnectionStringPattern = "server={0};database={1};userid={2};password={3};";
		private NpgsqlConnection conn;
		
		public override void OpenConnection(string Hostname, string Database, string Username, string Password)
		{
			if (Verbosity >= 2)
			{
				Console.WriteLine ("Connecting to Postgre SQL database...");
			}
			var connString = string.Format(ConnectionStringPattern, Hostname, Database, Username, Password);
			conn = new NpgsqlConnection(connString);
			conn.Open();
			if (Verbosity >= 2)
			{
				Console.WriteLine ("Connected; server version is \"" + conn.ServerVersion + "\"");
			}
		}
		
		public override long GetSystemId(string SystemName)
		{
			return 1;
		}

		public override long GetDeviceId(long SystemId, string DeviceName)
		{
			return 1;
		}

		public override int UpdateConsumptionData(long DeviceId, DateTime ConsumptionOccured, long Received, long Sent)
		{
			int ra;
			Console.WriteLine("UpdateConsumptionData: " + DeviceId + " " + ConsumptionOccured + " " + Received + " " + Sent);
			using (var insertCommand = conn.CreateCommand()) {
				insertCommand.CommandText = "insert into consumption_data " +
					"(cd_dev_id, cd_timestamp, cd_rx, cd_tx, cd_refreshed) values " +
					"(:cd_dev_id, :cd_timestamp, :cd_rx, :cd_tx, :cd_refreshed)";
				insertCommand.Parameters.Add("cd_dev_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = DeviceId;
				insertCommand.Parameters.Add("cd_timestamp", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = ConsumptionOccured;
				insertCommand.Parameters.Add("cd_rx", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Received;
				insertCommand.Parameters.Add("cd_tx", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Sent;
				insertCommand.Parameters.Add("cd_refreshed", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = DateTime.Now;
				insertCommand.Prepare ();
				ra = insertCommand.ExecuteNonQuery ();
			}
			return ra;
		}
	}
}