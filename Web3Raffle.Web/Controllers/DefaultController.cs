using Microsoft.AspNetCore.Mvc;

namespace Web3raffle.Web.Controllers;

public class DefaultController : Controller
{
	public IActionResult Index()
	{
		return this.View();
	}
}