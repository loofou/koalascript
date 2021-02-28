using KoalaLiteDb.Parser;
using NUnit.Framework;
using Pidgin;

namespace KoalaLiteDb.Tests
{

	[TestFixture]
	public class ParserHelperTests
	{
		[TestCase("hello", "hello")]
		[TestCase("hello    ", "hello")]
		[TestCase("hello  \t\t", "hello")]
		[TestCase("hello\n\r", "hello")]
		[TestCase("hello\t\n\r     ", "hello")]
		public void TokTest(string script, string expected)
		{
			Assert.AreEqual(expected, ParserHelper.Tok(expected).ParseOrThrow(script));
		}

		[TestCase("h", 'h')]
		[TestCase("h    ", 'h')]
		[TestCase("h  \t\t", 'h')]
		[TestCase("h\n\r", 'h')]
		[TestCase("h\t\n\r     ", 'h')]
		public void TokTest(string script, char expected)
		{
			Assert.AreEqual(expected, ParserHelper.Tok(expected).ParseOrThrow(script));
		}

		[TestCase("hello.", "hello")]
		[TestCase("hello. # comment", "hello")]
		[TestCase("#comment\nhello .", "hello")]
		[TestCase("#comment\nhello .#comment2", "hello")]
		public void LineTest(string script, string expected)
		{
			Assert.AreEqual(expected, ParserHelper.Line(ParserHelper.Tok(expected)).ParseOrThrow(script));
		}

		[TestCase("hello")]
		[TestCase("#comment\nhello")]
		[TestCase("#comment")]
		[TestCase("hello#comment")]
		[TestCase("hello # comment\n .")]
		public void LineFail(string script)
		{
			Assert.Catch<ParseException>(() => ParserHelper.Line(ParserHelper.Tok("hello")).ParseOrThrow(script));
		}
	}

}
