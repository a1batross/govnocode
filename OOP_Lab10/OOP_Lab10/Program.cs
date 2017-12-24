using System;
using System.Collections.Generic;
namespace OOP_Lab10
{
	class MainClass
	{
		public const int SIZE = 3;

		public struct ZNAK
		{
			public string firstName;
			public string lastName;
			public int zodiac;
			public int[] birthday;
		}

		public static List<ZNAK> znaks = new List<ZNAK>(SIZE);

		public static void Main (string[] args)
		{
			bool valid = false;
			do {
				Console.WriteLine("1) Ввести");
				Console.WriteLine("2) Вывести");
				Console.WriteLine("3) Выход");

				switch( GetKey() )
				{
				case '1': EnterZnaks(); break;
				case '2': PrintZnaks(); break;
				case '3': valid = true; break;
				}
			} while(!valid);;
		}

		public static void EnterZnaks()
		{
			for (int i = 0; i < SIZE; i++) {
				ZNAK znak = new ZNAK();

				Console.Write ("Введите имя: ");
				znak.firstName = Console.ReadLine ();

				Console.Write ("Введите фамилию: ");
				znak.lastName = Console.ReadLine ();

				znak.zodiac = RangeInput ("Введите номер знака зодиака: ", INVALID_DATA, 1, 12);
				znak.birthday = new int[3];
				znak.birthday[2] = IntInput ("Введите год рождения: ");
				znak.birthday[1] = RangeInput ("Введите месяц рождения: ", INVALID_DATA, 1, 12 );
				znak.birthday[0] = RangeInput ("Введите день рождения: ", 
					INVALID_DATA, 1, GetMaxDayForDate( znak.birthday[1], znak.birthday[2] ) );
				znaks.Add( znak );
			}
			znaks.Sort (new ZnakComparer ());
		}

		public static void PrintZnaks()
		{
			int month, i;
			bool nofind = true;

			month = RangeInput ("Введите месяц рождения: ", INVALID_DATA, 1, 12 );

			for (i = 0; i < znaks.Count; i++) {
				ZNAK znak = znaks [i];
				if (znak.birthday [1] == month) {

					Console.WriteLine ("{0, 10} | {1,10} | {2,10} | {3}.{4},{5}", 
						znak.lastName, znak.firstName, znak.zodiac, 
						znak.birthday[0], znak.birthday[1], znak.birthday[2] );

					nofind = false;
				}
			}

			if (nofind) {
				Console.WriteLine ("Нет таких людей!");
			}
		}

		public class ZnakComparer : IComparer<ZNAK>
		{
			public int Compare( ZNAK a, ZNAK b )
			{
				if (a.zodiac < b.zodiac)
					return -1;
				if (a.zodiac > b.zodiac)
					return 1;
				return 0;
			}
		} 

		static char GetKey()
		{
			ConsoleKeyInfo key = Console.ReadKey ();
			Console.WriteLine ();
			return key.KeyChar;
		}

		static int GetMaxDayForDate( int month, int year )
		{
			if (month == 2) {
				if (year % 4 != 0)
					return 28;
				else
					return 29;
			} else if (month % 2 == 0)
				return 30;
			else
				return 31;
		}

		static int RangeInput( string str, string invalid, int min, int max )
		{
			bool valid = false;
			bool once = false;
			int i;
			Console.Write (str);
			do {
				if( once ) Console.WriteLine (invalid);
				else once = true;
				valid = int.TryParse( Console.ReadLine(), out i );
				if( valid )
					valid = i >= min && i <= max;
				
			} while( !valid);
			return i;
		}

		static private string INVALID_DATA = "Введены неверные данные.";

		static int IntInput( string str )
		{
			RangeInput (str, INVALID_DATA, Int32.MinValue, Int32.MaxValue);
		}
	}
}
