using Newtonsoft.Json;

namespace Root
{
	public class JsonDotNetModule : IJsonService
	{
		public string ToJson(object obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public T FromJson<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}
