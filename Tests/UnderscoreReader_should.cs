﻿using Mark.Readers;
using NUnit.Framework;
using Mark;

namespace Tests
{
	[TestFixture]
	class UnderscoreReader_should
	{
		private static readonly UnderscoreReader Reader = new UnderscoreReader();

		[Test]
		public void return_null_when_no_underscores()
		{
			var result = Reader.ReadToken("abc");
			Assert.IsNull(result);
		}

		[Test]
		public void find_two_underscores_if_possible()
		{
			var result = Reader.ReadToken("__abc");
			Assert.True(new Token("__", TokenType.DoubleUnderscore).Equals(result));
		}

		[Test]
		public void find_one_underscore_if_possible()
		{
			var result = Reader.ReadToken("_ abc");
			Assert.True(new Token("_", TokenType.Underscore).Equals(result));
		}

		[Test]
		public void find_no_more_than_two_underscores()
		{
			var result = Reader.ReadToken("___abc");
			Assert.True(new Token("___", TokenType.Unknown).Equals(result));
		}
	}
}
