using System;
using System.IO;
using NUnit.Framework;
using Mark;

namespace Tests
{
	[TestFixture]
	class HtmlConverter_should
	{
		[Test]
		public void throw_exception_if_no_such_file()
		{
			Assert.Throws<FileNotFoundException>(() => HtmlConverter.ConvertFile("no_such_file.txt"));
		}

		[Test]
		public void create_file_with_html_extension()
		{
			HtmlConverter.ConvertFile("sample.txt");
			Assert.True(File.Exists("sample.html"));
		}

		[Test]
		public void convert_empty_file_into_file_with_html_tag()
		{
			var lineEndings = new[] {"\r\n", "\r", "\n"};
			HtmlConverter.ConvertFile("empty.txt");
			var result = new StreamReader("empty.html").ReadToEnd().Split(lineEndings, StringSplitOptions.RemoveEmptyEntries);
			var expected = new StreamReader("emptyResult.txt").ReadToEnd().Split(lineEndings, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void add_p_tags_around_paragraphs()
		{
			string text = "This\nis\nfirst paragraph\n   \nThis is\nsecond\n";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\n<body>\n<p>\nThis\nis\nfirst paragraph\n</p>\n<p>\nThis is\nsecond\n</p>\n</body>\n</html>", result);
		}

		[Test]
		public void find_em_tag()
		{
			string text = "_hello world_";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\n<body>\n<p>\n<em>hello world</em>\n</p>\n</body>\n</html>", result);
		}

		[Test]
		public void find_strong_tag()
		{
			string text = "__hello world__";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\n<body>\n<p>\n<strong>hello world</strong>\n</p>\n</body>\n</html>", result);
		}

		[Test]
		public void find_code_tag()
		{
			string text = "`hello _p_ world`";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\n<body>\n<p>\n<code>hello _p_ world</code>\n</p>\n</body>\n</html>", result);
		}

		[Test]
		public void find_strong_tag_inside_em_tag()
		{
			string text = "_hello __world__!_";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\n<body>\n<p>\n<em>hello <strong>world</strong>!</em>\n</p>\n</body>\n</html>", result);
		}

		[Test]
		public void not_convert_escaped_underscore()
		{
			string text = "\\_hello world\\_";
			string result = HtmlConverter.ConvertString(text);
			Assert.AreEqual("<html><head><meta charset=\"UTF-8\"></head>\n<body>\n<p>\n_hello world_\n</p>\n</body>\n</html>", result);
		}
	}
}
