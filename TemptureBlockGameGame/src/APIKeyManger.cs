using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TemptureBlockGameGame.src
{
	public class APIKeyManger
	{
		private static APIKeyManger _singleton;

		private Dictionary<string, string> _apiKeys = new Dictionary<string, string>();

		private APIKeyManger() { }

		public static APIKeyManger GetInstance()
		{
			if (_singleton == null)
			{
				_singleton = new APIKeyManger();
			}

			return _singleton;
		}

		public void AddKey(string key, string apiKey)
		{
			_apiKeys.Add(key, apiKey);
		}

		public string GetApiKey(string key)
		{
			return _apiKeys[key];
		}

		public void Load(Dictionary<string, string> dict)
		{
			_apiKeys = dict;
		}

		public void Save(string fileName)
		{
			using (StreamWriter streamWriter = new StreamWriter(fileName))
			{
				var jsonString = JsonConvert.SerializeObject(this);
				streamWriter.Write(jsonString);
			}
		}
	}
}
