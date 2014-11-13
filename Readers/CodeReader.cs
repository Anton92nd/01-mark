namespace Mark.Readers
{
	public class CodeReader : ITokenReader
	{
		public Token ReadToken(string from)
		{
			return (from.StartsWith("`")) ? new Token(from.Substring(0, 1), TokenType.Code) : null;
		}
	}
}
