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
	class WhitespaceReader_should
	{

		private static readonly WhitespaceReader Reader = new WhitespaceReader();

		[Test]
		public void return_null_when_no_whitespace_at_start()
		{
			var result = Reader.ReadToken("abc");
			Assert.IsNull(result);
		}

		[Test]
		public void find_all_starting_whitespaces()
		{
			var result = Reader.ReadToken("\t \tab\tc");
			Assert.True(new Token("\t \t", TokenType.Whitespace).Equals(result));
		}

		[Test]
		public void not_take_line_endings_as_whitespaces()
		{
			var result = Reader.ReadToken("\t\n");
			Assert.True(new Token("\t", TokenType.Whitespace).Equals(result));
		}
	}
}
