using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Server.ViewModel;

namespace Server.Controllers
{
    public class AccountController : Controller
    {
		private readonly TestUserStore _user;  //放入DI容器中的TestUser(GeTestUsers方法)，通过这个对象获取
		public AccountController(TestUserStore user)
		{
			_user = user;
		}
		public IActionResult Login(string strReturnUrl = null)
		{
			ViewData["ReturnUrl"] = strReturnUrl;
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginViewModel,string ReturnUrl)
		{
			//用户登录
			if(ModelState.IsValid)
			{
				ViewData["ReturnUrl"] = ReturnUrl;
				var user = _user.FindByUsername(loginViewModel.Email);
				if(user == null)
				{
					ModelState.AddModelError(nameof(loginViewModel.Email),"Email not exists");
				}
				else
				{
					var result = _user.ValidateCredentials(loginViewModel.Email,loginViewModel.Password);
					if(result)
					{
						var props = new AuthenticationProperties()
						{
							IsPersistent = true,
							ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
						};
						await Microsoft.AspNetCore.Http.AuthenticationManagerExtensions.SignInAsync(   //Id4扩展方法和HttpContext扩展方法重名，这里强制使用命名空间方法
							this.HttpContext,
							user.SubjectId,
							user.Username,
							props);
						return Redirect(ReturnUrl);
					}
					else
					{
						ModelState.AddModelError(nameof(loginViewModel.Email),"Wrong password");
					}
				}
			}

			return View();
		}
	}
}