using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIDConnectMvc.Models;

namespace OpenIDConnectMvc.Controllers
{
    public class HomeController : Controller
    {
		[Authorize]///
		public IActionResult Index()
        {
            return View();
        }

		[Authorize]
		public IActionResult Secure()
		{
			ViewData["Message"] = "Secure page.";

			return View();
		}

		public async Task Logout()
		{
			await HttpContext.SignOutAsync("Cookies");
			await HttpContext.SignOutAsync("oidc");
		}

		public IActionResult Error()
		{
			return View();
		}
	}
}
