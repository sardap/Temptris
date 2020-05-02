using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class TestWeather: IWeatherInfo
	{
		const int LIGHT = 2;
		const int MEDIUM = LIGHT * 2;
		const int HEAVY = MEDIUM * 4;

		public double AmountOfDrizzle()
		{
			throw new NotImplementedException();
		}

		public double AmountOfRain()
		{
			throw new NotImplementedException();
		}

		public double AmountOfSnow()
		{
			throw new NotImplementedException();
		}

		public double AmountOfWeather()
		{
			throw new NotImplementedException();
		}

		public bool ClearSky()
		{
			throw new NotImplementedException();
		}

		public double CurrentTemp(TemptureMessurements temptureMessurement)
		{
			throw new NotImplementedException();
		}

		public void Initlise()
		{
			throw new NotImplementedException();
		}

		public double AmountOfThunderstrom()
		{
			throw new NotImplementedException();
		}

		public double WindLevel()
		{
			throw new NotImplementedException();
		}
	}
}
