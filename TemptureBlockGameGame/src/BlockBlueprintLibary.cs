using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class BlockBlueprintLibary
	{
		private static Random _random = new Random();

		private static BlockBlueprintLibary _singleton = null;

		public static BlockBlueprintLibary GetInstance()
		{
			if (_singleton == null)
			{
				_singleton = new BlockBlueprintLibary();
			}

			return _singleton;
		}

		private OddsTable<double, IBlockBluePrint> _oddsNorimeTable = new OddsTable<double, IBlockBluePrint>();
		private OddsTable<double, Tuple<IBlockBluePrint, int>> _oddsWeatherTable = new OddsTable<double, Tuple<IBlockBluePrint, int>>();

		private BlockBlueprintLibary()
		{
			_oddsNorimeTable.Add(5, StraightFourLine());
			_oddsNorimeTable.Add(5, TBlock());
			_oddsNorimeTable.Add(5, Sqaure());
			_oddsNorimeTable.Add(5, ZigZag());
			_oddsNorimeTable.Add(5, ZagZig());
			_oddsNorimeTable.Add(5, LBlock());
			_oddsNorimeTable.Add(5, BackwardsLBlock());
		}

		public void UpdateOddsTableWithWeatherData(IWeatherInfo weatherInfo)
		{
			_oddsWeatherTable.Add(weatherInfo.AmountOfRain(), new Tuple<IBlockBluePrint, int>(RainDrop(), 1));
			_oddsWeatherTable.Add(weatherInfo.AmountOfDrizzle(), new Tuple<IBlockBluePrint, int>(Drizzle(), 5));
			_oddsWeatherTable.Add(weatherInfo.AmountOfSnow(), new Tuple<IBlockBluePrint, int>(Snow(), 10));
		}

		public IBlockBluePrint PureRandomBlock()
		{
			var CreateMethods = typeof(BlockBlueprintLibary).GetMethods().ToList();
			CreateMethods.RemoveAll(i => i.Name == "RandomBlock");
			CreateMethods.RemoveAll(i => i.GetParameters().Count() > 0);
			CreateMethods.RemoveAll(i => i.ReturnType != typeof(IBlockBluePrint));

			return (IBlockBluePrint)CreateMethods[_random.Next(CreateMethods.Count)].Invoke(this, new object[] { });
		}

		public IBlockBluePrint RandomNorimeBlock()
		{
			var randomNumber = _random.NextDouble() * (_oddsNorimeTable.Max() - 0) + 0;

			return _oddsNorimeTable.Get(randomNumber);
		}

		public List<IBlockBluePrint> WeatherRandomBlock()
		{
			var randomNumber = _random.NextDouble() * (_oddsWeatherTable.Max() - 0) + 0;

			var oddsTableResult = _oddsWeatherTable.Get(randomNumber);
			var result = new List<IBlockBluePrint>();

			var n = _random.Next(oddsTableResult.Item2) + 1;

			for(int i = 0; i < n; i++)
			{
				result.Add(oddsTableResult.Item1);
			}

			return result;
		}

		public IBlockBluePrint Snow()
		{
			return StraightLine(1, Color.White);
		}

		public IBlockBluePrint StraightFourLine()
		{
			return StraightLine(4);
		}

		public IBlockBluePrint TBlock()
		{
			return TBlock(2, 2);
		}

		public IBlockBluePrint Sqaure()
		{
			return Sqaure(2);
		}

		public IBlockBluePrint ZigZag()
		{
			return ZigZag(0);
		}

		public IBlockBluePrint ZagZig()
		{
			return ZagZig(0);
		}

		public IBlockBluePrint Drizzle()
		{
			return StraightLine(2, new Color(3, 74, 264));
		}

		public IBlockBluePrint LBlock()
		{
			IBlockBluePrint bottom = StraightLine(3);

			var next = new BlockBlueprint();
			bottom.Add(Directions.Right, next);

			return bottom;
		}

		public IBlockBluePrint BackwardsLBlock()
		{
			IBlockBluePrint bottom = StraightLine(3);

			var next = new BlockBlueprint();
			bottom.Add(Directions.Left, next);

			return bottom;
		}

		public IBlockBluePrint RainDrop()
		{
			var color = new Color(3, 74, 264);

			void AddRow(IBlockBluePrint start, int numToAdd)
			{
				var nestedCurrent = start;

				for (int i = 0; i < numToAdd - 1; i++)
				{
					IBlockBluePrint innerNext = new BlockBlueprint(color);
					nestedCurrent.Add(Directions.Right, innerNext);
					nestedCurrent = innerNext;
				}
			}

			IBlockBluePrint bottom = new BlockBlueprint(color);
			IBlockBluePrint next;
			var current = bottom;

			AddRow(current, 5);

			current = current.Blocks[Directions.Right];
			next = new BlockBlueprint(color);
			current.Add(Directions.Up, next);
			AddRow(next, 3);

			next = new BlockBlueprint(color);
			current.Add(Directions.Down, next);
			AddRow(next, 3);


			next = new BlockBlueprint(color);
			current = current.Blocks[Directions.Up];
			current = current.Blocks[Directions.Right];
			next = new BlockBlueprint(color);
			current.Add(Directions.Up, next);

			return bottom;
		}

		private IBlockBluePrint ZigZag(int zig)
		{
			var bottom = new BlockBlueprint();
			var current = bottom;

			var next1 = new BlockBlueprint();
			var next2 = new BlockBlueprint();
			var next3 = new BlockBlueprint();

			bottom.Add(Directions.Up, next1);
			next1.Add(Directions.Left, next2);
			next2.Add(Directions.Up, next3);

			return bottom;
		}

		private IBlockBluePrint ZagZig(int zig)
		{
			var bottom = new BlockBlueprint();
			var current = bottom;

			var next1 = new BlockBlueprint();
			var next2 = new BlockBlueprint();
			var next3 = new BlockBlueprint();

			bottom.Add(Directions.Up, next1);
			next1.Add(Directions.Right, next2);
			next2.Add(Directions.Up, next3);

			return bottom;
		}


		private IBlockBluePrint StraightLine(int height)
		{
			return StraightLine(height, Utils.RandomElement(Const.BLOCK_COLORS));
		}

		private IBlockBluePrint StraightLine(int height, Color color)
		{
			var bottom = new BlockBlueprint(color);
			var current = bottom;

			for (int i = 0; i < height - 1; i++)
			{
				var next = new BlockBlueprint(color);
				current.Add(Directions.Up, next);
				current = next;
			}

			return bottom;
		}

		private IBlockBluePrint TBlock(int height, int width)
		{
			var top = StraightLine(height);
			top = top.GetTop();

			var currentLeft = top;
			var currentRight = top;

			for(int i = 0; i < width / 2; i++)
			{
				void AddDirection(ref IBlockBluePrint current, Directions direction)
				{
					var next = new BlockBlueprint();
					current.Add(direction, next);
					current = next;
				}

				AddDirection(ref currentLeft, Directions.Left);
				AddDirection(ref currentRight, Directions.Right);
			}

			return top.GetBottom();
		}

		private IBlockBluePrint Sqaure(int size)
		{
			var topLeft = new BlockBlueprint();

			var current = topLeft;

			for(int i = 0; i < size; i++)
			{
				var farRight = current;

				for (int j = 0; j < size - 1; j++)
				{
					var next = new BlockBlueprint();
					farRight.Add(Directions.Right, next);
					farRight = next;
				}

				if(i < size - 1)
				{
					var next = new BlockBlueprint();
					current.Add(Directions.Down, next);
					current = next;
				}
			}

			return topLeft;
		}
	}
}
