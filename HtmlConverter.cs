using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Mark
{
	public class HtmlConverter
	{
		private static readonly Dictionary<TokenType, string> TypeToTag = new Dictionary<TokenType, string>
		{
			{TokenType.Code, "code"}, {TokenType.Underscore, "em"}, {TokenType.DoubleUnderscore, "strong"}
		};

		private static readonly Dictionary<TokenType, string> TypeToSource = new Dictionary<TokenType, string>
		{
			{TokenType.Code, "`"}, {TokenType.Underscore, "_"}, {TokenType.DoubleUnderscore, "__"}
		}; 

		private static string BuildTree(List<int>[] edges, List<Token> tokens, int node, bool code = false)
		{
			bool isCode =  code || node > 0 && tokens[node - 1].type == TokenType.Code;
			var subTree = edges[node].Aggregate("", (current, i) => current + BuildTree(edges, tokens, i, isCode));
			if (node == 0)
				return subTree;
			if (TypeToTag.ContainsKey(tokens[node - 1].type))
			{
				var tag = TypeToTag[tokens[node - 1].type];
				var oldTag = TypeToSource[tokens[node - 1].type];
				return code ? oldTag + subTree + oldTag : "<" + tag + ">" + subTree + "</" + tag + ">";
			}
			return tokens[node - 1].value;
		}

		private static string ConstructHtmlParagraph(List<Token> tokens)
		{
			var edges = new List<int>[tokens.Count + 1];
			for (int i = 0; i < edges.Length; i++)
				edges[i] = new List<int>();
			var stack = new Stack<Tuple<TokenType, int>>();
			stack.Push(new Tuple<TokenType, int>(TokenType.Word, 0));
			for (int i = 0; i < tokens.Count; i++)
			{
				var token = tokens[i];
				if (!TypeToTag.ContainsKey(token.type))
				{
					edges[stack.Peek().Item2].Add(i + 1);
					continue;
				}
				if (token.type == stack.Peek().Item1)
				{
					if (token.type == TokenType.Code || i == tokens.Count - 1 || tokens[i + 1].type != TokenType.Word)
					{
						stack.Pop();
						continue;
					}
				}
				edges[stack.Peek().Item2].Add(i + 1);
				if (token.type == TokenType.Code || i == 0 || tokens[i - 1].type != TokenType.Word)
				{
					stack.Push(new Tuple<TokenType, int>(token.type, i + 1));
				}
			}
			return BuildTree(edges, tokens, 0);
		}

		private static List<string> BuildParagraphs(string[] lines)
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

		public static string ConvertString(string content)
		{
			var lines = content.Split(LineEndings, StringSplitOptions.None).ToArray();
			var paragraphs = BuildParagraphs(lines);
			var parsedParagraphs = paragraphs.Select(x => ConstructHtmlParagraph(Parser.Parse(x))).ToArray();
			return "<html><head><meta charset=\"UTF-8\"></head>\n<body>\n" + 
				parsedParagraphs.Aggregate("", (result, str) => result + "<p>\n" + str + "\n</p>\n") + "</body>\n</html>";
		}

		public static void ConvertFile(string fileNameWithExtension)
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

		public static void Main(string[] args)
		{
			string fileName = Console.ReadLine();
			ConvertFile(fileName);
		}
	}
}
