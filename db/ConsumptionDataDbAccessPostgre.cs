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
			if (Verbosity >= 1)
			{
				Console.WriteLine ("Connected; server version is \"" + conn.ServerVersion + "\"");
			}
		}
		
		public override long GetSystemId(string SystemName)
		{
			int attempts = 2;
			long result = -1;
			while (attempts > 0)
			{
				if (Verbosity >= 2)
				{
					Console.WriteLine ("GetSystemId: " + SystemName);
				}
				var sqlGetSystemId = "select sys_id from systems where sys_name = :sys_name";
				using (var cmdGetSystemId = conn.CreateCommand())
				{
					cmdGetSystemId.CommandText = sqlGetSystemId;
					cmdGetSystemId.Parameters.Add("sys_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = SystemName;
					object oResult = cmdGetSystemId.ExecuteScalar();
					if (oResult != null)
					{
						if (long.TryParse(oResult.ToString(), out result))
						{
							attempts = 0; // we have got a result, so we're finished
						}
						else
						{
							Console.WriteLine("  Warning: " + oResult + " can't be converted to long!");
						}
					}
					else
					{
						Console.WriteLine("  Not found!");
					}
				}
			}
			return result;
		}

		public override long GetDeviceId(long SystemId, string DeviceName)
		{
			int attempts = 2;
			long result = -1;
			while (attempts > 0)
			{
				if (Verbosity >= 2)
				{
					Console.WriteLine ("GetDeviceId: " + DeviceName);
				}
				var sqlGetDeviceId = "select dev_id from devices where dev_name = :dev_name and dev_sys_id = :sys_id";
				using (var cmdGetDeviceId = conn.CreateCommand())
				{
					cmdGetDeviceId.CommandText = sqlGetDeviceId;
					cmdGetDeviceId.Parameters.Add("dev_name", NpgsqlTypes.NpgsqlDbType.Varchar).Value = DeviceName;
					cmdGetDeviceId.Parameters.Add("sys_id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = SystemId;
					object oResult = cmdGetDeviceId.ExecuteScalar();
					if (oResult != null)
					{
						if (long.TryParse(oResult.ToString(), out result))
						{
							attempts = 0; // we have got a result, so we're finished
						}
						else
						{
							Console.WriteLine("  Warning: " + oResult + " can't be converted to long!");
						}
					}
					else
					{
						Console.WriteLine("  Not found!");
					}
				}
			}
			return result;
		}

		public override int UpdateConsumptionData(long DeviceId, DateTime ConsumptionOccured, long Received, long Sent)
		{
			int ra;
			if (Verbosity >= 1)
			{
				Console.WriteLine("UpdateConsumptionData: " + DeviceId + " " + ConsumptionOccured + " " + Received + " " + Sent);
			}
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
				insertCommand.Prepare();
				ra = insertCommand.ExecuteNonQuery();
			}
			return ra;
		}
	}
}