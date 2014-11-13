using System.Linq;
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
		public void not_lose_any_symbols()
		{
			var text = "hello& wo_rld _\\<__\r\n`";
			var result = Parser.Parse(text).Select(token => token.Source).Aggregate("", (str, source) => str + source);
			Assert.AreEqual(text, result);
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
