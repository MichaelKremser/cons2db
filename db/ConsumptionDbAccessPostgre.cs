using System;
using System.Data;
using Npgsql;

namespace cons2db
{
	public class ConsumptionDbAccessPostgre : ConsumptionDbAccess
	{
		public ConsumptionDbAccessPostgre()
		{
		}

		private readonly string ConnectionStringPattern = "server={0};database={1};userid={2};password={3};";
		
		public override void OpenConnection(string Hostname, string Database, string Username, string Password)
		{
			if (Verbosity >= 2)
			{
				Console.WriteLine ("Connecting to Postgre SQL database...");
			}
			var connString = string.Format(ConnectionStringPattern, Hostname, Database, Username, Password);
			var conn = new NpgsqlConnection(connString);
			conn.Open();
			if (Verbosity >= 2)
			{
				Console.WriteLine ("Connected; server version is \"" + conn.ServerVersion + "\"");
			}
		}
	}
}