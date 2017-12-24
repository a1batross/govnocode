using System;

namespace OOP_Lab4
{
	public class QuadraticEquation
	{
		public QuadraticEquation( double a, double b, double c )
		{
			mA = a; mB = b; mC = c;
		}

		private double mA, mB, mC;

		public double[] Solve()
		{
			double D = mB * mB - 4 * mA * mC;

			if( D < 0 )
			{
				throw new NoRootsException();
			}

			double firstPart = -mB / 2 * mA;
			const double epsilon = 0.01;
			double[] roots;

			if( Math.Abs(D) < epsilon)
			{
				roots = new double[1];
				roots [0] = firstPart;
				return roots;
			}

			double secondPart = Math.Sqrt (D) / 2 * mA;
			roots = new double[2];
			roots [0] = firstPart + secondPart;
			roots [1] = firstPart - secondPart;
			return roots;
		}

		public class NoRootsException : Exception { }
	}
}

