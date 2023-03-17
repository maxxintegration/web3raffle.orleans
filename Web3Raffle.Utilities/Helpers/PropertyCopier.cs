namespace Web3raffle.Utilities.Helpers
{
	public class PropertyCopier<TParent, TChild> where TParent : class
												 where TChild : class, new()
	{
		public static void CopyCollection(List<TParent> parent, List<TChild> child, List<string>? excludeProperty = null)
		{
			foreach (var item in parent)
			{
				var childItem = new TChild();
				Copy(item, childItem, excludeProperty);
				child.Add(childItem);
			}
		}

		public static void Copy(TParent parent, TChild child, List<string>? excludeProperty = null)
		{
			var parentProperties = parent.GetType().GetProperties();
			var childProperties = child.GetType().GetProperties();

			foreach (var parentProperty in parentProperties)
			{
				foreach (var childProperty in childProperties)
				{
					bool excludeThisProperty = excludeProperty != null && excludeProperty.Where(x => x.ToLower() == parentProperty.Name.ToLower()).Count() > 0;

					if (!excludeThisProperty && parentProperty.Name == childProperty.Name && parentProperty.PropertyType == childProperty.PropertyType)
					{
						childProperty.SetValue(child, parentProperty.GetValue(parent));
						break;
					}
				}
			}
		}
	}
}