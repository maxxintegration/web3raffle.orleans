using Newtonsoft.Json;
using Web3raffle.Api.ExternalServices.IServices;
using Web3raffle.Models.Data;

namespace Web3raffle.Api.ExternalServices.Services;

public class VeeFriendsProfileService : IExternalProfileService
{
	private readonly HttpClient httpClient;
	private readonly ILogger<VeeFriendsProfileService> logger;

	public VeeFriendsProfileService(HttpClient httpClient, ILogger<VeeFriendsProfileService> logger)
	{
		this.httpClient = httpClient;
		this.logger = logger;
	}

	public async Task<ExternalProfileModel?> GetProfileByWalletAddress(string walletAddress, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrEmpty(walletAddress))
		{
			return null;
		}

		walletAddress = walletAddress.Trim().ToLower();

		try
		{
			var responseAsString = await this.httpClient.GetStringAsync($"api/users/{walletAddress}", cancellationToken);
			return JsonConvert.DeserializeObject<ExternalProfileModel>(responseAsString)!;
		}
		catch (Exception ex)
		{
			this.logger.LogError(ex, "Could not get profile.");

			return null;
		}
	}
}