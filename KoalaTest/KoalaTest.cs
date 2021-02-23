using KoalaScript.Lang;
using KoalaScript.Parser;
using NUnit.Framework;
using Pidgin;

namespace KoalaTest
{

	[TestFixture]
	public class KoalaTest
	{
		[Test]
		public void SimpleTest()
		{
			string test = "set $hello to \"bam!\"";
			KoalaType koalaType = ParserUtils.VarType.ParseOrThrow(test);
			Assert.IsTrue(true);
		}
	}

}
