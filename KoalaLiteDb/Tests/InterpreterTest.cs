using System;
using System.IO;
using KoalaLiteDb.Tests.Data;
using NUnit.Framework;

namespace KoalaLiteDb.Tests
{

	[TestFixture]
	public class InterpreterTest
	{
		[SetUp]
		public void Setup()
		{
			DatabaseBuilder builder = new();
			Config config = new("testConfig");
			config.Settings.Add("test", 4);

			builder.Append(new[]
				{
					config
				});

			byte[] data = builder.Build();
			File.WriteAllBytes("Test.dat", data);
		}

		[Test]
		public void Test()
		{
			byte[] data = File.ReadAllBytes("Test.dat");

			MemoryDatabase db = new(data);
			Console.WriteLine(db.ConfigTable.FindByConfigId("testConfig").Settings["test"]);
		}
	}

}
