using Web3raffle.Models.Responses;

namespace Web3raffle.Utilities.Extensions
{
	public static class ResponseModelExtensions
	{
		public static ResponseModel<T> ToResponseModel<T>(this T data) where T : BaseResponseDataModel => new()
		{
			Data = data,
		};

		public static ResponseCollectionModel<T> ToResponseModel<T>(this List<T> data) where T : BaseResponseDataModel => new()
		{
			Data = data
		};

		public static TTarget CopyObject<TSource, TTarget>(this TSource source, TTarget target) where TSource : class where TTarget : class
		{
			var targetInfos = target
				.GetType()
				.GetProperties();

			foreach (var prop in targetInfos)
			{
				var sourceInfo = source
					.GetType()
					.GetProperties()
					.Where(x => x.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase))
					.FirstOrDefault();

				if (sourceInfo is not null && sourceInfo.PropertyType == prop.PropertyType)
				{
					prop.SetValue(target, sourceInfo.GetValue(source), null);
				}
			}

			return target;
		}

		public static TDestination Clone<TSource, TDestination>(this TSource source)
			where TSource : BaseResponseDataModel
			where TDestination : BaseResponseDataModel, new()
		{
			ArgumentNullException.ThrowIfNull(source);

			var dest = new TDestination();

			ArgumentNullException.ThrowIfNull(dest);

			var result = source.CopyObject(dest);

			ArgumentNullException.ThrowIfNull(result);

			return result;
		}
	}
}