using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	public static class Const
	{
		public const int WINDOW_HEIGHT = 1080;
		public const int WINDOW_WIDTH = 1920;

		public const int GRID_SCALE = 1;
		public const int CELL_SIZE = 45 / GRID_SCALE;
		public const int GRID_HEIGHT = 24 * GRID_SCALE;
		public const int GRID_WIDTH = 10 * GRID_SCALE;
		public static readonly Color BLOCK_COLOR = new Color(255, 0, 0);
		public static readonly Color EMPTY_COLOR = new Color(225, 225, 225);


		public static double TIME_SCLAE = 1;

		public static double TIME_BETWEEN_GRID_UPDATES_START
		{
			get
			{
				return 0.25 / TIME_SCLAE;
			}
		}

		public static double TIME_BETWEEN_GRID_UPDATES_MAX
		{
			get
			{
				return	0.005 / TIME_SCLAE;
			}
		}

		public static double MOVE_ACTIVE_BLOCK_COOLDOWN
		{
			get
			{
				return 0.05 / TIME_SCLAE;
			}
		}

		public static double SOCRE_INCREMENT
		{
			get
			{
				return 0.5 / TIME_SCLAE;
			}
		}

		public const double HIGH_WIND_BOUNDRY_METERS = 10;

		public const int WIND_TRIGGER_MIN = 15;
		public const int WIND_TRIGGER_MAX = 50;

		public const int THUNDER_TRIGGER = 10;


		public static readonly List<Color> BLOCK_COLORS = new List<Color>()
		{
			Color.Orange
		};
	}
}
