using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Server.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
	public class ConsentService
	{

		private readonly IClientStore _clientStore;
		private readonly IResourceStore _resourceStore;
		private readonly IIdentityServerInteractionService _identityServerInteractionService;
		public ConsentService(IClientStore clientStore,
			IResourceStore resourceStore,
			IIdentityServerInteractionService identityServerInteractionService)
		{
			_clientStore = clientStore;
			_resourceStore = resourceStore;
			_identityServerInteractionService = identityServerInteractionService;
		}
		private ConsentViewModel CreateConsentViewModel(AuthorizationRequest request,Client client,Resources resources,InputConsentViewModel model)
		{
			//用户选中的Scopes
			var selectedScopes = model?.ScopesConsented ?? Enumerable.Empty<string>();
			//客户端传入信息填充consentViewModel
			var vm = new ConsentViewModel();
			vm.ClientName = client.ClientName;
			vm.ClientLogoUrl = client.LogoUri;
			vm.ClientUrl = client.ClientUri;
			vm.RemeberConsent = model?.RemeberConsent ?? true;
			vm.IdentityScopes = resources.IdentityResources.Select(t => CreateScopeViewModel(t,selectedScopes.Contains(t.Name) || model == null));    //resources的IdentityResources需要转换成我们自己的ViewModel ; 假如用户存在用户选中的Scope的话check 就传递一个true
			vm.ResourceScopes = resources.ApiResources.SelectMany(t => t.Scopes).Select(e => CreateScopeViewModel(scope: e,check: selectedScopes.Contains(e.Name) || model == null));
			return vm;
		}
		private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource,bool check)
		{
			return new ScopeViewModel
			{
				Name = identityResource.Name,
				Checked = check || identityResource.Required,
				DisplayName = identityResource.DisplayName,
				Description = identityResource.Description,
				Required = identityResource.Required,
				Emphasize = identityResource.Emphasize
			};
		}
		private ScopeViewModel CreateScopeViewModel(Scope scope,bool check)
		{
			return new ScopeViewModel
			{
				Name = scope.Name,
				Checked = check || scope.Required,
				DisplayName = scope.DisplayName,
				Description = scope.Description,
				Required = scope.Required,
				Emphasize = scope.Emphasize
			};
		}
		public async Task<ConsentViewModel> BuildConsentViewModelAsync(string returnUrl,InputConsentViewModel viewModel = null)
		{
			var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl); //解析returnUrl 拿到AuthorizationRequest对象，这个对象中包含ClientId等信息
			if(request == null)
				return null;
			var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId); //根据request的ClientId拿到client
			var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);//拿到api的resource
			var vm = CreateConsentViewModel(request,client,resources,viewModel);
			vm.ReturnUrl = returnUrl;
			return vm;
		}
		public async Task<ProcessConsentResult> PorcessConsent(InputConsentViewModel viewModel)
		{
			var result = new ProcessConsentResult();
			ConsentResponse consentResponse = null;
			if(viewModel.Button == "no")
			{
				consentResponse = ConsentResponse.Denied;
			}
			else if(viewModel.Button == "yes")         //用户选择确认授权，把用户选择的scopes赋值给ConsentResponse的Scopes
			{
				if(viewModel.ScopesConsented != null && viewModel.ScopesConsented.Any())
				{
					consentResponse = new ConsentResponse
					{
						ScopesConsented = viewModel.ScopesConsented,
						RememberConsent = viewModel.RemeberConsent          //是否记住
					};
				}
				result.ValidationError = "请至少选中一个权限";
			}
			if(consentResponse != null)
			{
				var request = await _identityServerInteractionService.GetAuthorizationContextAsync(viewModel.ReturnUrl);
				await _identityServerInteractionService.GrantConsentAsync(request,consentResponse);

				result.RedirectUrl = viewModel.ReturnUrl;
			}
			var consentViewModel = await BuildConsentViewModelAsync(viewModel.ReturnUrl,viewModel);
			result.ViewModel = consentViewModel;
			return result;
		}
	}
}
