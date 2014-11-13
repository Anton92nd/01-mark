using System.Linq;

namespace Mark.Readers
{
	public class UnderscoreReader : ITokenReader
	{
		public Token ReadToken(string from)
		{
			var count = from.TakeWhile(c => c == '_').Count();
			if (count > 2)
				return new Token(from.Substring(0, count), TokenType.Unknown);
			if (from.StartsWith("__"))
				return new Token("__", TokenType.DoubleUnderscore);
			return from.StartsWith("_") ? new Token("_", TokenType.Underscore) : null;
		}
	}
}
