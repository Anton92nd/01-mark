using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Mark
{
	class Converter
	{
		static public readonly string[] LineEndings = { "\r\n", "\r", "\n" };

		private static string[] GetParagraphsFromText(string[] text)
		{
			var result = new List<string>();
			while (text.Length > 0)
			{
				text = text.SkipWhile(x => x.Trim().Length == 0).ToArray();
				var paragraphLines = text.TakeWhile(x => x.Trim().Length > 0);
				text = text.SkipWhile(x => x.Trim().Length > 0).ToArray();
				var paragraph = String.Join(" ", paragraphLines);
				if (paragraph.Trim().Length > 0)
					result.Add("<p>\n" + paragraph + "\n</p>");
			}
			return result.ToArray();
		}

		static public string ConvertString(string content)
		{
			var paragraphs = GetParagraphsFromText(content.Split(LineEndings, StringSplitOptions.None));
			string result = "<html>";
			foreach (var i in paragraphs)
			{
				result += '\n' + i;
			}
			return result + "\n</html>\n";
		}

		static public void ConvertFile(string fileNameWithExtension)
		{
			using (var inputStream = new StreamReader(fileNameWithExtension))
			{
				var fileName = fileNameWithExtension.Split('.')[0];
				var outputStream = new StreamWriter(fileName + ".html");
				var result = ConvertString(inputStream.ReadToEnd());
				outputStream.Write(result);
				outputStream.Close();
			}
		}

		static void Main(string[] args)
		{
			string fileName = Console.ReadLine();
			ConvertFile(fileName);
		}
	}

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
			Converter.ConvertFile("empty.txt");
			var result = new StreamReader("empty.html").ReadToEnd().Split(Converter.LineEndings, StringSplitOptions.RemoveEmptyEntries);
			var expected = new StreamReader("emptyResult.txt").ReadToEnd().Split(Converter.LineEndings, StringSplitOptions.RemoveEmptyEntries);
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
