using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TelChina.AF.Sys.Service;
using TelChina.AF.Demo.DemoSV;
using TelChina.AF.Sys.Context;

namespace TelChina.AF.Demo.Portal
{
    public partial class BizPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.TextBox1.Text = ContextSession.Current.UserCode;
            var sv = ServiceProxy.CreateProxy<ITransSV>();
            var result = sv.Required(true);
        }
    }
}