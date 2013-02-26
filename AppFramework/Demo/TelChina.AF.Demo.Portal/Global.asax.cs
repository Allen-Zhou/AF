using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using TelChina.AF.Service.AppHosting;
using TelChina.AF.Persistant;
using TelChina.AF.Sys.Service;
using TelChina.AF.Sys.Context;

namespace TelChina.AF.Demo.Portal
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
           
            AppHost.Start();
            RepositoryContext.Config();
            //string SESSIONKEY = "SESSIONKEY";
            //Session[SessionEnum.curUser.ToString()] = user;
            //Session[SessionEnum.curUserLogName.ToString()] = logName;
            //Session[SessionEnum.curOrgCode.ToString()] = user.OrgCode;
            //Session[SessionEnum.curUserName.ToString()] = user.Name;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //ContextSession.Provider = new HttpContextSessionProvider();
            var ctx = new ServiceContext()
            {
                LoginIP = "127.0.0.1",
                LoginDate = DateTime.Now,
                Token = Guid.NewGuid().ToString(),
                Content = new Dictionary<string, string>(),
                UserCode = "Allen",
                UserID = "",
                UserName = "AllenZhou"
            };

            ContextSession.Current = ctx;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            AppHost.Stop();
        }
    }
}