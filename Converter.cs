using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Mark
{
	class Converter
	{
		static public void Convert(string fileName)
		{
			throw new NotImplementedException();
		}

		static void Main(string[] args)
		{
			
		}
	}

	[TestFixture]
	class Converter_should
	{
		[Test]
		public void create_file_with_html_extension()
		{
			Converter.Convert("sample.txt");
			Assert.True(File.Exists("sample.html"));
		}
	}
}
