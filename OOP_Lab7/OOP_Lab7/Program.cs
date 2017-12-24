using System;
using System.IO;

namespace OOP_Lab7
{
	class MainClass
	{

		public static void Main (string[] args)
		{
			StreamReader f = new StreamReader ("input.txt");

			string buf = f.ReadToEnd ();

			string[] sentences = buf.Split( new char[]{'.'} );

			foreach (string sentence in sentences) {
				if (sentence.Contains ("\n")) // normal sentences don't contain newlines
					continue;

				if (sentence.Contains (",")) // exercise
					continue;

				string str;
				if (sentence [0] == ' ') { // remove leading space
					str = sentence.Remove(0, 1);
				} else {
					str = sentence;
				}

				Console.WriteLine (str);
			}
			Console.ReadKey();
		}
	}
}
