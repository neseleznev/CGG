using System.Collections.Generic;

namespace Task5
{
	class Figure
	{
		public IList<Side> Sides { get; private set; }

		public Figure(IList<Side> sides)
		{
			Sides = sides;
		}
	}
}
