using System;
using fit;

namespace fit.gui.examples
{
	public class Article
	{
		public int Id;
		public string Name;
		private float price;

		public float Price()
		{
			return price;
		}

		public Article(int id, string name, float price)
		{
			Id = id;
			Name = name;
			this.price = price;
		}
	}

	public class InventoryFixture : RowFixture
	{
		private Article[] articles = new Article[]
		{ 
			new Article(1002, "Arm chair", 3000),
			new Article(1003, "Stool", 1000),
			new Article(1001, "Chair", 2000)
		};

		public override object[] query()
		{
			return articles;
		}

		public override Type getTargetClass()
		{
			return typeof(Article);
		}
	}
}

