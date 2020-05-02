using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace TemptureBlockGameGame.src
{
	public class PaulFileUtils
	{
		public static string ReadEntireFile(string fileName)
		{
			using (var streamReader = new StreamReader(fileName))
			{
				return streamReader.ReadToEnd();
			}
		}

		public static void WriteToJsonFile<T>(T toWrite, string fileName)
		{
			using (var streamWriter = new StreamWriter(fileName))
			{
				streamWriter.Write(JsonConvert.SerializeObject(toWrite));
			}
		}

		public static T DeserializeObjectFromFile<T>(string fileName)
		{
			return JsonConvert.DeserializeObject<T>(ReadEntireFile(fileName));
		}

		public static JObject GetJobjectOfFile(string fileName)
		{
			return new JObject(ReadEntireFile(fileName));
		}
		

	}
}
