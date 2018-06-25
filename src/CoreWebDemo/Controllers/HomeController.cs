using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoreWebDemo.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using System.IO;
using CoreWebDemo.Filters;
using static System.Net.Mime.MediaTypeNames;
using CoreWebDemo.Helper;
using Microsoft.Extensions.Logging;

namespace CoreWebDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;
        public HomeController(ILogger<HomeController> logger, VierificationCodeServices vierificationCodeServices)
        {
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Qrcode()
        {
            return View();
        }
        /// <summary>
        /// 图形验证码
        /// </summary>
        /// <returns></returns>
        public IActionResult ValidateCode([FromServices]VierificationCodeServices vierificationCodeServices)
        {
            /*
                services.AddSession();
                app.UseSession();   
                页面显示
            <img id="imgVerify" src="/Home/ValidateCode" alt="看不清？点击更换" onclick="this.src = this.src+'?'" style="vertical-align:middle;" />
             */
            string code = "";
            MemoryStream ms = vierificationCodeServices.Create(out code);
            HttpContext.Session.SetString("ValidateCode", code);
            Response.Body.Dispose();
            return File(ms.ToArray(), "image/png");
        }

        /// <summary>
        /// 视频
        /// </summary>
        /// <returns></returns>

        //public IActionResult Video()
        //{
        //    var file = Path.Combine(Directory.GetCurrentDirectory(),
        //                            "Upload", "movie.mp4");

        //    return PhysicalFile(file, "video/mp4");
        //}

        public IActionResult Vue() => View();

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });


        public IActionResult Test() => View();
        [HttpPost]
        public JsonResult UploadFile(IList<IFormFile> files, string Name = "")
        {
            var file = Request.Form.Files;

            foreach (var item in file)
            {
                var FileName = item.FileName;
                var PathName = Path.GetExtension(FileName);
                var ContentType = item.ContentType;
                var Length = item.Length;
                //创建文件流
                using (var stream = new FileStream(Environment.CurrentDirectory + "\\Upload\\" + Guid.NewGuid() + PathName, FileMode.Create))
                {
                    item.CopyTo(stream);
                }
            }
            return Json("UploadFile Success");
        }
    }
}
