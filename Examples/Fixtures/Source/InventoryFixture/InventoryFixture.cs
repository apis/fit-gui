using System;
using fit;

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

	public class InventoryFixture : RowFixture
	{
		private Article[] _articles;

		protected InventoryFixture(Article[] articles)
		{
			_articles = articles;
		}

		public override object[] query()
		{
			return _articles;
		}

		public override Type getTargetClass()
		{
			return typeof(Article);
		}
	}

	public class RightInventoryFixture : InventoryFixture
	{
		public RightInventoryFixture() : base(
			new Article[]
			{ 
				new Article(1002, "Arm chair", 3000),
				new Article(1003, "Stool", 1000),
				new Article(1001, "Chair", 2000)
			})
		{
		}
	}

	public class ExceptionInventoryFixture : InventoryFixture
	{
		public ExceptionInventoryFixture() : base(
			new Article[]
			{ 
				new Article(1002, "Arm chair", 3000),
				new Article(1003, "Stool", -1),
				new Article(1001, "Chair", 2000)
			})
		{
		}
	}

	public class WrongInventoryFixture : InventoryFixture
	{
		public WrongInventoryFixture() : base(
			new Article[]
			{ 
				new Article(1003, "Stool", 1005),
				new Article(1001, "Chair", 2000),
				new Article(1000, "Taboret", 500)
			})
		{
		}
	}
}

