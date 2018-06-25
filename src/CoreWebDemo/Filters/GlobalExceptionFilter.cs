using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebDemo.Filters
{
    public class GlobalExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var type = System.Reflection.MethodBase.GetCurrentMethod();
            IActionResult result = new ContentResult() { Content = "程序内部错误：" + context.Exception.Message, StatusCode = 500 };
            context.Result = result;
        }

    }
}
