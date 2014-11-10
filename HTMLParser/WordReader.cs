using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.HTMLParser
{
	public class WordReader : ITokenReader
	{

		public Token ReadToken(string from)
		{
			if (from.StartsWith("_"))
				return null;
			var i = 0;
			while (i < from.Length && (char.IsLetterOrDigit(from[i]) || from[i] == '_'))
				i++;
			while (i > 0 && from[i - 1] == '_')
				i--;
			return i == 0 ? null : new Token(from.Substring(0, i), TokenType.Word);
		}
	}
}
