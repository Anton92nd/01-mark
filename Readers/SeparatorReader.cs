using System.Linq;
using System.Web;

namespace Mark.Readers
{
	public class SeparatorReader : ITokenReader
	{
		private static readonly char[] Separators =  {'.', ',', '\'', '\"', '!', '?', '%', '@',
			'#', '$', '^', '&', '*', '/', '<', '>', '-', '=' };

		public Token ReadToken(string from)
		{
			if (from.Length == 0)
				return null;
			string result = from.Substring(0, 1);
			return Separators.Contains(from[0]) ? new Token(result, HttpUtility.HtmlEncode(result), TokenType.Separator) : null; 
		}
	}
}
