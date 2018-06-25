#dotNet Net Core 实现HttpContext.Current

using Microsoft.AspNetCore.Builder;

using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.DependencyInjection;

using System;

using System.Collections.Generic;

using System.Text;

namespace Core

{
    public static class HttpContext

    {

	public static IServiceProvider ServiceProvider = null;


	public static Microsoft.AspNetCore.Http.HttpContext Current

	{
            get
            {
                var service = ServiceProvider.GetService<IHttpContextAccessor>();
                return service.HttpContext;
            }
        }
    }
    /*============================================================================================
                                  dotNet Core 实现HttpContext.Current

    1、新建HttpContext.cs类文件，获取IHttpContextAccessor接口服务对象实例，代码如下：

    public static class HttpContext
    
	{
    
    public static IServiceProvider ServiceProvider = null;


        public static Microsoft.AspNetCore.Http.HttpContext Current
        {
            get
            {
                var service = ServiceProvider.GetService<IHttpContextAccessor>();
                return service.HttpContext;
            }
        }
    }

    2、 Startup.cs--ConfigureServices下注入HttpContextAccessor接口服务，代码如下：
     
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    
    3、 Startup.cs--Configure下配置自定义服务对象实例，代码如下：

     Core.HttpContext.ServiceProvider = app.ApplicationServices;（Core此处是自定义程序集名称）
    
    
    ============================================================================================= */
}
