using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TelChina.AF.Sys.Context;
using TelChina.AF.Sys.Service;
using TelChina.AF.Demo.DemoSV;

namespace TelChina.AF.Demo.Portal
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //ContextSession.Provider = new HttpContextSessionProvider();
            SetContext();
        }

        private void SetContext()
        {
            var ctx = new ServiceContext()
                          {
                              LoginIP = "127.0.0.1",
                              LoginDate = DateTime.Now,
                              UserCode = "Allen" ,
                              UserID = "",
                          };
            ContextSession.Current = ctx;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            SetContext();
        }

    }
}