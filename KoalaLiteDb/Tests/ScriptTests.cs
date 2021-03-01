using System.Collections.Generic;
using System.IO;
using KoalaLiteDb.Parser;
using NUnit.Framework;
using UltraLiteDB;

namespace KoalaLiteDb.Tests
{

	[TestFixture]
	public class ScriptTests
	{
		public class TellasConfig
		{
			public string Id { get; set; }
			public string FilePath { get;set; }
			public Dictionary<string, BsonValue> Tags { get; set;}
		}

		[Test]
		public void InterpreterTest()
		{
			string script = File.ReadAllText("../../../test.koala");

			UltraLiteDatabase database = new("test.db");
			KoalaInterpreter interpreter = new(database);
			interpreter.RunScript(script);

			Assert.IsTrue(database.CollectionExists("Configs"));
			UltraLiteCollection<BsonDocument> collection = database.GetCollection("Configs");
			Assert.IsTrue(collection.Count() == 1);

			BsonDocument document = collection.FindById("testConfig");
			Assert.AreEqual("hello world", document["filePath"].AsString);
			Assert.AreEqual(true, document["tags"]["test2"].AsBoolean);

			UltraLiteCollection<TellasConfig> mappedCollection = database.GetCollection<TellasConfig>("Configs");
			Assert.IsTrue(mappedCollection.Count() == 1);

			TellasConfig config = mappedCollection.FindById("testConfig");
			Assert.AreEqual("testConfig", config.Id);
			Assert.AreEqual("hello world", config.FilePath);
			Assert.AreEqual(true, config.Tags["test2"].AsBoolean);
		}
	}

}
