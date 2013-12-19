using System;

namespace fit.gui.examples
{
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