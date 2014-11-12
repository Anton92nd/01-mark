using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.HTMLParser
{
	public class UnderscoreReader : ITokenReader
	{
		public Token ReadToken(string from)
		{
			if (from.StartsWith("__"))
				return new Token("__", TokenType.DoubleUnderscore);
			return from.StartsWith("_") ? new Token("_", TokenType.Underscore) : null;
		}
	}
}
