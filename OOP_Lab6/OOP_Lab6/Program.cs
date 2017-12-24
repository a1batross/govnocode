using System;

namespace OOP_Lab6
{
	class MainClass
	{
		//static double fl = -5;
		static double DoubleInput( string msg )
		{
			//return fl++;

			double fl;
			bool valid = false;
			Console.Write (msg);
			do {
				valid = Double.TryParse( Console.ReadLine(), out fl );
			} while(!valid);
			return fl;
		}

		static int IntInput( string msg )
		{
			int i;
			bool valid = false;
			Console.Write (msg);
			do {
				valid = int.TryParse( Console.ReadLine(), out i );
			} while(!valid);
			return i;
		}

		static void PrintMatrix( double[][] matrix )
		{
			foreach( double[] arr in matrix )
			{
				foreach( double fl in arr )
				{
					Console.Write (fl + " ");
				}
				Console.WriteLine();
			}
		}

		static void Swap<T>( ref T left, ref T right )
		{
			T tmp = right;
			right = left;
			left = tmp;
		}

		static void Do( ref double[][] matrix )
		{
			int n = matrix.Length; // square matrix
			double max = Double.MinValue;
			int mx = 0, my = 0;

			for (int d = 0; d < n; d++)
			{
				// find max
				for( int i = 0; i < n; i++ )
				{
					for( int j = 0; j < n; j++ )
					{
						// skip already swapped
						if( i == j && i < d )
							continue;

						if( matrix[i][j] > max )
						{
							max = matrix[i][j];
							mx = i;
							my = j;
						}
					}
				}

				// actually swap
				Swap( ref matrix[d][d], ref matrix[mx][my] );
				max = Double.MinValue;
			}
		}

		public static void Main (string[] args)
		{
			int n = IntInput ("N = ");
			double[][] matrix = new double[n][];

			for (int i = 0; i < n; i++) {
				matrix [i] = new double[n];
				for (int j = 0; j < n; j++) {
					matrix[i][j] = DoubleInput( "M[" + i + "][" + j + "] = " );
				}
			}

			PrintMatrix (matrix);

			Do (ref matrix);
			PrintMatrix (matrix);
			Console.ReadKey();
		}
	}
}
