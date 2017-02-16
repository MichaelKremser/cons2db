using System;
using System.Diagnostics;
using System.IO;

namespace cons2db
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			if (args.Length < 1)
			{
				Console.WriteLine("Calling syntax\n{0} [-v[v[v]]] -|file --filetype filetype --dbtype dbtype --credentials username:password@database#host|file", Process.GetCurrentProcess().ProcessName);
				Console.WriteLine("-v is for setting verbosity (not supplied = be quiet, -vvv = give detailed feedback about every action)");
				Console.WriteLine("filetype: VnStatXml");
				Console.WriteLine("dbtype: Npgsql");
				return;
			}
			string inputFile = "", dbUsername = "", dbPassword = "", dbDatabase = "", dbHost = "";
			var wantedDb = ConsumptionDbAccessKind.Unknown;
			var wantedReader = ConsumptionDataReaderKind.Unknown;
			byte verbosity = 0;
			#if DEBUG
			verbosity = 3;
			#endif
			ParseCommandLineArguments (args, ref inputFile, ref dbUsername, ref dbPassword, ref dbDatabase, ref dbHost, ref wantedDb, ref wantedReader, ref verbosity);
//			var wantedDb = ConsumptionDbAccessKind.Npgsql;
			var dbAccess = ConsumptionDataDbAccess.CreateConsumptionDbAccess(wantedDb);
			dbAccess.Verbosity = verbosity;
//			dbAccess.OpenConnection("localhost", "playground", "testuser", "TestUser#");
			dbAccess.OpenConnection(dbHost, dbDatabase, dbUsername, dbPassword);

//			var wantedReader = ConsumptionDataReaderKind.VnStatXml;
			var reader = ConsumptionDataReader.CreateConsumptionDataReader(wantedReader);
			reader.ConsumptionDataDestination = dbAccess;

			// Example data created using this command:
			// vnstat -i ppp0 --xml > /tmp/vnstat_ppp0.20170209.xml
			//var inputFile = "/tmp/vnstat_ppp0.20170209.xml"; // TO DO: retrieve from command line arguments while "-" means use standard input
			//inputFile = "/tmp/vnstat_ppp0.20170209.xml";
			var inputFileContent = File.ReadAllText(inputFile);
			reader.ProcessInputData(inputFileContent);
		}

		static bool ParseCommandLineArguments(string[] args, ref string inputFile, ref string dbUsername, ref string dbPassword, ref string dbDatabase, ref string dbHost, ref ConsumptionDbAccessKind wantedDb, ref ConsumptionDataReaderKind wantedReader, ref byte verbosity)
		{
			int idx = 0;
			while (idx < args.Length) {
				if (!args [idx].StartsWith ("-") || args [idx] == "-") {
					inputFile = args [idx];
				}
				else {
					if (args [idx].StartsWith ("-v")) {
						verbosity = (byte)(args [idx].Length - 1);
						// don't care if there is something else than a "v"
						if (verbosity > 3) {
							verbosity = 3;
							Console.WriteLine ("Verbosity can't be more than 3!");
						}
					}
					else {
						if (args [idx] == "--filetype") {
							idx++;
							var requestedFileType = args [idx];
							if (!Enum.TryParse<ConsumptionDataReaderKind> (requestedFileType, out wantedReader)) {
								Console.WriteLine ("Unknown filetype \"{0}\"", requestedFileType);
								return false;
							}
						}
						else {
							if (args [idx] == "--dbtype") {
								idx++;
								var requestedDbType = args [idx];
								if (!Enum.TryParse<ConsumptionDbAccessKind> (requestedDbType, out wantedDb)) {
									Console.WriteLine ("Unknown database type \"{0}\"", requestedDbType);
									return false;
								}
							}
							else {
								if (args [idx] == "--credentials") {
									idx++;
									var thisArg = args[idx];
									if (args[idx].Contains (":") && args[idx].Contains ("@") && args[idx].Contains ("#")) {
										if (!ParseCredentials(thisArg, ref dbUsername, ref dbPassword, ref dbDatabase, ref dbHost))
										{
											return false;
										}
									}
									else
									{
										if (File.Exists(thisArg))
										{
											var allLines = File.ReadAllLines(thisArg);
											if (allLines.Length < 1)
											{
												Console.WriteLine("File {0} does not contain a line with credentials.", thisArg);
												return false;
											}
											bool credentialsParsed = false;
											int lineNumber = 0;
											while (!credentialsParsed)
											{
												if (!allLines[lineNumber].StartsWith("#"))
												{
													if (!ParseCredentials(allLines[lineNumber], ref dbUsername, ref dbPassword, ref dbDatabase, ref dbHost))
													{
														return false;
													}
													credentialsParsed = true;
												}
												lineNumber++;
												if (lineNumber >= allLines.Length)
												{
													Console.WriteLine("File {0} does not contain a line with credentials.", thisArg);
													return false;
												}
											}
										}
										else
										{
											Console.WriteLine("Specify credentials in format username:password@database#host or supply a readable file.");
										}
									}
								}
							}
						}
					}
				}
				idx++;
			}
			return true;
		}

		static bool ParseCredentials(string thisArg, ref string dbUsername, ref string dbPassword, ref string dbDatabase, ref string dbHost)
		{
			var columnPos = thisArg.IndexOf (':');
			if (columnPos < 1) {
				Console.WriteLine ("Specify a username!");
				return false;
			}
			var atPos = thisArg.IndexOf ('@', columnPos + 2);
			if (atPos < 1) {
				Console.WriteLine ("Specify a password!");
				return false;
			}
			var hashPos = thisArg.IndexOf ('#', atPos + 2);
			if (hashPos < 1 || hashPos > thisArg.Length - 2) {
				Console.WriteLine ("Specify a database name and host name seperated by a '#'!");
				return false;
			}
			// a:b@c#d
			// 0123456
			dbUsername = thisArg.Substring (0, columnPos);
			dbPassword = thisArg.Substring (columnPos + 1, (atPos - columnPos - 1));
			dbDatabase = thisArg.Substring (atPos + 1, (hashPos - atPos - 1));
			dbHost = thisArg.Substring (hashPos + 1);
			return true;
		}
	}
}
