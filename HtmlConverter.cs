using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Mark.HTMLParser;

namespace Mark
{
	public class HtmlConverter
	{
		private static readonly Dictionary<TokenType, string> TypeToTag = new Dictionary<TokenType, string>
		{
			{TokenType.Code, "code"}, {TokenType.Underscore, "em"}, {TokenType.DoubleUnderscore, "strong"}
		};

		private static string RecursiveConstruct(List<Token> tokens, int start, int finish, bool code = false)
		{
			string result = "";
			if (code)
			{
				return tokens.Skip(start).Take(finish - start).Select(token => token.value)
					.Aggregate("", (str, token) => str + token);
			}
			int position = start;
			while (position < finish)
			{
				var token = tokens[position];
				if (token.type == TokenType.Underscore || token.type == TokenType.DoubleUnderscore)
				{
					int i = -1;
					for (int j = position + 1; j < finish; j++)
					{
						if (tokens[j].type == token.type && (j + 1 == tokens.Count || tokens[j + 1].type != TokenType.Word))
						{
							i = j;
							break;
						}
					}
					if (i > position)
					{
						var tag = TypeToTag[token.type];
						result += "<" + tag + ">" + RecursiveConstruct(tokens, position + 1, i) + "</" + tag + ">";
						position = i + 1;
						continue;
					}
				} 
				if (token.type == TokenType.Code)
				{
					int i = tokens.FindIndex(position + 1, current => current.type == TokenType.Code);
					if (i > start && i < finish)
					{
						result += "<code>" + RecursiveConstruct(tokens, position + 1, i, true) + "</code>";
						position = i + 1;
						continue;
					}
				}
				result += token.value;
				position++;
			}
			return result;
		}

		private static string ConstructHtmlParagraph(List<Token> tokens)
		{
			return RecursiveConstruct(tokens, 0, tokens.Count);
		}

		static private List<string> BuildParagraphs(string[] lines)
		{
			var result = new List<string>();
			while (lines.Length > 0)
			{
				lines = lines.SkipWhile(line => line.Trim().Length == 0).ToArray();
				if (lines.Length == 0)
					break;
				var paragraph = lines.TakeWhile(line => line.Trim().Length > 0).ToList();
				result.Add(paragraph.Aggregate((sentence, line) => sentence + "\n" + line));
				lines = lines.SkipWhile(line => line.Trim().Length > 0).ToArray();
			}
			return result;
		}

		private static readonly string[] LineEndings = { "\r\n", "\r", "\n" };
		private static readonly Parser Parser = new Parser();

		static public string ConvertString(string content)
		{
			var lines = content.Split(LineEndings, StringSplitOptions.None).ToArray();
			var paragraphs = BuildParagraphs(lines);
			var parsedParagraphs = paragraphs.Select(x => ConstructHtmlParagraph(Parser.Parse(x))).ToArray();
			return "<html><head><meta charset=\"UTF-8\"></head>\n" + parsedParagraphs.Aggregate("", (result, str) => result + "<p>\n" + str + "\n</p>\n") + "</html>";
		}

		static public void ConvertFile(string fileNameWithExtension)
		{
			using (var input = File.OpenText(fileNameWithExtension))
			{
				var fileName = fileNameWithExtension.Split('.')[0];
				var result = ConvertString(input.ReadToEnd());
				var outputStream = File.CreateText(fileName + ".html");
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
