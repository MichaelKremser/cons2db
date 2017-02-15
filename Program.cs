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
			int idx = 0;
			var wantedDb = ConsumptionDbAccessKind.Unknown;
			var wantedReader = ConsumptionDataReaderKind.Unknown;
			byte verbosity = 0;
			#if DEBUG
			verbosity = 3;
			#endif
			while (idx < args.Length)
			{
				if (!args[idx].StartsWith("-") || args[idx] == "-")
				{
					inputFile = args[idx];
				}
				else
				{
					if (args[idx].StartsWith("-v"))
					{
						verbosity = (byte)(args[idx].Length - 1); // don't care if there is something else than a "v"
						if (verbosity > 3)
						{
							verbosity = 3;
							Console.WriteLine("Verbosity can't be more than 3!");
						}
					}
					else
					{
						if (args[idx] == "--filetype")
						{
							idx++;
							var requestedFileType = args[idx];
							if (!Enum.TryParse<ConsumptionDataReaderKind>(requestedFileType, out wantedReader))
							{
								Console.WriteLine("Unknown filetype \"{0}\"", requestedFileType);
								return;
							}
						}
						else
						{
							if (args[idx] == "--dbtype")
							{
								idx++;
								var requestedDbType = args[idx];
								if (!Enum.TryParse<ConsumptionDbAccessKind>(requestedDbType, out wantedDb))
								{
									Console.WriteLine("Unknown database type \"{0}\"", requestedDbType);
									return;
								}
							}
							else
							{
								if (args[idx] == "--credentials")
								{
									idx++;
									if (args[idx].Contains(":") || args[idx].Contains("@") || args[idx].Contains("#"))
									{
										var columnPos = args[idx].IndexOf(':');
										if (columnPos < 1)
										{
											Console.WriteLine ("Specify a username!");
											return;
										}
										var atPos = args[idx].IndexOf('@', columnPos + 2);
										if (atPos < 1)
										{
											Console.WriteLine ("Specify a password!");
											return;
										}
										var hashPos = args[idx].IndexOf('#', atPos + 2);
										if (hashPos < 1)
										{
											Console.WriteLine ("Specify a database name and host name seperated by a '#'!");
											return;
										}
										// a:b@c#d
										// 0123456
										dbUsername = args[idx].Substring(0, columnPos);
										dbPassword = args[idx].Substring(columnPos + 1, (atPos - columnPos - 1));
										dbDatabase = args[idx].Substring(atPos + 1, (hashPos - atPos - 1));
										dbHost = args[idx].Substring(hashPos + 1);
									}
									else
									{

									}
								}
							}
						}
					}
				}
				idx++;
			}
//			var wantedDb = ConsumptionDbAccessKind.Npgsql;
			var dbAccess = ConsumptionDataDbAccess.CreateConsumptionDbAccess(wantedDb);
#if DEBUG
			dbAccess.Verbosity = verbosity;
#endif
//			dbAccess.OpenConnection("localhost", "playground", "testuser", "TestUser#");
			dbAccess.OpenConnection(dbHost, dbDatabase, dbUsername, dbPassword);

//			var wantedReader = ConsumptionDataReaderKind.VnStatXml;
			var reader = ConsumptionDataReader.CreateConsumptionDataReader(wantedReader);
			reader.ConsumptionDataDestination = dbAccess;

			// Example data created using this command:
			// vnstat -i ppp0 --xml > /tmp/vnstat_ppp0.20170209.xml
			//var inputFile = "/tmp/vnstat_ppp0.20170209.xml"; // TO DO: retrieve from command line arguments while "-" means use standard input
			inputFile = "/tmp/vnstat_ppp0.20170209.xml";
			var inputFileContent = File.ReadAllText(inputFile);
			reader.ProcessInputData(inputFileContent);
		}
	}
}
