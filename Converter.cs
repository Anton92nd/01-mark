using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Mark.HTMLParser;

namespace Mark
{
	class Converter
	{

		private static Parser InitParser()
		{
			var parser = new Parser();
			return parser;
		}

		private static string ConstructHTML(List<Token> tokens)
		{
			return "<html>\n</html>\n";
		}

		static public string ConvertString(string content)
		{
			var parser = InitParser();
			var tokens = parser.Parse(content);
			return ConstructHTML(tokens);
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
}
