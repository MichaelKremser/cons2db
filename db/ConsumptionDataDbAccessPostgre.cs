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
			var sql = "WITH new_values (cd_dev_id, cd_timestamp, cd_rx, cd_tx) as ( " + 
				"values (:cd_dev_id, :cd_timestamp, :cd_rx, :cd_tx)), " + 
					"upsert as ( " + 
					"update consumption_data m " + 
					"set cd_rx = nv.cd_rx, " + 
					"cd_tx = nv.cd_tx, " + 
					"cd_refreshed = clock_timestamp() " + 
					"FROM new_values nv " + 
					"WHERE 1=1 " + 
					"and m.cd_dev_id = nv.cd_dev_id " + 
					"and m.cd_timestamp = nv.cd_timestamp " + 
					" RETURNING m.* ) " + 
					"INSERT INTO consumption_data (cd_dev_id, cd_timestamp, cd_rx, cd_tx, cd_refreshed) " + 
					"SELECT cd_dev_id, cd_timestamp, cd_rx, cd_tx, clock_timestamp() " + 
					"FROM new_values " + 
					"WHERE NOT EXISTS (  " + 
					"SELECT 1 " + 
					"FROM upsert up " + 
					"WHERE 1=1 " + 
					"and up.cd_dev_id = new_values.cd_dev_id " + 
					"and up.cd_timestamp = new_values.cd_timestamp " + 
					")";
			using (var insertCommand = conn.CreateCommand()) {
				insertCommand.CommandText = sql;
				insertCommand.Parameters.Add("cd_dev_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = DeviceId;
				insertCommand.Parameters.Add("cd_timestamp", NpgsqlTypes.NpgsqlDbType.Timestamp).Value = ConsumptionOccured;
				insertCommand.Parameters.Add("cd_rx", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Received;
				insertCommand.Parameters.Add("cd_tx", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Sent;
				insertCommand.Prepare ();
				ra = insertCommand.ExecuteNonQuery ();
			}
			return ra;
		}
	}
}