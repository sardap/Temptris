using CoordinateSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	public enum CellTypes
	{
		Empty, Block
	}


	class Grid
	{
		private class Block
		{
			public int X { get; set; }
			public int Y { get; set; }

			public Block(int x, int y)
			{
				X = x;
				Y = y;
			}

			public bool SamePos(Block other)
			{
				return other.X == X && other.Y == Y;
			}

			public override string ToString()
			{
				return string.Format("X:{0}, Y:{1}", X, Y);
			}

			public static Block operator +(Block a, Block b)
			{
				return new Block(a.X + b.X, a.Y + b.Y);
			}
		}

		private class BlockSet
		{
			public List<Block> Blocks { get; set; }

			public Color Color { get; set; }

			public bool Contoled { get; set; }

			public BlockSet()
			{
				Blocks = new List<Block>();

				var bytes = new byte[3];
				_random.NextBytes(bytes);

				Color = new Color(bytes[0], bytes[1], bytes[2], (byte)255);
			}

			public CellTypes[][] ToGrid()
			{
				var minWidth = Blocks.Min(i => i.X);
				var maxWidth = Blocks.Max(i => i.X);
				var minHeight = Blocks.Min(i => i.Y);
				var maxHeight = Blocks.Max(i => i.Y);

				var height = maxHeight - minHeight;
				var width = maxWidth - minWidth;
				var length = Math.Max(height, width) + 1;

				var result = new CellTypes[length][];

				for (int y = 0; y < length; y++)
				{
					result[y] = new CellTypes[length];

					for (int x = 0; x < length; x++)
					{
						result[y][x] = CellTypes.Empty;
					}
				}

				foreach (var block in Blocks)
				{
					result[block.Y - minHeight][block.X - minWidth] = CellTypes.Block;
				}

				return result;
			}

			public void Rotate90D(Grid grid)
			{
				var maxtrix = ToGrid();

				int n = maxtrix.Length;

				for (int x = 0; x < n / 2; x++)
				{
					// Consider elements in group of 4 in  
					// current square 
					for (int y = x; y < n - x - 1; y++)
					{
						// store current cell in temp variable 
						var temp = maxtrix[x][y];

						// move values from right to top 
						maxtrix[x][y] = maxtrix[y][n - 1 - x];

						// move values from bottom to right 
						maxtrix[y][n - 1 - x] = maxtrix[n - 1 - x][n - 1 - y];

						// move values from left to bottom 
						maxtrix[n - 1 - x][n - 1 - y] = maxtrix[n - 1 - y][x];

						// assign temp to left 
						maxtrix[n - 1 - y][x] = temp;
					}
				}

				var minWidth = Blocks.Min(i => i.X);
				var minHeight = Blocks.Min(i => i.Y);

				var updateBlocks = new List<Block>();

				for (int y = 0; y < maxtrix.Length; y++)
				{
					for (int x = 0; x < maxtrix.Length; x++)
					{
						if (maxtrix[y][x] == CellTypes.Block)
						{
							var newBlock = new Block(x + minWidth, y + minHeight);

							if (grid.VaildMove(newBlock))
							{
								updateBlocks.Add(newBlock);
							}
							else
							{
								return;
							}
						}
					}
				}

				Blocks = updateBlocks;
			}
		}

		private class RowConsoldator
		{
			public Stack<int> RowsToClear = new Stack<int>();

			public void Consoldate(Grid grid)
			{
				while (RowsToClear.Count > 0)
					MoveGridDownOnce(RowsToClear.Pop(), grid);
			}

			private void MoveGridDownOnce(int startRow, Grid grid)
			{
				for (int y = startRow; y > 0; y--)
				{
					for (int x = 0; x < Const.GRID_WIDTH; x++)
					{
						var temp = grid._cells[y - 1][x];
						grid._cells[y - 1][x] = grid._cells[y][x];
						grid._cells[y][x] = temp;
					}

					Console.WriteLine("ROW {0}", y);
					Console.WriteLine(grid);
				}
			}
		}

		private class LightingBolt
		{
			public int Col { get; set; }

			public bool Striked { get; set; }

			public void Draw(SpriteBatch spriteBatch)
			{
				var rectangle = new RectangleF(Col * Const.CELL_SIZE, 0, Const.CELL_SIZE, Const.GRID_HEIGHT * Const.CELL_SIZE);
				ShapeExtensions.FillRectangle(spriteBatch, rectangle, Color.Yellow);
			}

			public void Apply(Grid grid)
			{
				grid.ClearCol(Col);
			}
		}

		private static Random _random = new Random();

		private CellTypes[][] _cells = new CellTypes[Const.GRID_HEIGHT][];
		private List<BlockSet> _blocks = new List<BlockSet>();
		private Queue<BlockSet> _freezeQeue = new Queue<BlockSet>();
		private RowConsoldator _rowConsoldator = new RowConsoldator();
		private GridColorEntry _colors = null;
		private LightingBolt _lightingBolt = null;
		private bool _failure = false;

		public Score Score { get; set; }

		public bool Failure
		{
			get
			{
				return _failure;
			}
		}

		public CellTypes[][] Cells
		{
			get
			{
				return _cells;
			}
		}

		public double[] DataRepresentation
		{
			get
			{
				var result = new List<double>();

				foreach (var row in _cells)
				{
					foreach(var cell in row)
					{
						result.Add(cell == CellTypes.Block ? 2 : 1);
					}
				}

				foreach(var blockset in _blocks)
				{
					foreach(var block in blockset.Blocks)
					{
						result[block.Y * Const.GRID_WIDTH + block.X] = 3;
					}
				}

				return result.ToArray();
			}
		}

		public Grid()
		{
			for (int y = 0; y < Const.GRID_HEIGHT; y++)
			{
				_cells[y] = new CellTypes[Const.GRID_WIDTH];
				for (int x = 0; x < Const.GRID_WIDTH; x++)
				{
					_cells[y][x] = CellTypes.Empty;
				}
			}
		}


		public override string ToString()
		{

			char[][] charGrid = new char[Const.GRID_HEIGHT][];

			for(int y = 0; y < Const.GRID_HEIGHT; y++)
			{
				charGrid[y] = new char[Const.GRID_WIDTH];

				for(int x = 0; x < Const.GRID_WIDTH; x++)
				{
					charGrid[y][x] = _cells[y][x] == CellTypes.Empty ? 'O' : 'X';
				}

			}

			foreach(var blockSet in _blocks)
			{
				foreach(var block in blockSet.Blocks)
				{
					charGrid[block.Y][block.X] = 'H';
				}
			}

			string result = "";

			for (int y = 0; y < Const.GRID_HEIGHT; y++)
			{
				result += string.Format("{0,2}: ", y);

				for (int x = 0; x < Const.GRID_WIDTH; x++)
				{
					result += charGrid[y][x];
				}

				result += "\n";
			}


			return result;
		}

		public void Step()
		{
			_blocks.ForEach(i => UpdateBlocks(i));

			_rowConsoldator.Consoldate(this);

			if(_lightingBolt != null && _lightingBolt.Striked)
			{
				_lightingBolt.Apply(this);
				_lightingBolt = null;
			}

			var highestRowCleared = int.MinValue;

			while (_freezeQeue.Count > 0)
			{
				var blocks = _freezeQeue.Dequeue().Blocks;

				var row = blocks.Max(i => i.Y);
				highestRowCleared = Math.Max(row, highestRowCleared);

				FreezeBlock(blocks);

				while (row > 0)
				{
					if(UpdateRow(row))
					{
						_rowConsoldator.RowsToClear.Push(row);
					}

					row--;
				}
			}

			_blocks.RemoveAll(i => i.Blocks.Count == 0);

			if(_lightingBolt != null)
				_lightingBolt.Striked = true;
		}

		public void Insert(List<IBlockBluePrint> blockBlueprint)
		{
			blockBlueprint.ForEach(i => Insert(i, false));
		}


		public void Insert(IBlockBluePrint blockBlueprint, bool controled)
		{
			var bottom = blockBlueprint.GetBottom();
			var height = blockBlueprint.MaxHeight();
			var visted = new HashSet<BlockBlueprint>();

			var maxWidth = bottom.MaxWidth();

			int startX = controled ? (Const.GRID_WIDTH / 2) - maxWidth : _random.Next(maxWidth, Const.GRID_WIDTH - maxWidth);

			var toAdd = new BlockSet() { Contoled = controled };
			
			if(blockBlueprint.Color != null)
			{
				toAdd.Color = (Color)blockBlueprint.Color;
			}
			else
			{
				toAdd.Color = Utils.RandomElement(Const.BLOCK_COLORS);
			}

			Create(startX, height, bottom, toAdd.Blocks, new HashSet<IBlockBluePrint>());

			_blocks.Add(toAdd);

			CheckFailure(toAdd);
		}

		public int ActiveBlocks()
		{
			return _blocks.Where(i => i.Contoled).Count();
		}

		private void Create(int startX, int startY, IBlockBluePrint blockBlueprint, List<Block> blocks, HashSet<IBlockBluePrint> visted)
		{
			if (visted.Contains(blockBlueprint))
				return;

			blocks.Add(new Block(startX, startY));
			visted.Add(blockBlueprint);

			var nextX = startX;
			var nextY = startY;

			if(blockBlueprint.Blocks.ContainsKey(Directions.Up))
			{
				Create(nextX, nextY - 1, blockBlueprint.Blocks[Directions.Up], blocks, visted);
			}

			if(blockBlueprint.Blocks.ContainsKey(Directions.Down))
			{
				Create(nextX, nextY + 1, blockBlueprint.Blocks[Directions.Down], blocks, visted);
			}

			if (blockBlueprint.Blocks.ContainsKey(Directions.Right))
			{
				Create(nextX - 1, nextY, blockBlueprint.Blocks[Directions.Right], blocks, visted);
			}

			if (blockBlueprint.Blocks.ContainsKey(Directions.Left))
			{
				Create(nextX + 1, nextY, blockBlueprint.Blocks[Directions.Left], blocks, visted);
			}
		}

		public void ApplyWind(double speed, Directions direction)
		{
			var colsToMove = speed / Const.HIGH_WIND_BOUNDRY_METERS;
			var x = Const.GRID_WIDTH - 1;

			for (int i = 0; i < colsToMove; i++)
			{
				for (int y = 0; y < Const.GRID_HEIGHT; y++)
				{
					PushCellsDirection(direction, new Block(x - i, y));
				}
			}
		}

		public void ApplyAmbientWeatherEffects(DateTime time, IWeatherInfo weatherInfo, SelectedPlaceInfo selectedPlaceInfo)
		{
			if(!weatherInfo.ClearSky())
			{
				_colors = new GridColorEntry(Color.DarkGray, Color.Red);
			}
			else
			{
				var colorsTable = new CompraerTable<int, GridColorEntry>() { CompareValue = 1, CompareOpator = new CompraerTable<int, GridColorEntry>.LessThan() };

				Celestial cel = Celestial.CalculateCelestialTimes
				(
					selectedPlaceInfo.Location.Lat, 
					selectedPlaceInfo.Location.Lon, 
					time
				);

				var sunRiseHour = TimeZoneInfo.ConvertTimeFromUtc(cel.SunRise.Value, TimeZoneInfo.Local).Hour;
				var sunSetHour = TimeZoneInfo.ConvertTimeFromUtc(cel.SunSet.Value, TimeZoneInfo.Local).Hour;

				colorsTable.Add(0, new GridColorEntry(ExtraColors.NIGHT_RIDER, Color.Red));
				colorsTable.Add(sunRiseHour,  new GridColorEntry(Color.LightSkyBlue, Color.Red));
				colorsTable.Add(sunSetHour, new GridColorEntry(ExtraColors.NIGHT_RIDER, Color.Red));
				colorsTable.Add(24, new GridColorEntry(ExtraColors.NIGHT_RIDER, Color.Red));

				var colors = colorsTable.GetFirstAndPrevous(time.Hour);

				if (colors.Count < 2)
				{
					colors.Clear();
					colors.Add(colorsTable.GetAt(0));
					colors.Add(colorsTable.GetAt(3));
					colors.RemoveAt(0);
				}

				var perctangeTillNext = (int)(((double)time.Hour / (double)colors[1].Key) * 100);

				_colors = new GridColorEntry
				(
					ColorUtils.GetBlendedColor(colors[0].Value.EmptyColor, colors[1].Value.EmptyColor, perctangeTillNext),
					ColorUtils.GetBlendedColor(colors[0].Value.BlockColor, colors[1].Value.BlockColor, perctangeTillNext)
				);
			}
		}

		private void PushCellsDirection(Directions direction, Block block)
		{
			var nextBlock = block + DirectionToBlock(direction);

			if (!VaildIndex(nextBlock))
			{
				return;
			}

			if (!CellEmpty(block))
			{
				PushCellsDirection(direction, nextBlock);
			}

			if (CellEmpty(nextBlock))
			{
				if(!CellEmpty(block))
				{
					_cells[nextBlock.Y][nextBlock.X] = CellTypes.Block;
					_cells[block.Y][block.X] = CellTypes.Empty;
				}
				return;
			}
		}

		private Block DirectionToBlock(Directions direction)
		{
			switch(direction)
			{
				case Directions.Down:
					return new Block(0, 1);
				case Directions.Up:
					return new Block(0, -1);
				case Directions.Left:
					return new Block(-1, 0);
				case Directions.Right:
					return new Block(1, 0);
			}

			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
		{
			var cellSize = Const.CELL_SIZE;
			var scaleX = (int)Math.Ceiling((float)bounds.Width / (Const.GRID_WIDTH));
			var scaleY = (int)Math.Ceiling((float)bounds.Height / (Const.GRID_HEIGHT));
			var lineThickness = 1;
			var scaleMatrix = Matrix.CreateScale(scaleX, scaleY, 1.0f);

			for (int y = 0; y < Const.GRID_HEIGHT; y++)
			{
				for(int x = 0; x < Const.GRID_WIDTH; x++)
				{
					Rectangle cell = new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize);

					Color color;

					switch (_cells[y][x])
					{
						case CellTypes.Block:
							color = _colors.BlockColor;
							break;

						case CellTypes.Empty:
							color = _colors.EmptyColor;
							break;

						default:
							throw new NotImplementedException();
					}

					ShapeExtensions.FillRectangle(spriteBatch, cell, color);

					ShapeExtensions.DrawRectangle(spriteBatch, cell, new Color(2, 2, 2, 255), lineThickness);
				}
			}

			foreach(var blockSet in _blocks)
			{
				foreach(var block in blockSet.Blocks)
				{
					var x = block.X;
					var y = block.Y;

					Rectangle cell = new Rectangle(x * cellSize, y * cellSize, cellSize, cellSize);

					ShapeExtensions.FillRectangle(spriteBatch, cell, blockSet.Color);

					ShapeExtensions.DrawRectangle(spriteBatch, cell, new Color(0, 0, 255, 255), lineThickness);

				}
			}

			if(_lightingBolt != null)
				_lightingBolt.Draw(spriteBatch);
		}

		public void MoveActiveBlocks(int xOffset, int yOffset)
		{
			var offsetBlock = new Block(xOffset, yOffset);

			foreach(var blockSet in _blocks.Where(i => i.Contoled))
			{
				if(blockSet.Blocks.All(i => VaildMove(i + offsetBlock)))
				{
					for (int i = 0; i < blockSet.Blocks.Count; i++)
					{
						blockSet.Blocks[i] += offsetBlock;
					}
				}
			}
		}

		public void RotateActive()
		{
			foreach(var blockSet in _blocks.Where(i => i.Contoled))
			{
				blockSet.Rotate90D(this);
			}
		}

		public void ApplyLightingBolt()
		{
			_lightingBolt = new LightingBolt() { Striked = false, Col = _random.Next(0, Const.GRID_WIDTH) };
		}

		private bool VaildMove(Block block)
		{
			return VaildIndex(block) && CellEmpty(block);
		}

		private bool CellEmpty(Block block)
		{
			return _cells[block.Y][block.X] == CellTypes.Empty;
		}

		private bool VaildIndex(Block block)
		{
			return VaildIndex(block.X, block.Y);
		}

		private bool VaildIndex(int x, int y)
		{
			return y >= 0 && x >= 0 && y < Const.GRID_HEIGHT && x < Const.GRID_WIDTH;
		}

		private bool AnyBlockAtPostion(List<Block> blocks, int x, int y)
		{
			return blocks.Any(i => i.SamePos(new Block(x, y)));
		}

		private void UpdateBlocks(BlockSet blockSet)
		{
			var blocks = blockSet.Blocks;

			var bottomHit = blocks.Any(i => i.Y + 1 == Const.GRID_HEIGHT);

			var placedBlockHit = !bottomHit && 
				blocks.Any
				(
					j => _cells[j.Y + 1][j.X] == CellTypes.Block
				);

			if (bottomHit || placedBlockHit)
			{
				_freezeQeue.Enqueue(blockSet);
				return;
			}

			foreach(var block in blocks)
			{
				block.Y++;
			}
		}

		private void FreezeBlock(List<Block> blocks)
		{
			foreach(var block in blocks)
			{
				_cells[block.Y][block.X] = CellTypes.Block;
			}

			blocks.Clear();
		}

		/// <summary>
		/// Returns if the row was cleared
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		private bool UpdateRow(int row)
		{
			if (_cells[row].All(i => i == CellTypes.Block))
			{
				Score.Value += 1000;

				for (int x = 0; x < Const.GRID_WIDTH; x++)
				{
					_cells[row][x] = CellTypes.Empty;
				}

				return true;
			}

			return false;
		}

		private void Fall(int startingRow)
		{
			for(int y = startingRow; y > 0; y--)
			{
				for(int x = 0; x < Const.GRID_WIDTH; x++)
				{
					if(_cells[y][x] == CellTypes.Block)
					{
						MoveCellDown(x, y);
					}
				}
			}
		}

		private void MoveCellDown(int x, int y)
		{
			if (y + 1 >= Const.GRID_HEIGHT)
				return;

			if (_cells[y + 1][x] == CellTypes.Block)
				return;

			_cells[y + 1][x] = CellTypes.Block;
			_cells[y][x] = CellTypes.Empty;

			MoveCellDown(x, y + 1);
		}

		private void ClearCol(int col)
		{
			for(int y = 0; y < Const.GRID_HEIGHT; y++)
			{
				_cells[y][col] = CellTypes.Empty;
			}
		}

		private void CheckFailure(BlockSet newBlock)
		{
			var blocks = new Stack<Block>();

			for(int y = 0; y < Const.GRID_HEIGHT / 2; y++)
			{
				for(int x = 0; x < Const.GRID_WIDTH; x++)
				{
					if(_cells[y][x] == CellTypes.Block)
					{
						blocks.Push(new Block(x, y));
					}
				}
			}

			while(blocks.Count > 0)
			{
				var current = blocks.Pop();

				if (newBlock.Blocks.Any(i => i.SamePos(current)))
				{
					_failure = true;
					return;
				}
			}
		}
	}
}
