using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.ViewModel
{
	/// <summary>
	/// 解析跳转url后得到的应用权限等信息
	/// </summary>
	public class ConsentViewModel:InputConsentViewModel
    {
		public string ClientId
		{
			get; set;
		}
		public string ClientName
		{
			get; set;
		}
		public string ClientUrl
		{
			get; set;
		}
		public string ClientLogoUrl
		{
			get; set;
		}
		public IEnumerable<ScopeViewModel> IdentityScopes
		{
			get; set;
		}
		public IEnumerable<ScopeViewModel> ResourceScopes
		{
			get; set;
		}
	}
}
