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
		private static readonly List<ITokenReader> readers = new List<ITokenReader>
		{
			new LineEndReader(),
			new WhitespaceReader(),
			new WordReader(),
			new UnderscoreReader(),
			new EscapeReader(),
			new SeparatorReader()
		};

		public List<Token> Parse(string text)
		{
			var result = new List<Token>();
			while (text.Length > 0)
			{
				Token best = null;
				int bestLength = 0;
				foreach (var i in readers)
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
					best = new Token(text.Substring(0, 1), TokenType.Separator);
				}
				result.Add(best);
				text = text.Substring(best.source.Length);
			}
			return result;
		}
	}
}
