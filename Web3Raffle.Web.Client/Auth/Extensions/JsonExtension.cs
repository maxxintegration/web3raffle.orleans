using Newtonsoft.Json;

namespace Web3raffle.Web.Client.Auth.Extensions;

public static class JsonExtension
{

	public static T JsonToObject<T>(this string json)
	{
		return JsonConvert.DeserializeObject<T>(json)!;
	}

	public static List<T> JsonToList<T>(this string json)
	{
		return JsonConvert.DeserializeObject<List<T>>(json)!;
	}


}