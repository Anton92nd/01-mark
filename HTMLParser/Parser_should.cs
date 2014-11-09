using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Mark.HTMLParser
{
	[TestFixture]
	class Parser_should
	{
		[Test]
		public void return_empty_list_on_empty_string()
		{
			var result = new Parser().Parse("");
			Assert.IsEmpty(result);
		}
	}
}
