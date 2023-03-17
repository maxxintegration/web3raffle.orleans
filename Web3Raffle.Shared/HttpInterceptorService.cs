using System.Net;
using Toolbelt.Blazor;
using Web3raffle.Shared.Exceptions;

namespace Web3raffle.Shared
{
	// SOURCE FROM:  https://code-maze.com/global-http-error-handling-in-blazor-webassembly/
	// Handle error response in Webassembly
	public class HttpInterceptorService
	{
		private readonly HttpClientInterceptor _interceptor;

		public HttpInterceptorService(HttpClientInterceptor interceptor)
		{
			this._interceptor = interceptor;
		}

		public HttpResponseMessage? Response { get; set; }

		public void RegisterEvent() => this._interceptor.AfterSend += this.InterceptResponse!;

		private void InterceptResponse(object sender, HttpClientInterceptorEventArgs e)
		{
			string message = string.Empty;
			if (!e.Response.IsSuccessStatusCode)
			{
				var statusCode = e.Response.StatusCode;
				this.Response = e.Response;
				switch (statusCode)
				{
					case HttpStatusCode.BadRequest:
						break;

					case HttpStatusCode.NotFound:
						throw new HttpResponseException("The requested resorce was not found.");
					case HttpStatusCode.Unauthorized:
						throw new HttpResponseException("User is not authorized");
					default:
						throw new HttpResponseException("Something went wrong, please contact Administrator");
				}
			}
		}

		public void DisposeEvent() => this._interceptor.AfterSend -= this.InterceptResponse!;
	}
}