using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TemptureBlockGameGame.src
{
	class BlockBlueprint : IBlockBluePrint
	{
		public Dictionary<Directions, IBlockBluePrint> Blocks { get; set; }

		public Color? Color { get; set;}

		public BlockBlueprint()
		{
			Blocks = new Dictionary<Directions, IBlockBluePrint>();
		}

		public BlockBlueprint(Color color): this()
		{
			Color = color;
		}

		public int MaxWidth()
		{
			return FindDirection(Directions.Right).Steps;
		}

		public int MaxHeight()
		{
			var result = 0;

			var current = GetBottom().Blocks;

			while (current.ContainsKey(Directions.Up))
			{
				current = current[Directions.Up].Blocks;
				result++;
			}

			return result;
		}

		public IBlockBluePrint GetBottom()
		{
			return GetDirection(Directions.Down);
		}

		public IBlockBluePrint GetTop()
		{
			return GetDirection(Directions.Up);
		}

		private class BestPair
		{
			public int Steps = int.MinValue;
			public BlockBlueprint BlockBlueprint;
		}

		private BlockBlueprint GetDirection(Directions directionToGet)
		{
			return FindDirection(directionToGet).BlockBlueprint;
		}

		private BestPair FindDirection(Directions directionToGet)
		{
			return FindDirection(directionToGet, 1, new HashSet<BlockBlueprint>(), new BestPair() { Steps = 1, BlockBlueprint = this });
		}

		private BestPair FindDirection(Directions directionToGet, int steps, HashSet<BlockBlueprint> visted, BestPair best)
		{
			visted.Add(this);

			foreach (var i in typeof(Directions).GetEnumValues())
			{
				var direction = (Directions)i;

				if (Blocks.ContainsKey(direction))
				{
					var next = (BlockBlueprint)Blocks[direction];

					if (direction == directionToGet)
					{
						if (steps > best.Steps)
						{
							best.Steps = steps;
							best.BlockBlueprint = next;
						}
					}

					if (!visted.Contains(next))
					{
						var hashSetCopy = new HashSet<BlockBlueprint>(visted.ToList());

						next.FindDirection(directionToGet, steps + 1, hashSetCopy, best);
					}
				}
			}

			return best;
		}

		public void Add(Directions direction, IBlockBluePrint value)
		{
			Blocks.Add(direction, value);

			var invertedDirection = InvertDirection(direction);

			if (!value.Blocks.ContainsKey(invertedDirection))
				value.Blocks.Add(invertedDirection, this);
		}

		private Directions InvertDirection(Directions direction)
		{
			switch(direction)
			{
				case Directions.Down:
					return Directions.Up;

				case Directions.Up:
					return Directions.Down;

				case Directions.Left:
					return Directions.Right;

				case Directions.Right:
					return Directions.Left;

				default:
					throw new NotImplementedException();
			}
		}
	}
}