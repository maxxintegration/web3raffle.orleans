using Slugify;

namespace Web3raffle.Utilities.Extensions
{
	public static class StringExtensions
	{
		public static string ToUrlSlug(this string title, SlugHelperConfiguration? slugOptions = null)
		{
			var helper = slugOptions is null
				? new SlugHelper()
				: new SlugHelper(slugOptions);

			return helper
				.GenerateSlug(title);
		}

		public static string? MaskString(this string? str, bool isMask, string? mask = "********")
		{
			return isMask ? mask : str;
		}
	}
}