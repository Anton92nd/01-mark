using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.HTMLParser
{
	enum TokenType
	{
		LineEnd,
		Whitespace,
		Word,
		Underscore,
		Code,
		Separator
	}

	class Token
	{
		private string source { get; set; }
		private string value { get; set; }
		private TokenType type { get; set; }

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
	}
}
