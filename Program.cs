using System;
using System.IO;

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
			dbAccess.OpenConnection("localhost", "playground", "testuser", "TestUser#");

			var wantedReader = ConsumptionDataReaderKind.VnStatXml;
			var reader = ConsumptionDataReader.CreateConsumptionDataReader(wantedReader);
			reader.ConsumptionDataDestination = dbAccess;

			// Example data created using this command:
			// vnstat -i ppp0 --xml > /tmp/vnstat_ppp0.20170209.xml
			var inputFile = "/tmp/vnstat_ppp0.20170209.xml"; // TO DO: retrieve from command line arguments
			var inputFileContent = File.ReadAllText(inputFile);
			reader.ProcessInputData(inputFileContent);
		}
	}
}
