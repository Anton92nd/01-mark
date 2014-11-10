using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.HTMLParser
{
	public class EscapeReader : ITokenReader
	{
		public Token ReadToken(string from)
		{
			if (!from.StartsWith("\\") || from.Length < 2)
				return null;
			string value = from.Substring(1, 1);
			if (value.Equals("<"))
				value = "&lt;";
			if (value.Equals(">"))
				value = "&gt;";
			return new Token(from.Substring(0, 2), value, TokenType.Separator);
		}
	}
}
