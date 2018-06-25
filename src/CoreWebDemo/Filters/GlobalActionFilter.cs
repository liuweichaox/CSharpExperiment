using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.Kestrel.Transport.Libuv.Internal.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CoreWebDemo.Filters
{
    public class GlobalActionFilter : ActionFilterAttribute
    {
        public HttpRequest httpRequest;
        public override void OnActionExecuting(ActionExecutingContext context)
        {           
            httpRequest = context.HttpContext.Request;
            var method = MethodBase.GetCurrentMethod();
            //获取控制器信息
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var url = string.Format("/{0}/{1}", controller, action);
            //判断请求方式
            var isAjax = context.HttpContext.Request.Headers["X-Requested-With"].ToString()
              .Equals("XMLHttpRequest", StringComparison.CurrentCultureIgnoreCase);
            var isPost = context.HttpContext.Request.Method.Equals("POST");
            var isGet = context.HttpContext.Request.Method.Equals("GET");
            var json = GetArguments();
            //获取客户端IP信息
            var localIpAddress = context.HttpContext.Connection.LocalIpAddress;//本地IP
            var localPort = context.HttpContext.Connection.LocalPort;//本地端口
            var remoteIpAddress = context.HttpContext.Connection.RemoteIpAddress;//远程IP
            var remotePort = context.HttpContext.Connection.RemotePort;//远程端口

            //IActionResult result=new ContentResult { Content="记得，青睐刘大大！" };//控制返回结果
            //context.Result = result;
            //base.OnActionExecuting(context);

        }

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <returns>返回Json字符串</returns>
        public string GetArguments()
        {
            string result = string.Empty;
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            if (httpRequest.QueryString.HasValue)
            {
                var keys = httpRequest.Query.Keys.ToArray();
                foreach (var key in keys)
                {
                    var value = httpRequest.Query[key];
                    keyValues.Add(key, value);
                }
                result = JsonConvert.SerializeObject(keyValues);
            }
            if (httpRequest.HasFormContentType)
            {
                if (httpRequest.Form.Count > 0 && httpRequest.Form.Any())
                {
                    foreach (var key in httpRequest.Form)
                    {
                        keyValues.Add(key.Key, key.Value);
                    }
                    result = JsonConvert.SerializeObject(keyValues);
                }
            }
            if (httpRequest.Body.CanRead)
            {
                using (StreamReader stream = new StreamReader(httpRequest.Body))
                {
                    var readStr = stream.ReadToEnd();
                    if (!string.IsNullOrEmpty(readStr))
                    {
                        result = readStr;
                    }
                }
            }
            return result;
        }
    }
}
