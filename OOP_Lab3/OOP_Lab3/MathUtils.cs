using System;
using System.Collections;

namespace OOP_Lab3
{
	class MathUtils
	{
		static public double Circle( double r, double x0, double y0, double x )
		{
			// (x - x0)^2 + (y - y0)^2 = r^2
			return Math.Sqrt( Math.Pow( r, 2 ) - Math.Pow( ( x - x0 ), 2 ) ) + y0;
		}

		static public bool IsInCircle( double r, double x0, double y0, double x, double y )
		{
			// (x - x0)^2 + (y - y0)^2 = r^2
			return Math.Pow( x - x0, 2 ) + Math.Pow( y - y0, 2 ) <= Math.Pow( r, 2 );
		}

		static public double Line( double x1, double y1, double x2, double y2, double x )
		{
			// ( y - y1 ) / ( y2 - y1 ) = ( x - x1 ) / ( x2 - x1 )
			return ( ( x - x1 ) * ( y2 - y1 ) ) / ( x2 - x1 ) + y1;
		}

		static public bool IsRightOfLine( double x1, double y1, double x2, double y2, double x, double y )
		{
			double l = ( y - y1 ) / ( y2 - y1 );
            double r = ( x - x1 ) / ( x2 - x1 );

			return l < r && l != r;
		}
	}
}
