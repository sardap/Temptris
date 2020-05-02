using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	static class ColorUtils
	{
		public static Color GetBlendedColor(Color a, Color b, int percentage)
		{
			if (percentage < 50)
				return Interpolate(a, b, percentage / 50.0);

			return Interpolate(a, b, (percentage - 50) / 50.0);
		}

		private static Color Interpolate(Color color1, Color color2, double fraction)
		{
			double r = Interpolate(color1.R, color2.R, fraction);
			double g = Interpolate(color1.G, color2.G, fraction);
			double b = Interpolate(color1.B, color2.B, fraction);

			return new Color((int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));
		}

		private static double Interpolate(double d1, double d2, double fraction)
		{
			return d1 + (d2 - d1) * fraction;
		}

	}
}
