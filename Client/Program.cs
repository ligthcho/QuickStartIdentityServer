using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
	class Program
    {
        static async Task Main(string[] args)
        {
			//// 从元数据中发现端口
			//var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
			//// 请求令牌
			//var tokenClient = new TokenClient(disco.TokenEndpoint,"client","secret");
			//var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

			//if(tokenResponse.IsError)
			//{
			//	Console.WriteLine(tokenResponse.Error);
			//	return;
			//}

			//Console.WriteLine(tokenResponse.Json);


			// 从元数据中发现客户端
			var disco = await DiscoveryClient.GetAsync("http://localhost:5000");

			// 请求令牌
			var tokenClient = new TokenClient(disco.TokenEndpoint,"ro.client","secret");
			var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice","password","api1");//使用用户名密码

			if(tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return;
			}

			Console.WriteLine(tokenResponse.Json);
			Console.WriteLine("\n\n");

			// 调用api
			var client = new HttpClient();
			client.SetBearerToken(tokenResponse.AccessToken);

			var response = await client.GetAsync("http://localhost:5001/identity");
			if(!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(JArray.Parse(content));
			}
			Console.ReadKey();
		}
	}
}
