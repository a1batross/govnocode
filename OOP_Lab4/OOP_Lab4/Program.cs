using System;

namespace OOP_Lab4
{
	class MainClass
	{
		public static double DoubleInput( string msg )
		{
			double fl;
			bool valid = false;
			do {
				Console.Write(msg);
				valid = Double.TryParse (Console.ReadLine (), out fl);
			} while(!valid);
			return fl;
		}

		public static void Main (string[] args)
		{
			double a, b, c;

			a = DoubleInput ("A = ");
			b = DoubleInput ("B = ");
			c = DoubleInput ("C = ");

			QuadraticEquation eq = new QuadraticEquation (a, b, c);

			double[] roots;

			try
			{
				roots = eq.Solve();
			}
			catch( QuadraticEquation.NoRootsException ) 
			{
				Console.WriteLine ("No roots!");
                Console.ReadKey();
                return;
			}


			if (roots.Length == 1) {
				Console.WriteLine ( "x = " + roots[0]);
			} else {
				Console.WriteLine ( "x1 = " + roots[0]);
				Console.WriteLine ( "x2 = " + roots[1]);
			}

            Console.ReadKey();
		}
	}
}
