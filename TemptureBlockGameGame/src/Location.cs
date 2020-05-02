using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class Location
	{
		public double Lat { get; set; }
		public double Lon { get; set; }

		public Location(double lat, double lon)
		{
			Lat = lat;
			Lon = lon;
		}
	}
}
