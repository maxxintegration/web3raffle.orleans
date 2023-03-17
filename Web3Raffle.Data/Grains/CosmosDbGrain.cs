using Microsoft.Extensions.Caching.Distributed;
using Web3raffle.Models.Requests;
using Web3raffle.Models.Responses;
using Web3raffle.Shared.Exceptions;
using Web3raffle.Utilities.Repositories.CosmosDb;

namespace Web3raffle.Data.Grains;

[CollectionAgeLimit(Minutes = 2)]
[VFGrainPlacement]
public class CosmosDb<T> : Grain, ICosmosDbGrain<T> where T : BaseResponseDataModel
{
	private readonly CosmosDbRepository<T> cosmosRepository;

	//private readonly IDistributedCache distributedCache;
	private readonly ILogger<CosmosDb<T>> logger;

	public CosmosDb(CosmosDbRepository<T> cosmosRepository, IDistributedCache distributedCache, ILogger<CosmosDb<T>> logger)
	{
		this.cosmosRepository = cosmosRepository;
		//this.distributedCache = distributedCache;
		this.logger = logger;
	}

	public async Task<T> Read(string id, GrainCancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(id);

		//var cachedResult = this.distributedCache.GetString(id);

		//if (!string.IsNullOrEmpty(cachedResult))
		//{
		//	var cachedModel = cachedResult.FromJson<T>();

		//	if (cachedModel is not null)
		//	{
		//		this.logger.LogInformation("Cache hit for {id}", id);

		//		return cachedModel;
		//	}
		//}

		//this.logger.LogInformation("Cache miss for {id}", id);

		try
		{
			T objResult = await this.cosmosRepository.ReadAsync(id, ct.CancellationToken);
			return objResult;
		}
		catch
		{
			return null!;
		}

		//if (objResult is not null)
		//{
		//	var cacheOptions = new DistributedCacheEntryOptions
		//	{
		//		AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
		//	};

		//	var jsonResult = objResult.ToJson();

		//	await this.distributedCache.SetStringAsync(id, jsonResult, cacheOptions, ct);

		//	this.logger.LogInformation("Cache set for {id}", id);
		//}

		//CosmosDbException.ThrowEmptyModel(objResult);
	}

	public async Task<T> Write(T value, GrainCancellationToken ct)
	{
		CosmosDbException.ThrowEmptyModel(value);

		T result = await this.cosmosRepository.WriteAsync(value, ct.CancellationToken);

		CosmosDbException.ThrowEmptyModel(result);

		//if (result is not null)
		//{
		//	var cacheOptions = new DistributedCacheEntryOptions
		//	{
		//		AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
		//	};

		//	var jsonResult = result.ToJson();

		//	await this.distributedCache.SetStringAsync(result.Id, jsonResult, cacheOptions, ct);

		//	this.logger.LogInformation("Cache set for {id}", result.Id);
		//}

		return result;
	}

	public async Task<T> Update(T value, GrainCancellationToken ct)
	{
		CosmosDbException.ThrowEmptyModel(value);

		T result = await this.cosmosRepository.UpdateAsync(value, ct.CancellationToken);

		CosmosDbException.ThrowEmptyModel(result);

		//if (result is not null)
		//{
		//	var cacheOptions = new DistributedCacheEntryOptions
		//	{
		//		AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
		//	};

		//	var jsonResult = result.ToJson();

		//	await this.distributedCache.SetStringAsync(result.Id, jsonResult, cacheOptions, ct);

		//	this.logger.LogInformation("Cache set for {id}", result.Id);
		//}

		return result;
	}

	public async Task<T> Delete(string id, GrainCancellationToken ct)
	{
		ArgumentNullException.ThrowIfNull(id);

		var result = await this.cosmosRepository.DeleteAsync(id, ct.CancellationToken);

		//CosmosDbException.ThrowEmptyModel(result);

		//if (result is not null)
		//{
		//	await this.distributedCache.RemoveAsync(id, ct);

		//	this.logger.LogInformation("Cache removed for {id}", id);
		//}

		return result;
	}

	public async Task<List<T>> Read(QueryModel queryParams, GrainCancellationToken ct)
	{
		try
		{
			var result = await this.cosmosRepository.ReadManyItemsAsync(queryParams, ct.CancellationToken);
			return result;
		}
		catch
		{
			return null!;
		}

		//var result = await this.cosmosRepository.ReadManyItemsAsync(queryParams, ct);

		//CosmosDbException.ThrowEmptyModel(result);

		//return result;
	}

	public async Task<List<T>> Query(string sqlQuery, GrainCancellationToken ct)
	{
		try
		{
			var result = await this.cosmosRepository.ReadManyItemsAsync(sqlQuery, ct.CancellationToken);
			return result;
		}
		catch (Exception ex)
		{
			this.logger.LogError(ex.ToString());
			return Array.Empty<T>().ToList();
		}
	}

	public async Task<int> Count(QueryModel queryParams, GrainCancellationToken ct)
	{
		return await this.cosmosRepository.ReadCountManyItemsAsync(queryParams, ct.CancellationToken);
	}
}