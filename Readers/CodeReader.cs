namespace Mark.Readers
{
	public class CodeReader : ITokenReader
	{
		public Token ReadToken(string from)
		{
			if (!from.StartsWith("`"))
				return null;
			var i = 1;
			while (i < from.Length && from[i] != '`')
			{
				i += from[i] == '\\' ? 2 : 1;
			}
			return i >= from.Length ? null : new Token(from.Substring(0, i + 1), from.Substring(1, i - 1), TokenType.Code);
		}
	}
}
