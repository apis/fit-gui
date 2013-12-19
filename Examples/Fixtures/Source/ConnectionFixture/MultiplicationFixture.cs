using System;
using fit;

namespace fit.gui.examples
{
	public class MultiplicationFixture : ColumnFixture
	{
		public int multiplicand;
		public int multiplier;

		public int product()
		{
			return multiplicand * multiplier;
		}	
	}
}