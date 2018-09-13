using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Config
    {
		//定义要保护的资源（webapi）
		public static IEnumerable<ApiResource> GetApiResources()
		{
			return new List<ApiResource>
			{
				new ApiResource("api1", "My API")
			};
		}
		//定义可以访问该API的客户端
		public static IEnumerable<Client> GetClients()
		{
			return new List<Client>
			{
				new Client
				{
					ClientId = "mvc",
                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.Implicit,  //简化模式
                    // secret for authentication
                    ClientSecrets =
					{
						new Secret("secret".Sha256())
					},
					RequireConsent =true,                                  //用户选择同意认证授权
                    RedirectUris={ "http://localhost:6006/signin-oidc" },  //指定允许的URI返回令牌或授权码(我们的客户端地址)
                    PostLogoutRedirectUris={ "http://localhost:6006/signout-callback-oidc" },//注销后重定向地址 参考https://identityserver4.readthedocs.io/en/release/reference/client.html
                    LogoUri="https://ss1.bdstatic.com/70cFuXSh_Q1YnxGkpoWK1HF6hhy/it/u=3298365745,618961144&fm=27&gp=0.jpg",
                    // scopes that client has access to
                    AllowedScopes = {                       //客户端允许访问个人信息资源的范围
                        IdentityServerConstants.StandardScopes.Profile,
						IdentityServerConstants.StandardScopes.OpenId,
						IdentityServerConstants.StandardScopes.Email,
						IdentityServerConstants.StandardScopes.Address,
						IdentityServerConstants.StandardScopes.Phone
					}
				}
			};
		}
		public static List<TestUser> GeTestUsers()
		{
			return new List<TestUser>
			{
				new TestUser
				{
					SubjectId = "1",
					Username = "alice",
					Password = "password"
				},
				new TestUser
				{
					SubjectId = "2",
					Username = "bob",
					Password = "password"
				}
			};
		}
		//openid  connect
		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
			{
				new IdentityResources.OpenId(),
				new IdentityResources.Profile(),
				new IdentityResources.Email()
			};
		}
	}
}
