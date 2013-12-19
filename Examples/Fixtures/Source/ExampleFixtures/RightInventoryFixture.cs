using System;

namespace fit.gui.examples
{
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
}