using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Mark.HTMLParser;

namespace Tests
{
	[TestFixture]
	class EscapeReader_should
	{
		private static readonly EscapeReader Reader = new EscapeReader();

		[Test]
		public void return_null_if_no_backslash()
		{
			var result = Reader.ReadToken("abc");
			Assert.IsNull(result);
		}

		[Test]
		public void read_one_escaped_character()
		{
			var result = Reader.ReadToken("\\qwe");
			Assert.True(new Token("\\q", "q", TokenType.Separator).Equals(result));
		}

		[Test]
		public void replace_less_with_lt()
		{
			var result = Reader.ReadToken("\\<we");
			Assert.True(new Token("\\<", "&lt;", TokenType.Separator).Equals(result));
		}

		[Test]
		public void replace_greater_with_gt()
		{
			var result = Reader.ReadToken("\\>we");
			Assert.True(new Token("\\>", "&gt;", TokenType.Separator).Equals(result));
		}
	}
}
