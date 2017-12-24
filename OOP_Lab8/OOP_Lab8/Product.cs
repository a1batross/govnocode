using System;
using System.Collections.Generic;

namespace OOP_Lab8
{
	public class Product
	{
		public Product ()
		{
		}
		public Product (string name, string provider, int value)
		{
			Name = name;
			Seller = provider;
			Value = value;
		}

		public string Name { get; set; }
		public string Seller { get; set; }
		public int Value { get; set; }

		public void Print()
		{
			Console.WriteLine ("{0,10} | {1,10} | {2,10}", Name, Seller, Value );
		}

		static public int operator+(Product a, Product b)
		{
			return a.Value + b.Value;
		}

		public class NameComparer : IComparer<Product>
		{
			public int Compare(Product x, Product y)
			{
				return String.Compare( x.Name, y.Name );
			}
		}

		public class SellerComparer : IComparer<Product>
		{
			public int Compare(Product x, Product y)
			{
				return String.Compare( x.Name, y.Name );
			}
		}

		public class ValueComparer : IComparer<Product>
		{
			public int Compare(Product x, Product y)
			{
				if (x.Value < y.Value)
					return -1;
				if (x.Value > y.Value)
					return 1;
				return 0;
			}
		}

	}
}

