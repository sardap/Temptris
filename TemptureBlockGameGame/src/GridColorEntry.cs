using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class GridColorEntry
	{
		public Color EmptyColor { get; set; }
		public Color BlockColor { get; set; }

		public GridColorEntry(Color empty, Color block)
		{
			EmptyColor = empty;
			BlockColor = block;
		}

		public GridColorEntry()
		{
		}
	}
}
