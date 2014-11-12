using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Mark.HTMLParser;

namespace Tests
{
	[TestFixture]
	class HTMLParser_should
	{
		private static readonly Parser parser = new Parser();

		[Test]
		public void return_empty_list_on_empty_string()
		{
			var result = parser.Parse("");
			Assert.IsEmpty(result);
		}

		[Test]
		public void parse_all_possible_tokens()
		{
			var result = parser.Parse("hello& wo_rld _\\<__\r\n`");
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
				new Token("`", TokenType.Code)
			}, result);
		}

		[Test]
		public void parse_unknown_symbols_as_separators()
		{
			var result = parser.Parse("{}");
			Assert.AreEqual(new Token[]
			{
				new Token("{", TokenType.Separator),
				new Token("}", TokenType.Separator),  
			}, result);
		}
	}
}
