using Web3raffle.Models.Data;

namespace Web3raffle.Api.ExternalServices.IServices;

public interface IExternalProfileService
{
	Task<ExternalProfileModel?> GetProfileByWalletAddress(string walletAddress, CancellationToken cancellationToken = default);
}