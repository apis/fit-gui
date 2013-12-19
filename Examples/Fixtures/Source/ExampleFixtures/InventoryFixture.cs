using System;

namespace fit.gui.examples
{
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
}