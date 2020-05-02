﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	interface IWeatherNotifcation
	{
		double MessageShownFor { get; }

		Color FontColor { get; }

		string Message { get; }

		float Scale { get; }
	}
}
