using System;

namespace OOP_Lab8
{
	class MainClass
	{
		static Storage storage = new Storage();

		static char GetKey()
		{
			ConsoleKeyInfo key = Console.ReadKey ();
			Console.WriteLine ();
			return key.KeyChar;
		}

		static int IntInput( string str )
		{
			bool valid = false;
			int i;
			Console.Write (str);
			do {
				valid = int.TryParse( Console.ReadLine(), out i );
			} while( !valid);
			return i;
		}

		static void MainMenu()
		{
			bool valid = false;
			do {
				Console.WriteLine ("Выберите действие: ");
				Console.WriteLine ("1) Добавить");
				Console.WriteLine ("2) Отоборазить на экране");
				Console.WriteLine ("3) Отсортировать");
				Console.WriteLine ("4) Найти");
				Console.WriteLine ("5) Сложить");
				Console.WriteLine ("6) Выйти");

				switch( GetKey() )
				{
				case '1':
					AddProductMenu();
					break;
				case '2':
					storage.Print();
					break;
				case '3':
					SortProductMenu(); 
					break;
				case '4':
					FindMenu();
					break;
				case '5':
					SumProductsMenu();
					break;
				case '6':
					valid = true;
					break;
				}
			} while(!valid);

		}

		static Product FindMenu()
		{
			Product pr = null;

			bool valid = false;
			do {
				Console.WriteLine ("Выберите поиск: ");
				Console.WriteLine ("1) По имени");
				Console.WriteLine ("2) По ID");

				switch (GetKey ()) {
				case '1':
					Console.Write("Имя: ");
					string name = Console.ReadLine();
					try
					{
						pr = storage.GetProductByName(name);
					}
					catch( Storage.NotFoundException )
					{
						Console.WriteLine("Нет такого");
						valid = false;
						pr = null;
						break;
					}
					pr.Print();
					return pr;
				case '2':
					int idx = IntInput("ID: ");
					try
					{
						pr = storage.GetProductByIndex( idx );
					}
					catch( Storage.NotFoundException )
					{
						Console.WriteLine("Нет такого");
						valid = false;
						pr = null;
						break;
					}
					pr.Print();
					return pr;
				}
			} while(!valid);

			return null;
		}

		static void SumProductsMenu()
		{
			Console.WriteLine ("Найдите первый элемент");
			Product pr1 = null;
			do {
				pr1 = FindMenu();
			} while(pr1 == null);

			Console.WriteLine ("Найдите второй элемент");
			Product pr2 = null;
			do {
				pr2 = FindMenu();
			} while(pr2 == null);

			int sum = (int)(pr1 + pr2);

			Console.WriteLine ("Сумма их цен: " + sum);
		}

		static void AddProductMenu()
		{
			Console.WriteLine ("Имя: ");
			string name = Console.ReadLine ();
			Console.WriteLine ("Продавец: ");
			string seller = Console.ReadLine ();
			int i = IntInput("Цена: ");

			storage.Add ( new Product(name, seller, i) );
		}

		static void SortProductMenu()
		{
			Console.WriteLine ("Выберите сортировку: ");
			Console.WriteLine ("1) По имени");
			Console.WriteLine ("2) По продавцу");
			Console.WriteLine ("3) По цене");

			bool valid = false;
			do {
				switch (GetKey ()) {
				case '1':
					storage.Sort (new Product.NameComparer ());
					storage.Print ();
					return;
				case '2':
					storage.Sort (new Product.SellerComparer ());
					storage.Print ();
					return;
				case '3':
					storage.Sort (new Product.ValueComparer ());
					storage.Print ();
					return;
				}
			} while(!valid);
		}

		public static void Main (string[] args)
		{
			MainMenu ();
		}
	}
}
