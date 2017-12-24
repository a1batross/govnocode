using System;

namespace OOP_Lab9
{
	class MainClass
	{
		public static void PrintMemory( ref Element element )
		{
			Console.Write (element.Name + ": ");
			for (int i = 0; i < element.Outputs; i++) {
				Console.Write (" " + element.GetOutputData (i));
			}
			Console.WriteLine ();
		}

		public static void TestElement( ref Element element, params int[] param )
		{
			for (int i = 0; i < param.Length; i++)
				element.SetInputData (i, param [i]);
			element.Run ();
			PrintMemory ( ref element );
		}

		public static void Main (string[] args)
		{
			Console.WriteLine ("Hello World!");

			Element memory = new Register.Memory ();
			Element register = new Register ( "Register" );
			Element comb = new CombinationalElement ("CombinationalElement", 8);

			TestElement (ref memory, 0, 0);
			TestElement (ref memory, 1, 0);
			TestElement (ref memory, 0, 1);
			TestElement (ref memory, 0, 0);
			TestElement (ref memory, 1, 0);
			TestElement (ref memory, 1, 1);

			TestElement (ref register, 1, 0, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0);
			TestElement (ref register, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0);

			TestElement (ref comb, 1, 1, 1, 1, 1, 1, 0, 0);
			TestElement (ref comb, 0, 0, 0, 0, 0, 0, 0, 0);
			TestElement (ref comb, 1, 1, 1, 1, 1, 1, 1, 1);
		}
	}
}
