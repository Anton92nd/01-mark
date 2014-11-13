using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mark.Readers;
using MoreLinq;

namespace Mark
{
	public static class Parser
	{
		private static readonly List<ITokenReader> Readers = new List<ITokenReader>
		{
			new LineEndReader(),
			new WhitespaceReader(),
			new WordReader(),
			new UnderscoreReader(),
			new EscapeReader(),
			new CodeReader(),
			new SeparatorReader()
		};

		public static List<Token> Parse(string text)
		{
			var result = new List<Token>();
			while (text.Length > 0)
			{
				var best = Readers.Select(x => x.ReadToken(text)).Where(token => token != null)
					.Concat(new Token(text.Substring(0, 1), HttpUtility.HtmlEncode(text.Substring(0, 1)), TokenType.Unknown))
					.MaxBy(token => token.Source.Length);
				result.Add(best);
				text = text.Substring(best.Source.Length);
			}
			return result;
		}
	}
}
