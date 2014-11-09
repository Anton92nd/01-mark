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
	class LineEndReader_should
	{
		private static readonly LineEndReader Reader = new LineEndReader();

		[Test]
		public void return_null_when_no_CR_or_LF()
		{
			var result = Reader.ReadToken("abc");
			Assert.IsNull(result);
		}

		[Test]
		public void find_CR_and_LF_if_both_present()
		{
			var result = Reader.ReadToken("\r\n hello");
			Assert.True(new Token("\r\n", TokenType.LineEnd).Equals(result));
		}

		[Test]
		public void find_CR()
		{
			var result = Reader.ReadToken("\r hello");
			Assert.True(new Token("\r", TokenType.LineEnd).Equals(result));
		}

		[Test]
		public void find_LF()
		{
			var result = Reader.ReadToken("\n\r hello");
			Assert.True(new Token("\n", TokenType.LineEnd).Equals(result));
		}
	}
}
