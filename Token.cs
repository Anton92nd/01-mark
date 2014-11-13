namespace Mark
{
	public enum TokenType
	{
		LineEnd,
		Whitespace,
		Word,
		Underscore,
		DoubleUnderscore,
		Code,
		Separator,
		Unknown
	}

	public class Token
	{
		public readonly string Source, Value;
		public readonly TokenType Type;

		public Token(string source, string value, TokenType type)
		{
			this.Source = source;
			this.Value = value;
			this.Type = type;
		}

		public Token(string source, TokenType type)
		{
			this.Source = source;
			this.Value = source;
			this.Type = type;
		}

		public override bool Equals(object obj)
		{
			var token = obj as Token;
			if (token == null)
				return false;
			return Source.Equals(token.Source) && Value.Equals(token.Value) && token.Type == Type;
		}
	}
}
