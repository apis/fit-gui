using System;

namespace fit.gui.examples
{
	public class AdditionFixture : PrimitiveFixture
	{
		int _x = 0;
		int _y = 0;

		public override void doRows(Parse rows)
		{
			base.doRows(rows.more);
		}

		public override void doCell(Parse cell, int column)
		{
			switch (column)
			{
				case 0: 
					_x = (int)parseLong(cell); 
					break;

				case 1: 
					_y = (int)parseLong(cell); 
					break;

				case 2: 
					check(cell, _x + _y); 
					break;

				default: 
					ignore(cell); 
					break;
			}
		}
	}
}