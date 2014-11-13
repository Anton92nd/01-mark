using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mark.Readers;

namespace Mark
{
	public class Parser
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

		public List<Token> Parse(string text)
		{
			var result = new List<Token>();
			while (text.Length > 0)
			{
				Token best = null;
				int bestLength = 0;
				foreach (var i in Readers)
				{
					Token current = i.ReadToken(text);
					if (current == null)
						continue;
					if (current.source.Length > bestLength)
					{
						best = current;
						bestLength = current.source.Length;
					}
				}
				if (best == null)
				{
					best = new Token(text.Substring(0, 1), TokenType.Unknown);
				}
				result.Add(best);
				text = text.Substring(best.source.Length);
			}
			return result;
		}
	}
}
