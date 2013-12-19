using System;

namespace fit.gui.examples
{
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
}