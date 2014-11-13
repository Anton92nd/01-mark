using System.Linq;

namespace Mark.Readers
{
	public class WhitespaceReader : ITokenReader
	{
		private static readonly char[] LineEndings = { '\r', '\n' };
		public Token ReadToken(string from)
		{
			var i = 0;
			while (i < from.Length && char.IsWhiteSpace(from[i]) && !LineEndings.Contains(from[i]))
				i++;
			return i == 0 ? null : new Token(from.Substring(0, i), TokenType.Whitespace);
		}
	}
}
