using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.ViewModel;

namespace Server.Controllers
{
	/// <summary>
	/// 用来显示授权登录页面，以及相应的跳转登录逻辑
	/// </summary>
	public class ConsentController:Controller
	{
		private readonly ConsentService _consentService;
		public ConsentController(ConsentService consentService)
		{
			_consentService = consentService;
		}
		public async Task<IActionResult> Index(string returnUrl)
		{
			//调用consentService的BuildConsentViewModelAsync方法，将跳转Url作为参数传入，解析得到一个ConsentViewModel
			var model = await _consentService.BuildConsentViewModelAsync(returnUrl);
			if(model == null)
				return null;
			return View(model);
		}
		[HttpPost]
		public async Task<IActionResult> Index(InputConsentViewModel viewModel)
		{
			//用户选择确认按钮的时候，根据选择按钮确认/取消，以及勾选权限
			var result = await _consentService.PorcessConsent(viewModel);
			if(result.IsRedirectUrl)
			{
				return Redirect(result.RedirectUrl);
			}
			if(!string.IsNullOrEmpty(result.ValidationError))
			{
				ModelState.AddModelError("",result.ValidationError);
			}
			return View(result.ViewModel);
		}
	}
}
