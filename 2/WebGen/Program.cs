using System;

namespace WebGen
{
	class Program
	{
		static void Main(string[] args)
		{
			new Generator().Generate(@"..\..\content", @"c:\temp\bltoolkit\");
		}
	}
}
