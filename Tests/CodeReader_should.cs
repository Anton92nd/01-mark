using System;
using NUnit.Framework;
using Mark.Readers;
using Mark;

namespace Tests
{
	[TestFixture]
	class CodeReader_should
	{
		private static readonly CodeReader Reader = new CodeReader();

		[Test]
		public void parse_string_with_one_backquote()
		{
			var result = Reader.ReadToken("`code");
			Assert.IsNull(result);
		}

		[Test]
		public void find_code_content()
		{
			var result = Reader.ReadToken("`_code_`abc");
			Assert.AreEqual(new Token("`_code_`", "_code_", TokenType.Code), result);
		}
	}
}
