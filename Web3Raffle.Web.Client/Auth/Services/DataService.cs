using Azure;
using VeeFriends.Models.Data.Web3Raffle;
using VeeFriends.Models.Responses;
using VeeFriends.Web.Web3Raffle.Client.Auth.Extensions;

namespace VeeFriends.Web.Web3Raffle.Client.Auth.Services;

public partial class DataService : IDataService
{

	//private readonly IAuthService _authService;

	public DataService(IAuthService authService)
	{
		this.AuthService = authService;
	}

	public IAuthService AuthService { get; }



	//private async Task<ResponseCollectionModel<T>> RequestCollection(Models.Enums.HttpMethod method, string url, object param)
	//{
	//	var response = await this._authService.HttpClientAsync(method, url, param);

	//	var resData = new ResponseCollectionModel<T>();

	//	//if (!response.StatusCode.IsResponseSuccess())
	//	//{
	//	//	resData.Errors = response.Content.ReadAsStringAsync().Result.JsonToObject<ResponseError>();
	//	//}

	//	return resData;
	//}






}