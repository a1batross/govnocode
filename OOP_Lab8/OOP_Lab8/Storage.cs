using System;
using System.Collections.Generic;

namespace OOP_Lab8
{
	public class Storage
	{
		public Storage () { }

		public int Add( Product product )
		{
			products.Add (product);
			return products.Count;
		}

		private List<Product> products = new List<Product>();

		public Product GetProductByIndex(int idx)
		{
			if( idx < 0 || idx > products.Count )
				throw new NotFoundException();
			return products [idx];
		}

		public Product GetProductByName(string name)
		{
			foreach (Product pr in products) {
				if (pr.Name.Equals (name)) {
					return pr;
				}
			}

			throw new NotFoundException();
		}

		public void Print()
		{
			int i = 0;
			foreach (Product pr in products) {
				Console.Write ("{0,3}|", i);
				pr.Print ();
				i++;
			}
		}

		public void Sort(IComparer<Product> comparer)
		{
			products.Sort (comparer);
		}

		public class NotFoundException : Exception { }
	}
}

