using System;
using System.IO;
using NUnit.Framework;
using Mark;

namespace Tests
{
	[TestFixture]
	class HtmlConverter_should
	{
		private const string TestsDirectory = "../../tests/";

		[Test]
		public void throw_exception_if_no_such_file()
		{
			Assert.Throws<FileNotFoundException>(() => HtmlConverter.ConvertFile("no_such_file.txt"));
		}

		[Test]
		public void create_file_with_html_extension()
		{
			HtmlConverter.ConvertFile(TestsDirectory + "sample.txt");
			Assert.True(File.Exists(TestsDirectory + "sample.html"));
		}

		[Test]
		public void convert_empty_file_into_file_with_html_tag()
		{
			var lineEndings = new[] {"\r\n", "\r", "\n"};
			HtmlConverter.ConvertFile(TestsDirectory + "empty.txt");
			var result = new StreamReader(TestsDirectory + "empty.html").ReadToEnd().Split(lineEndings, StringSplitOptions.RemoveEmptyEntries);
			var expected = new StreamReader(TestsDirectory + "emptyResult.txt").ReadToEnd().Split(lineEndings, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual(expected, result);
		}

		private const string Beginning = "<html><head><meta charset=\"UTF-8\"></head>\r\n<body>\r\n",
			Ending = "</body>\r\n</html>";


		[Test]
		public void add_p_tags_around_paragraphs()
		{
			var text = "This\r\nis\r\nfirst paragraph\r\n   \r\nThis is\r\nsecond\r\n";
			var result = HtmlConverter.ConvertString(text);
			Assert.AreEqual(Beginning + "<p>\r\nThis\r\nis\r\nfirst paragraph\r\n</p>\r\n<p>\r\nThis is\r\nsecond\r\n</p>\r\n" + Ending, result);
		}

		[Test]
		public void find_em_tag()
		{
			var text = "_hello world_";
			var result = HtmlConverter.ConvertString(text);
			Assert.AreEqual(Beginning + "<p>\r\n<em>hello world</em>\r\n</p>\r\n" + Ending, result);
		}

		[Test]
		public void find_strong_tag()
		{
			var text = "__hello world__";
			var result = HtmlConverter.ConvertString(text);
			Assert.AreEqual(Beginning + "<p>\r\n<strong>hello world</strong>\r\n</p>\r\n" + Ending, result);
		}

		[Test]
		public void find_code_tag()
		{
			var text = "`hello _p_ world`";
			var result = HtmlConverter.ConvertString(text);
			Assert.AreEqual(Beginning + "<p>\r\n<code>hello _p_ world</code>\r\n</p>\r\n" + Ending, result);
		}

		[Test]
		public void find_strong_tag_inside_em_tag()
		{
			var text = "_hello __world__!_";
			var result = HtmlConverter.ConvertString(text);
			Assert.AreEqual(Beginning + "<p>\r\n<em>hello <strong>world</strong>!</em>\r\n</p>\r\n" + Ending, result);
		}

		[Test]
		public void not_convert_escaped_underscore()
		{
			var text = "\\_hello world\\_";
			var result = HtmlConverter.ConvertString(text);
			Assert.AreEqual(Beginning + "<p>\r\n_hello world_\r\n</p>\r\n" + Ending, result);
		}

		[Test]
		public void ignore_odd_tags()
		{
			string text = "_test__`_ _";
			var result = HtmlConverter.ConvertString(text);
			Assert.AreEqual(Beginning + "<p>\r\n<em>test__`</em> _\r\n</p>\r\n" + Ending, result);
		}
	}
}
