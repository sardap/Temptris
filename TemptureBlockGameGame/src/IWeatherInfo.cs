using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	interface IWeatherInfo
	{
		void Initlise();

		double CurrentTemp(TemptureMessurements temptureMessurement);

		double AmountOfRain();

		double AmountOfDrizzle();

		double AmountOfSnow();

		double AmountOfWeather();

		bool ClearSky();

		double WindLevel();

		double AmountOfThunderstrom();
	}
}
