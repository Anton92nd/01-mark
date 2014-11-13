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
		public void find_code_separator()
		{
			var result = Reader.ReadToken("`code`");
			Assert.True(new Token("`", TokenType.Code).Equals(result));
		}
	}
}
