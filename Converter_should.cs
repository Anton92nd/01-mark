using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Mark
{
	[TestFixture]
	class Converter_should
	{
		[Test]
		public void throw_exception_if_no_such_file()
		{
			Assert.Throws<FileNotFoundException>(() => Converter.ConvertFile("no_such_file.txt"));
		}

		[Test]
		public void create_file_with_html_extension()
		{
			Converter.ConvertFile("sample.txt");
			Assert.True(File.Exists("sample.html"));
		}

		[Test]
		public void convert_empty_file_into_file_with_html_tag()
		{
			var lineEndings = new[] {"\r\n", "\r", "\n"};
			Converter.ConvertFile("empty.txt");
			var result = new StreamReader("empty.html").ReadToEnd().Split(lineEndings, StringSplitOptions.RemoveEmptyEntries);
			var expected = new StreamReader("emptyResult.txt").ReadToEnd().Split(lineEndings, StringSplitOptions.RemoveEmptyEntries);
			Assert.AreEqual(expected, result);
		}

		[Test]
		public void add_p_tags_around_paragraphs()
		{
			string text = "This\nis\nfirst paragraph\n   \nThis is\nsecond\n";
			string result = Converter.ConvertString(text);
			Assert.AreEqual("<html>\n<p>\nThis is first paragraph\n</p>\n<p>\nThis is second\n</p>\n</html>\n", result);
		}
	}
}
