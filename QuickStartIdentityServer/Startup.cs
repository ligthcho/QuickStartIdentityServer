using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace QuickStartIdentityServer
{
	public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
		{//扩展在每次启动时，为令牌签名创建了一个临时密钥。在生成环境需要一个持久化的密钥
			services.AddIdentityServer()
				.AddDeveloperSigningCredential()
				.AddInMemoryIdentityResources(Config.GetIdentityResources())//添加身份验证资源
				.AddInMemoryApiResources(Config.GetApiResources())//添加api资源
				.AddInMemoryClients(Config.GetClients())///添加客户端
				.AddTestUsers(Config.GetUsers()); //添加测试用户
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app,IHostingEnvironment env,ILoggerFactory loggerFactory)
        {
			loggerFactory.AddConsole(LogLevel.Debug);
			app.UseDeveloperExceptionPage();

			app.UseIdentityServer();

		}
    }
}
