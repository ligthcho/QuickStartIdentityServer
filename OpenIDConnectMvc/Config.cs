using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenIDConnectMvc
{
    public class Config
    {
		public static IEnumerable<IdentityResource> GetIdentityResources()
		{
			return new List<IdentityResource>
	        {
	        	new IdentityResources.OpenId(),
	        	new IdentityResources.Profile(),
	        };
		}
	}
}
