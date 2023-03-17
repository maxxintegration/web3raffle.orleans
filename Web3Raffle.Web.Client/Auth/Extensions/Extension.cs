using Slugify;
using System.Net;
using System.Text.RegularExpressions;

namespace Web3raffle.Web.Client.Auth.Extensions;

public static class Extension
{

	public static bool IsResponseSuccess(this HttpStatusCode statusCode)
	{
		int code = Convert.ToInt32(statusCode);
		return code >= 200 && code < 300;
	}

	public static string ReplaceAllSpecialCharacters(this string s, string replaceString)
	{
		string strRegex = @"[^A-Za-z0-9_-]";
		s = s ?? "";
		var regex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		return regex.Replace(s, replaceString);
	}

	public static string ReplaceAllRepeatedCharacters(this string s, char repeatChar, string replaceString)
	{
		string strRegex = "[" + repeatChar.ToString() + "]{2,}";

		s = s ?? "";
		replaceString = replaceString ?? "";
		var regex = new Regex(strRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
		return regex.Replace(s, replaceString);
	}

	//public static string ToUrlSlug(this string s)
	//{
	//	return string.IsNullOrEmpty(s) ? s : s.ReplaceAllSpecialCharacters("-").ReplaceAllRepeatedCharacters('-', "-").ReplaceAllRepeatedCharacters('_', "_").Trim('-').Trim('_').ToLower();
	//}


	public static string ToUrlSlug(this string title, SlugHelperConfiguration? slugOptions = null)
	{
		var helper = slugOptions is null
			? new SlugHelper()
			: new SlugHelper(slugOptions);

		return helper
			.GenerateSlug(title);
	}
}