// #define USE_RANDOM

using System;

namespace OOP_Lab5
{
	class MainClass
	{
		private static int IntInput( string msg )
		{
			int i;
			bool valid = false;

			Console.WriteLine (msg);
			do {
				valid = int.TryParse (Console.ReadLine (), out i);
			} while( !valid );
			return i;
		}

        private static double DoubleInput(string msg)
        {
            double i;
            bool valid = false;

            Console.WriteLine(msg);
            do
            {
                valid = double.TryParse(Console.ReadLine(), out i);
            } while (!valid);
            return i;
        }

        private static void Ex1( double[] arr, double C )
		{
            int n = 0;
            foreach (double fl in arr)
                if (fl < C) n++;
            Console.WriteLine("Количество элементов массива, меньших " + C + " = " + n);
		}

		private static void Ex2(double[] arr)
		{
            double sum = 0;
            for( int i = arr.Length-1; i >= 0; i-- )
            {
                if( arr[i] == (int)arr[i])
                {
                    sum += arr[i];
                }

                if (arr[i] < 0)
                    break;
            }
            Console.WriteLine("Сумма целых частей, расположенных после последнего отрицательного = " + sum);
        }

		static void PrintArray<T>( T[] arr )
		{
			foreach (T i in arr)
				Console.Write (i + " ");
			Console.WriteLine ();
		}

		private static void Ex3(double[] arr)
		{
			Console.WriteLine ("Преобразованный массив: ");

            double max = arr[0];

            foreach (double fl in arr)
                if (max < fl) max = fl;

            int j = 0;

            for( int i = 0; i < arr.Length; i++ )
            {
                if( arr[i] < max * 0.2 )
                {
                    if( j != i )
                    {
                        double temp = arr[j];
                        arr[j] = arr[i];
                        arr[i] = temp;
                        j++;
                    }
                }
            }

            PrintArray(arr);
		}

		public static void Main (string[] args)
		{
			int n = IntInput ("N = ");
			double[] arr = new double[n];

            double C = DoubleInput("C = ");

			for (int i = 0; i < n; i++) {
				arr [i] = DoubleInput ("a[" + i + "] = ");
			}

			Ex1 (arr, C);
			Ex2 (arr);
			Ex3 (arr);

            Console.ReadKey();
		}
	}
}
