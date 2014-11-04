using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Mark
{
	class Converter
	{
		static public readonly string[] LineEndings = { "\r\n", "\r", "\n" };

		private static string[] GetParagraphsFromText(string[] text)
		{
			return text;
		}

		private static void WriteParagraphsToStream(StreamWriter outputStream, string[] paragraphs)
		{
			return;
		}

		static public void Convert(string fileNameWithExtension)
		{
			using (var inputStream = new StreamReader(fileNameWithExtension))
			{
				var fileName = fileNameWithExtension.Split('.')[0];
				var outputStream = new StreamWriter(fileName + ".html");
				var text = inputStream.ReadToEnd().Split(LineEndings, StringSplitOptions.None);
				var paragraphs = GetParagraphsFromText(text);
				outputStream.WriteLine("<html>");
				WriteParagraphsToStream(outputStream, paragraphs);
				outputStream.WriteLine("</html>");
				outputStream.Close();
			}
		}

		static void Main(string[] args)
		{
			string fileName = Console.ReadLine();
			Convert(fileName);
		}
	}

	[TestFixture]
	class Converter_should
	{
		[Test]
		public void throw_exception_if_no_such_fule()
		{
			Assert.Throws<FileNotFoundException>(() => Converter.Convert("no_such_file.txt"));
		}

		[Test]
		public void create_file_with_html_extension()
		{
			Converter.Convert("sample.txt");
			Assert.True(File.Exists("sample.html"));
		}

		[Test]
		public void convert_empty_file_into_file_with_html_tag()
		{
			Converter.Convert("empty.txt");
			var result = new StreamReader("empty.html").ReadToEnd();
			var expected = new StreamReader("emptyResult.txt").ReadToEnd();
			Assert.True(expected.Equals(result));
		}
	}
}
