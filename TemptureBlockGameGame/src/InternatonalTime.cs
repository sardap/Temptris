using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemptureBlockGameGame.src
{
	class GoogleTimeZone
	{
		public double dstOffset { get; set; }
		public double rawOffset { get; set; }
		public string status { get; set; }
		public string timeZoneId { get; set; }
		public string timeZoneName { get; set; }
	}

	class InternatonalTime
	{
		public static async Task<DateTime> GetLocalDateTimeAsync(Location location, DateTime utcDate)
		{
			var client = new RestClient("https://maps.googleapis.com");
			var request = new RestRequest("maps/api/timezone/json", Method.GET);
			request.AddParameter("location", location.Lat + "," + location.Lon);
			request.AddParameter("timestamp", utcDate.ToTimestamp());
			request.AddParameter("sensor", "false");
			var response = await client.Execute<GoogleTimeZone>(request);

			return utcDate.AddSeconds(response.Data.rawOffset + response.Data.dstOffset);
		}

	}
}
