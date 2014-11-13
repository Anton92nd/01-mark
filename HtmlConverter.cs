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

		private static string ConstructHtmlParagraph(List<Token> tokens)
		{
			var newTokens = (new List<Token> { { new Token("", TokenType.Unknown) } }
				.Concat(tokens)).ToList();
			var treeEdges = BuildTree(newTokens);
			return ConcatTree(treeEdges, newTokens, 0);
		}

		private static string ConcatTree(Tuple<List<int>,bool>[] edges, List<Token> tokens, int node, bool code = false)
		{
			var subTree = edges[node].Item1.Aggregate("", (str, i) =>
				str + ConcatTree(edges, tokens, i, code || tokens[node].Type == TokenType.Code));
			if (!TypeToTag.ContainsKey(tokens[node].Type) || edges[node].Item2)
				return tokens[node].Value + subTree;
			var tag = TypeToTag[tokens[node].Type];
			if (tokens[node].Type == TokenType.Code)
				subTree = tokens[node].Value + subTree;
			var oldTag = tokens[node].Source;
			return code ? oldTag + subTree + oldTag : "<" + tag + ">" + subTree + "</" + tag + ">";
		}

		private static Tuple<List<int>, bool>[] BuildTree(List<Token> tokens)
		{
			var treeEdges = new Tuple<List<int>, bool>[tokens.Count()];
			for (var i = 0; i < treeEdges.Length; i++)
				treeEdges[i] = new Tuple<List<int>, bool>(new List<int>(), false);
			var stack = new Stack<Tuple<TokenType, int>>();
			var counter = new Dictionary<TokenType, int>
			{
				{TokenType.Underscore, 0}, {TokenType.DoubleUnderscore, 0}
			};
			stack.Push(new Tuple<TokenType, int>(TokenType.Unknown, 0));
			for (var i = 1; i < tokens.Count(); i++)
			{
				var token = tokens[i];
				if (counter.ContainsKey(token.Type))
				{
					if (counter[token.Type] == 0)
					{
						counter[token.Type]++;
						treeEdges[stack.Peek().Item2].Item1.Add(i);
						stack.Push(new Tuple<TokenType, int>(token.Type, i));
						continue;
					}
					while (stack.Peek().Item1 != token.Type)
					{
						treeEdges[stack.Peek().Item2] = new Tuple<List<int>, bool>
							(treeEdges[stack.Peek().Item2].Item1, true);
						if (counter.ContainsKey(stack.Peek().Item1))
							counter[stack.Peek().Item1]--;
						stack.Pop();
					}
					counter[stack.Peek().Item1]--;
					stack.Pop();
				}
				else
					treeEdges[stack.Peek().Item2].Item1.Add(i);
			}
			while (stack.Count() > 1)
			{
				treeEdges[stack.Peek().Item2] = new Tuple<List<int>, bool>
							(treeEdges[stack.Peek().Item2].Item1, true);
				stack.Pop();
			}
			return treeEdges;
		}

		private static IEnumerable<string> BuildParagraphs(string[] lines)
		{
			var result = new List<string>();
			while (lines.Length > 0)
			{
				lines = lines.SkipWhile(line => line.Trim().Length == 0).ToArray();
				if (lines.Length == 0)
					break;
				var paragraph = lines.TakeWhile(line => line.Trim().Length > 0).ToList();
				result.Add(paragraph.Aggregate((sentence, line) => sentence + "\r\n" + line));
				lines = lines.SkipWhile(line => line.Trim().Length > 0).ToArray();
			}
			return result;
		}

		private static readonly string[] LineEndings = { "\r\n", "\r", "\n" };

		public static string ConvertString(string content)
		{
			var lines = content.Split(LineEndings, StringSplitOptions.None).ToArray();
			var paragraphs = BuildParagraphs(lines);
			var parsedParagraphs = paragraphs.Select(x => ConstructHtmlParagraph(Parser.Parse(x)));
			return "<html><head><meta charset=\"UTF-8\"></head>\r\n<body>\r\n" +
				parsedParagraphs.Aggregate("", (result, str) => result + "<p>\r\n" + str + "\r\n</p>\r\n") + "</body>\r\n</html>";
		}

		public static void ConvertFile(string fileNameWithExtension)
		{
			var text = File.ReadAllText(fileNameWithExtension);
			var fileName = fileNameWithExtension.Substring(0, fileNameWithExtension.LastIndexOf('.'));
			var result = ConvertString(text);
			File.WriteAllText(fileName + ".html", result);
		}

		public static void Main(string[] args)
		{
			string fileName = Console.ReadLine();
			ConvertFile(fileName);
		}
	}
}
