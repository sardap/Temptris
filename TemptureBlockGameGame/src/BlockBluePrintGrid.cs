using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TemptureBlockGameGame.src
{
	class BlockBluePrintGrid: IBlockBluePrint
	{
		private enum CellTypes
		{
			filled, Empty
		}

		private CellTypes[][] _cellTypes = new CellTypes[100][];

		public Dictionary<Directions, IBlockBluePrint> Blocks
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public Color? Color
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public void Add(Directions directions, IBlockBluePrint value)
		{
			throw new NotImplementedException();
		}

		public int MaxWidth()
		{
			throw new NotImplementedException();
		}

		public int MaxHeight()
		{
			throw new NotImplementedException();
		}

		public IBlockBluePrint GetBottom()
		{
			throw new NotImplementedException();
		}

		public IBlockBluePrint GetTop()
		{
			throw new NotImplementedException();
		}
	}
}
