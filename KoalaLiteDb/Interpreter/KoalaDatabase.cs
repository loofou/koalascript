using System;
using System.IO;
using UltraLiteDB;

namespace KoalaLiteDb.Interpreter
{

	public class KoalaDatabase : IDisposable
	{
		readonly MemoryStream databaseMemoryStream;
		public UltraLiteDatabase Database { get; }

		public KoalaDatabase()
		{
			databaseMemoryStream = new MemoryStream();
			BsonMapper bsonMapper = new();
			Database = new UltraLiteDatabase(databaseMemoryStream, bsonMapper);
		}

		public void Dispose()
		{
			Database?.Dispose();
			databaseMemoryStream?.Dispose();

			GC.SuppressFinalize(this);
		}
	}

}
