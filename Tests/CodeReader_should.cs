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
		public void find_code_content()
		{
			var result = Reader.ReadToken("`_code_`");
			Assert.AreEqual(new Token("`_code_`", "_code_", TokenType.Code), result);
		}
	}
}
