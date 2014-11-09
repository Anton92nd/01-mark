using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mark.HTMLParser
{
	public class Parser
	{
		private List<ITokenReader> readers;

		public List<Token> Parse(string text)
		{
			return new List<Token>();
		}
	}
}
