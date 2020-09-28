using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace HangFireDemo
{
    /// <summary>
    /// Hangfire仪表盘配置授权
    /// </summary>
    public class DashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public IConfiguration Configuration { get; }

        public DashboardAuthorizationFilter(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var header = httpContext.Request.Headers["Authorization"];
            if (string.IsNullOrWhiteSpace(header))
            {
                SetChallengeResponse(httpContext);
                return false;
            }
            var authValues = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(header);
            if (!"Basic".Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase))
            {
                SetChallengeResponse(httpContext);
                return false;
            }
            var parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter));
            var parts = parameter.Split(':');

            if (parts.Length < 2)
            {
                SetChallengeResponse(httpContext);
                return false;
            }
            var username = parts[0];
            var password = parts[1];
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                SetChallengeResponse(httpContext);
                return false;
            }
            var LoginName = Configuration["Hangfire:User"];
            var LoginPassword = Configuration["Hangfire:Password"];
            if (username == LoginName && password == LoginPassword)
            {
                return true;
            }
            SetChallengeResponse(httpContext);
            return false;
        }
        private void SetChallengeResponse(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 401;
            httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
            httpContext.Response.WriteAsync("Authentication is required.");
        }
    }
}
