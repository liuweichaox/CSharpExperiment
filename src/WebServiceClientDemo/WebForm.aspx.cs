using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebServiceClientDemo
{
    public partial class WebForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            MyWebService.WebService webService = new MyWebService.WebService();
            var str=webService.HelloWorld();
            Response.Write(str);
        }
    }
}