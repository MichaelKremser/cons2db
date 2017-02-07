using System;

namespace cons2db
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			var dbAccess = ConsumptionDbAccess.CreateConsumptionDbAccess(ConsumptionDbAccess.ConsumptionDbAccessKind.Npgsql);
#if DEBUG
			dbAccess.Verbosity = 3;
#endif
			dbAccess.OpenConnection("localhost", "playground", "testuser", "TestUser#");
		}
	}
}
