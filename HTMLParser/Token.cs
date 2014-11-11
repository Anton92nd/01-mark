using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.HTMLParser
{
	public enum TokenType
	{
		LineEnd,
		Whitespace,
		Word,
		Underscore,
		Code,
		Separator
	}

	public class Token
	{
		public readonly string source, value;
		public readonly TokenType type;

		public Token(string source, string value, TokenType type)
		{
			this.source = source;
			this.value = value;
			this.type = type;
		}

		public Token(string source, TokenType type)
		{
			this.source = source;
			this.value = source;
			this.type = type;
		}

		public override bool Equals(object obj)
		{
			var token = obj as Token;
			if (token == null)
				return false;
			return source.Equals(token.source) && value.Equals(token.value) && token.type == type;
		}
	}
}
