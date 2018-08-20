using IdentityModel;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;


namespace QuickStartIdentityServer.Quickstart.UI
{
	[SecurityHeaders]
	public class AccountController : Controller
    {
		//private readonly TestUserStore _users;
		//private readonly IIdentityServerInteractionService _interaction;
		//private readonly IClientStore _clientStore;
		//private readonly IAuthenticationSchemeProvider _schemeProvider;
		//private readonly IEventService _events;
		//public AccountController(
		//	IIdentityServerInteractionService interaction,
		//	IClientStore clientStore,
		//	IAuthenticationSchemeProvider schemeProvider,
		//	IEventService events,
		//	TestUserStore users = null)
		//{
		//	// if the TestUserStore is not in DI, then we'll just use the global users collection
		//	// this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
		//	_users = users ?? new TestUserStore(TestUsers.Users);

		//	_interaction = interaction;
		//	_clientStore = clientStore;
		//	_schemeProvider = schemeProvider;
		//	_events = events;
		//}
	}
}