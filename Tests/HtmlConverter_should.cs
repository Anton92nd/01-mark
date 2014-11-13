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

		[Test]
		public void add_p_tags_around_paragraphs()
		{
			string text = "This\r\nis\r\nfirst paragraph\r\n   \r\nThis is\r\nsecond\r\n";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\r\n<body>\r\n<p>\r\nThis\r\nis\r\nfirst paragraph\r\n</p>\r\n<p>\r\nThis is\r\nsecond\r\n</p>\r\n</body>\r\n</html>", result);
		}

		[Test]
		public void find_em_tag()
		{
			string text = "_hello world_";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\r\n<body>\r\n<p>\r\n<em>hello world</em>\r\n</p>\r\n</body>\r\n</html>", result);
		}

		[Test]
		public void find_strong_tag()
		{
			string text = "__hello world__";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\r\n<body>\r\n<p>\r\n<strong>hello world</strong>\r\n</p>\r\n</body>\r\n</html>", result);
		}

		[Test]
		public void find_code_tag()
		{
			string text = "`hello _p_ world`";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\r\n<body>\r\n<p>\r\n<code>hello _p_ world</code>\r\n</p>\r\n</body>\r\n</html>", result);
		}

		[Test]
		public void find_strong_tag_inside_em_tag()
		{
			string text = "_hello __world__!_";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\r\n<body>\r\n<p>\r\n<em>hello <strong>world</strong>!</em>\r\n</p>\r\n</body>\r\n</html>", result);
		}

		[Test]
		public void not_convert_escaped_underscore()
		{
			string text = "\\_hello world\\_";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\r\n<body>\r\n<p>\r\n_hello world_\r\n</p>\r\n</body>\r\n</html>", result);
		}
	}
}
