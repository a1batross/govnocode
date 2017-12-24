#define USE_PREDEFINED
using System;
using System.IO;
using System.Diagnostics;

namespace OOP_Lab3
{
	class Program
	{
		static double Ex3_fn( double x, int n )
		{
			double val = Math.Pow( x - 1, n + 1 ) / ( n + 1 );
			if( (n & 1) == 1 )
			{
				val *= -1;
			}
			return val;
		}

		static bool Ex2_fn( double x, double y, double R )
		{
			if( y >= 0.0f )
			{
				if( MathUtils.IsInCircle( R, R, R, x, y ) &&
					MathUtils.IsRightOfLine( -1, -1, 1, 1, x, y ) )
					return true;
			}
			else
			{
				if( MathUtils.IsInCircle( R, -R, -R, x, y ) &&
					!MathUtils.IsRightOfLine( -1, -1, 1, 1, x, y ) )
					return true;
			}
			return false;
		}

		static double Ex1_fn( double x )
		{
			if( x <= -2 )
			{
				return MathUtils.Line( -3, 1, -2, 0, x );
			}
			else if( x > -2 && x <= 0 )
			{
				return MathUtils.Circle( 1, -1, 0, x );
			}
			else if( x > 0 && x <= 4 )
			{
				return MathUtils.Circle( 2, 2, 0, x );
			}
			else if( x > 4 && x <= 6 )
			{
				return MathUtils.Line( 4, 0, 6, 1, x );
			}
			return MathUtils.Line( 6, 1, 7, 1, x );
		}

		static void Ex1()
		{
			// StreamReader f;
			double xmin, xmax, dx;

#if !USE_PREDEFINED
			try
			{
				f = new StreamReader( "input1.txt" );
			}
			catch( FileNotFoundException e )
			{
				Console.WriteLine( "Couldn't open file" );
				return;
			}

			xmin = Convert.ToDouble( f.ReadLine() );
			xmax = Convert.ToDouble( f.ReadLine() );
			dx = Convert.ToDouble( f.ReadLine() );
#else
			xmin = 0.0f;
			xmax = 1.0f;
			dx = 0.1;
#endif

			Console.WriteLine( "{0,10} | {1,10}", "x", "f(x)" );
			for( double x = xmin; x < xmax; x += dx )
			{
				Console.WriteLine( "{0:0.######,10} | {1:0.######,10}", x, Ex1_fn( x ) );
			}
			Console.WriteLine();
		}

		static void Ex2()
		{
			double R;
			double[] x, y;
			x = new double[10];
			y = new double[10];

#if !USE_PREDEFINED
			Console.WriteLine( "Введите радиус: " );
			R = Convert.ToDouble(Console.ReadLine());
		
			Console.WriteLine( "Введите десять координат: " );
			for( int i = 0; i < 10; i++ )
			{
				Console.WriteLine( "X: " );
				x[i] = Convert.ToDouble( Console.ReadLine() );

				Console.WriteLine( "Y: " );
				y[i] = Convert.ToDouble( Console.ReadLine() );
			}
#else
			R = 1.0f;
			x[0] = 0.25f; y[0] = 0.25f;
			x[1] = 0.75f; y[1] = 0.75f;
			x[2] = 0.25f; y[2] = 0.75f;
			x[3] = 0.75f; y[3] = 0.25f;
			x[4] = 0.0f;  y[4] = 0.0f;
			x[5] = -0.25f; y[5] = -0.25f;
			x[6] = -0.75f; y[6] = -0.75f;
			x[7] = -0.25f; y[7] = -0.75f;
			x[8] = -0.75f; y[8] = -0.25f;
			x[9] = -0.33f; y[9] = 0.5f;
#endif
			Console.WriteLine( "{0,10} | {1,10} | {2,10}", "x", "y", "область" );
			for( int i = 0; i < 10; i++ )
			{
				Console.WriteLine( "{0:0.######,10} | {1:0.######,10} | {2:0.######,10}", x[i], y[i], Ex2_fn( x[i], y[i], R ) ? "да" : "нет" );
			}
			Console.WriteLine();
		}

		static void Ex3()
		{
			double sum = 0;
			double dx = 0.2;
			Console.WriteLine( "{0,10} | {1,10} | {2,10} | {3,10}", "n", "x", "f(x)", "sum(f(x))" );
			for( int i = 0; i < 10; i++ )
			{
				double x = 0 + i * dx;
				double fn = Ex3_fn( x, i );
				sum += fn;
				Console.WriteLine( "{0:0.######,10} | {1:0.######,10} | {2:0.######,10} | {3:0.######,10}", i, x, fn, sum );
			}
			Console.WriteLine();
		}

		static void Main( string[] args )
		{
            //Ex1();
			//Ex2();
            Ex3();

            Console.ReadKey();
		}
	}
}
