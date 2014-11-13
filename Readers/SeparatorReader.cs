using System.Collections.Generic;
using System.Linq;

namespace Mark.Readers
{
	public class SeparatorReader : ITokenReader
	{
		private static readonly char[] Separators =  {'.', ',', '\'', '\"', '!', '?', '%', '@',
			'#', '$', '^', '&', '*', '/', '<', '>', '-', '=' };

		private static readonly Dictionary<string, string> NeedEscape = new Dictionary<string, string>()
		{
			{"&", "&amp;"}, {"<", "&lt;"}, {">", "&gt;"}
		};

		public Token ReadToken(string from)
		{
			if (from.Length == 0)
				return null;
			string result = from.Substring(0, 1);
			if (NeedEscape.ContainsKey(result))
			{
				result = NeedEscape[result];
			}
			return Separators.Contains(from[0]) ? new Token(from.Substring(0, 1), result, TokenType.Separator) : null; 
		}
	}
}
