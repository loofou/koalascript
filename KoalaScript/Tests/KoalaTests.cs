using System;
using System.IO;
using KoalaScript.Lang;
using KoalaScript.Parser;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Text;

namespace KoalaTest
{

	[TestFixture]
	public class Tests
	{
		[SetUp]
		public void Setup()
		{
			JsConfig.IncludePublicFields = true;
		}

		[Serializable]
		public struct KoalaConfigTest : IKoalaConfig
		{
			public string testVar;
			public string testString;
			public bool testBool;
			public int testNumber;

			public KoalaConfigTest(string testVar, string testString, bool testBool, int testNumber)
			{
				this.testVar = testVar;
				this.testString = testString;
				this.testBool = testBool;
				this.testNumber = testNumber;
			}
		}

		[Test]
		public void TestKoalaScriptFile()
		{
			string script = File.ReadAllText("../../../../KoalaScript/test.koala");

			KoalaConfigTest koalaConfigTest = new KoalaConfigTest("bar", "peter", false, 5);
			koalaConfigTest = KoalaInterpreter.LoadConfig(script, koalaConfigTest);

			Console.WriteLine(koalaConfigTest.Dump());
			Assert.Pass();
		}

		[Test]
		public void TestConfig()
		{
			string script = @"# test script
							set testString to ""hello world"".
							set testVar to foo. #does not actually exist
							set testBool to yes.
							set testNumber to -7.";

			KoalaConfigTest preConfig = new KoalaConfigTest("bar", "peter", false, 5);
			KoalaConfigTest koalaConfigTest = KoalaInterpreter.LoadConfig(script, preConfig);

			Assert.AreNotEqual(preConfig, koalaConfigTest);
			Assert.AreEqual("hello world", koalaConfigTest.testString);
			Assert.AreEqual(true, koalaConfigTest.testBool);
			Assert.AreEqual("foo", koalaConfigTest.testVar);
			Assert.AreEqual(-7, koalaConfigTest.testNumber);
		}
	}

}
