using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RioManager.Models;
using System.Web.SessionState;

namespace RioManager.Views.Tools
{
    /// <summary>
    /// selectCover 的摘要描述
    /// </summary>
    public class selectCover : IHttpHandler, IRequiresSessionState
    {        
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string UserID = string.Empty;
            if (context.Session["UserID"] !=null)
            {
                UserID = context.Session["UserID"].ToString();
            }
            
            if(context.Request.QueryString["s"] != null && context.Request.QueryString["t"] != null)
            {
                int SN = 0;
                int picSN = 0;
                string type = string.Empty;
                SN = new AccountModel().getAccountByID(UserID).SN;
                type = context.Request.QueryString["t"].ToString();
                int.TryParse(context.Request.QueryString["s"], out picSN);

                if (type.Equals("Account"))
                {
                    Rio_Account Account = new AccountModel().getAccountByID(UserID);
                    Account.PicSN = picSN;
                    new AccountModel().Update(Account);
                }
                else if (type.Equals("Index"))
                {
                    Rio_UserIndexSetting userSetting = new UserIndexSettingMode().getUserIndexSettingBySN(SN);
                    userSetting.CoverSN = picSN;
                    new UserIndexSettingMode().Update(userSetting);
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}