using System.Text.RegularExpressions;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;

namespace Web3raffle.Utilities.Repositories.CosmosDb
{
	public class CosmosDbRepository<T> where T : BaseResponseDataModel
	{
		private readonly Container cosmosContainer;

		public CosmosDbRepository(Container cosmosContainer)
		{
			this.cosmosContainer = cosmosContainer;
		}

		public async Task<List<T>> ReadManyItemsAsync(QueryModel queryParams, CancellationToken ct)
		{
			queryParams.Filter = ToSqlFilter(queryParams.Filter);
			queryParams.OrderBy = ToSqlOrder(queryParams.OrderBy);

			var documentName = typeof(T).Name;
			var queryText = $"SELECT * FROM {documentName} T1 {queryParams.Filter} {queryParams.OrderBy}";

			var data = new List<T>();

			// 0 maximum parallel tasks, effectively serial execution
			var options = new QueryRequestOptions()
			{
				MaxBufferedItemCount = 1000,
				MaxConcurrency = 0
			};

			var top = queryParams.Top ?? 1;
			var skip = queryParams.Skip ?? 0;

			using var query = this.cosmosContainer.GetItemQueryIterator<T>(queryText, requestOptions: options);

			while (query.HasMoreResults && data.Count < queryParams.Top)

			{
				foreach (T dataItem in await query.ReadNextAsync(ct))
				{
					Interlocked.Decrement(ref skip);

					if (skip > 0)
					{
						continue;
					}

					if (top > 0 && top <= data.Count)
					{
						break;
					}

					data.Add(dataItem);
				}
			}

			return data;
		}

		public async Task<List<T>> ReadManyItemsAsync(string sqlQuery, CancellationToken ct)
		{
			var data = new List<T>();

			var options = new QueryRequestOptions()
			{
				MaxBufferedItemCount = 1000,
				MaxConcurrency = 0
			};

			using (var query = this.cosmosContainer.GetItemQueryIterator<T>(sqlQuery, requestOptions: options))
			{
				while (query.HasMoreResults)
				{
					foreach (T dataItem in await query.ReadNextAsync(ct))
					{
						data.Add(dataItem);
					}
				}
			}

			return data;
		}

		public async Task<int> ReadCountManyItemsAsync(QueryModel queryParams, CancellationToken ct)
		{
			queryParams.Filter = ToSqlFilter(queryParams.Filter);

			var documentName = typeof(T).Name;
			var queryText = $"SELECT * FROM {documentName} T1 {queryParams.Filter}";

			var options = new QueryRequestOptions()
			{
				MaxBufferedItemCount = 1000,
				MaxConcurrency = 0
			};

			var count = 0;

			using var query = this.cosmosContainer.GetItemQueryIterator<T>(queryText, requestOptions: options);

			while (query.HasMoreResults)
			{
				foreach (T dataItem in await query.ReadNextAsync(ct))
				{
					Interlocked.Increment(ref count);
				}
			}

			return count;
		}

		public async Task<ItemResponse<T>> ReadAsync(string id, CancellationToken ct)
		{
			var result = await this
				.cosmosContainer
				.ReadItemAsync<T>(id, new PartitionKey(id), cancellationToken: ct)
				.ConfigureAwait(false);

			return result;
		}

		public async Task<ItemResponse<T>> WriteAsync(T modelToCreate, CancellationToken ct)
		{
			var result = await this
				.cosmosContainer
				.CreateItemAsync(modelToCreate, new PartitionKey(modelToCreate.Id), cancellationToken: ct)
				.ConfigureAwait(false);

			return result;
		}

		public async Task<ItemResponse<T>> UpdateAsync(T modelToUpdate, CancellationToken ct)
		{
			modelToUpdate.ModifiedAt = DateTimeOffset.UtcNow;

			var result = await this
				.cosmosContainer
				.UpsertItemAsync(modelToUpdate, new PartitionKey(modelToUpdate.Id), cancellationToken: ct)
				.ConfigureAwait(false);

			return result;
		}

		public async Task<ItemResponse<T>> DeleteAsync(string id, CancellationToken ct)
		{
			var result = await this
				.cosmosContainer
				.DeleteItemAsync<T>(id, new PartitionKey(id), cancellationToken: ct)
				.ConfigureAwait(false);

			return result;
		}

		private static string ToSqlOrder(string? orderString)
		{
			if (string.IsNullOrEmpty(orderString))
			{
				return string.Empty;
			}

			string[] arr = orderString.Split(",");

			for (int x = 0; x < arr.Length; x++)
			{
				arr[x] = $"T1.{arr[x].Trim()}";
			}

			orderString = $"ORDER BY {string.Join(", ", arr)}";

			return orderString;
		}

		private static string ToSqlFilter(string? filterString)
		{
			var operators = new Dictionary<string, string>
		{
			{ "eq", "=" },
			{ "ne", "<>" },
			{ "gt", ">" },
			{ "ge", ">=" },
			{ "lt", "<" },
			{ "le", "<=" }
		};

			if (string.IsNullOrEmpty(filterString))
			{
				return string.Empty;
			}

			foreach (var kvp in operators)
			{
				var regex = new Regex($"\\s{kvp.Key}\\s", RegexOptions.IgnoreCase | RegexOptions.Compiled);

				filterString = regex.Replace(filterString ?? string.Empty, $" {kvp.Value} ");
			}

			filterString = LogicalFilterParse(filterString);

			return $"{(string.IsNullOrEmpty(filterString) ? "" : "WHERE")} {filterString}".Trim();
		}

		private static string LogicalFilterParse(string filterString)
		{
			var count = 0;
			var logicals = new List<string> { "and", "or" };

			if (string.IsNullOrEmpty(filterString))
			{
				return string.Empty;
			}

			foreach (string logical in logicals)
			{
				count++;

				var regex = new Regex($"(\\s{logical}\\s)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				var arr = regex.Split(filterString);

				for (int x = 0; x < arr.Length; x++)
				{
					if (arr[x].Trim().ToLower() == logical)
					{
						continue;
					}

					if (arr[x].Trim().StartsWith("("))
					{

						arr[x] = $"(T1.{arr[x].Trim()[1..]}";

					}
					else
					{
						arr[x] = $"T1.{arr[x].Trim()}";
					}
				}

				filterString = string.Join("", arr);

			}

			// CLEAN UP DOUBLE TABLE ALIAS T1.T1.
			while (filterString.IndexOf("T1.T1.") > -1)
			{
				filterString = $"{filterString.Replace("T1.T1.", "T1.")}";
			}

			filterString = filterString.Replace("T1.ARRAY_CONTAINS", "ARRAY_CONTAINS", StringComparison.OrdinalIgnoreCase);

			return filterString;
		}
	}
}