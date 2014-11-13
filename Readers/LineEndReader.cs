namespace Mark.Readers
{
	public class LineEndReader : ITokenReader
	{
		public Token ReadToken(string from)
		{
			if (from.StartsWith("\r\n"))
				return new Token(from.Substring(0, 2), TokenType.LineEnd);
			if (from.StartsWith("\r") || from.StartsWith("\n"))
				return new Token(from.Substring(0, 1), TokenType.LineEnd);
			return null;
		}
	}
}
