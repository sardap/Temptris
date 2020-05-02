using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	static class TemptureConverter
	{
		public static double ConvertKelvinTo(TemptureMessurements temptureMessurement, double value)
		{
			var result = value;

			switch(temptureMessurement)
			{
				case TemptureMessurements.Ceilius:
					result -= 273.15;
					break;
				case TemptureMessurements.Pauls:
					result %= 26;
					break;
			}

			return result;
		}
	}
}
