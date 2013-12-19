using System;

namespace fit.gui.examples
{
	public class Article
	{
		public int Id;
		public string Name;
		private float _price;

		public float Price()
		{
			if (_price < 0)
				throw new Exception("Negative price!");

			return _price;
		}

		public Article(int id, string name, float price)
		{
			Id = id;
			Name = name;
			_price = price;
		}
	}
}

