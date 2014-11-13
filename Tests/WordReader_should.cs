using Mark.Readers;
using NUnit.Framework;
using Mark;

namespace Tests
{
	[TestFixture]
	class WordReader_should
	{
		private static readonly WordReader Reader = new WordReader();


		[Test]
		public void return_null_on_empty_string()
		{
			var result = Reader.ReadToken("");
			Assert.IsNull(result);
		}

		[Test]
		public void read_consecutive_letters_digits_and_underscores()
		{
			var result = Reader.ReadToken("hell0_world!");
			Assert.True(new Token("hell0_world", TokenType.Word).Equals(result));
		}

		[Test]
		public void not_read_word_starting_with_underscore()
		{
			var result = Reader.ReadToken("_hell0_world!");
			Assert.IsNull(result);
		}

		[Test]
		public void read_word_ending_with_underscore()
		{
			var result = Reader.ReadToken("hell0_world__");
			Assert.True(new Token("hell0_world", TokenType.Word).Equals(result));
		}
	}
}
