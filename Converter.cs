using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mark
{
	class Converter
	{
		static public readonly string[] LineEndings = { "\r\n", "\r", "\n" };

		static public string ConvertString(string content)
		{
			string result = "<html>\n" + content;
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
}
