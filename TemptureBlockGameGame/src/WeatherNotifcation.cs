using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class WeatherNotifcation: IWeatherNotifcation
	{
		public double MessageShownFor { get; set; }

		public Color FontColor { get; set; }

		public string Message { get; set; }

		public float Scale { get; set; }
	}
}
