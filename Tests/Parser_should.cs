using NUnit.Framework;
using Mark;

namespace Tests
{
	[TestFixture]
	class HTMLParser_should
	{
		[Test]
		public void return_empty_list_on_empty_string()
		{
			var result = Parser.Parse("");
			Assert.IsEmpty(result);
		}

		[Test]
		public void parse_all_possible_tokens()
		{
			var result = Parser.Parse("hello& wo_rld _\\<__\r\n`");
			Assert.AreEqual(new Token[]
			{
				new Token("hello", TokenType.Word), 
				new Token("&", "&amp;", TokenType.Separator), 
				new Token(" ", TokenType.Whitespace),
				new Token("wo_rld", TokenType.Word),
				new Token(" ", TokenType.Whitespace),
				new Token("_", TokenType.Underscore),
				new Token("\\<", "&lt;", TokenType.Separator), 
				new Token("__", TokenType.DoubleUnderscore),
 				new Token("\r\n", TokenType.LineEnd), 
				new Token("`", TokenType.Unknown)
			}, result);
		}

		[Test]
		public void parse_unknown_symbols()
		{
			var result = Parser.Parse("{}");
			Assert.AreEqual(new Token[]
			{
				new Token("{", TokenType.Unknown),
				new Token("}", TokenType.Unknown),  
			}, result);
		}
	}
}
