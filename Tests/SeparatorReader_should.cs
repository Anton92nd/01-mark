using Mark.Readers;
using NUnit.Framework;
using Mark;

namespace Tests
{
	[TestFixture]
	class SeparatorReader_should
	{
		private static readonly SeparatorReader Reader = new SeparatorReader();

		[Test]
		public void return_null_if_empty_string()
		{
			var result = Reader.ReadToken("");
			Assert.IsNull(result);
		}

		[Test]
		public void read_single_separator()
		{
			var result = Reader.ReadToken(",.");
			Assert.True(new Token(",", TokenType.Separator).Equals(result));
		}

		[Test]
		public void replace_ampersand_with_amp()
		{
			var result = Reader.ReadToken("&.");
			Assert.True(new Token("&", "&amp;", TokenType.Separator).Equals(result));
		}

		[Test]
		public void replace_less_with_lt()
		{
			var result = Reader.ReadToken("<.");
			Assert.True(new Token("<", "&lt;", TokenType.Separator).Equals(result));
		}

		[Test]
		public void replace_greater_with_gt()
		{
			var result = Reader.ReadToken(">we");
			Assert.True(new Token(">", "&gt;", TokenType.Separator).Equals(result));
		}

		[Test]
		public void find_code_separator()
		{
			var result = Reader.ReadToken("`code`");
			Assert.True(new Token("`", TokenType.Code).Equals(result));
		}
	}
}
