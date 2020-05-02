using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class OpenWeatherInfo: IWeatherInfo
	{
		const int LIGHT = 2;
		const int MEDIUM = LIGHT * 2;
		const int HEAVY = MEDIUM * 4;

		private Dictionary<int, double> _rainAmmountTable = new Dictionary<int, double>();
		private Dictionary<int, double> _drizleAmmountTable = new Dictionary<int, double>();
		private Dictionary<int, double> _snowAmmounts = new Dictionary<int, double>();
		private Dictionary<int, double> _thunderStomTable = new Dictionary<int, double>();
		private List<int> _weatherIds = new List<int>();

		private dynamic _jsonWeatherInfo;

		public string CityID { get; set; }

		public double AmountOfDrizzle()
		{
			return GetMaxForIdsInTable(_drizleAmmountTable);
		}

		public double AmountOfRain()
		{
			return GetMaxForIdsInTable(_rainAmmountTable);
		}

		public double AmountOfSnow()
		{
			return GetMaxForIdsInTable(_snowAmmounts);
		}

		private double GetMaxForIdsInTable(Dictionary<int, double> table)
		{
			double max = 0;

			foreach (var nextId in _weatherIds)
			{
				if (table.ContainsKey(nextId) && table[nextId] > max)
				{
					max = table[nextId];
				}
			}

			return max;
		}

		public double CurrentTemp(TemptureMessurements temptureMessurement)
		{
			return TemptureConverter.ConvertKelvinTo(temptureMessurement, _jsonWeatherInfo["main"]["temp"]);
		}

		public void Initlise()
		{
			string url = 
				"http://api.openweathermap.org/data/2.5/weather?id=" + CityID + 
				"&APPID=" + APIKeyManger.GetInstance().GetApiKey("OpenWeather");

			var request = WebRequest.Create(url);
			request.Credentials = CredentialCache.DefaultCredentials;
			var response = request.GetResponse();
			var dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			// Read the content.  
			string json = reader.ReadToEnd();
			dynamic temperatureData = JsonConvert.DeserializeObject(json);
			// Display the content.  
			response.Close();
			reader.Close();

			_jsonWeatherInfo = temperatureData;

			foreach (var weather in _jsonWeatherInfo.weather)
			{
				int nextId = weather.id;
				_weatherIds.Add(nextId);
			}


			PopluateTable
			(
				_rainAmmountTable,
				new List<int>() { 200, 500, 520, 615 },
				new List<int>() { 201, 313, 501, 521, 616 },
				new List<int>() { 202, 314, 502, 503, 504, 522, 531 }
			);

			PopluateTable
			(
				_drizleAmmountTable,
				new List<int>() { 310 },
				new List<int>() { 311 },
				new List<int>() { 312 }
			);

			PopluateTable
			(
				_snowAmmounts,
				new List<int>() { 600, 615 },
				new List<int>() { 601, 616, 620 },
				new List<int>() { 602, 621, 622 }
			);

			PopluateTable
			(
				_thunderStomTable,
				new List<int>() { 210 },
				new List<int>() { 200, 201, 202, 211, 230, 231 },
				new List<int>() { 212, 221, 232 }
			);
		}

		public double AmountOfWeather()
		{
			return AmountOfRain() + AmountOfDrizzle() + AmountOfSnow();
		}

		// ID Info https://openweathermap.org/weather-conditions
		private void PopluateTable(Dictionary<int, double> toFill, List<int> lightIDs, List<int> medIDs, List<int> heavyIDs)
		{
			void AddElements(IEnumerable<int> toAdd, int value)
			{
				foreach (var i in toAdd)
				{
					toFill.Add(i, value);
				}
			}

			AddElements(lightIDs, LIGHT);
			AddElements(medIDs, MEDIUM);
			AddElements(heavyIDs, HEAVY);
		}

		public bool ClearSky()
		{
			return _weatherIds.Any(i => i == 800) || (AmountOfWeather() <= 0 && AmountOfThunderstrom() <= 0);
		}

		public double WindLevel()
		{
			return _jsonWeatherInfo.wind.speed;
		}

		public Location GetLocation()
		{
			var lat = _jsonWeatherInfo.coord.lat.Value;
			var lon = _jsonWeatherInfo.coord.lon.Value;

			return new Location(lat, lon);
		}

		public double AmountOfThunderstrom()
		{
			return GetMaxForIdsInTable(_thunderStomTable);
		}
	}
}
