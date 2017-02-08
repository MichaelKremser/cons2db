using System;

namespace cons2db
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var wantedDb = ConsumptionDataDbAccess.ConsumptionDbAccessKind.Npgsql;
			var dbAccess = ConsumptionDataDbAccess.CreateConsumptionDbAccess(wantedDb);
#if DEBUG
			dbAccess.Verbosity = 3;
#endif
			var inputFile = "/tmp/vnstat_ppp0.xml"; // TO DO: retrieve from command line arguments
			var wantedReader = ConsumptionDataReaderKind.VnStatXml;
			var reader = ConsumptionDataReader.CreateConsumptionDataReader(wantedReader);
			reader.ConsumptionDataDestination = dbAccess;
			dbAccess.OpenConnection("localhost", "playground", "testuser", "TestUser#");
		}
	}
}
