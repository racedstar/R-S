using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RioManager.Models;
using System.Web.SessionState;

namespace RioManager.Views.Tools
{
    /// <summary>
    /// fbLogin 的摘要描述
    /// </summary>
    public class fbLogin : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (HttpContext.Current.Request.QueryString.Get("id") != null && HttpContext.Current.Request.QueryString.Get("name") != null && HttpContext.Current.Request.QueryString.Get("email") != null)
            {
                string email = HttpContext.Current.Request.QueryString.Get("email");
                Vw_Account account = new AccountModel().getVwAccountByFBEmail(email);
                if (account == null)
                {
                    string name = HttpContext.Current.Request.QueryString.Get("name");
                    string id = HttpContext.Current.Request.QueryString.Get("id");
                    Rio_Account rio_Account = addRioAccount(email, name); //註冊新帳號
                    int accountSN = new AccountModel().getVwAccountByID(rio_Account.ID).SN; // get帳號SN
                    addFaceBookLogin(accountSN, email, id, name); //加入FB使用者資訊(id, name, email)
                    context.Session["UserSN"] = accountSN;
                    context.Session["UserID"] = rio_Account.ID;
                }
                else
                {
                    context.Session["UserSN"] = account.SN;
                    context.Session["UserID"] = account.ID;                    
                }                
            }
        }

        private Rio_Account addRioAccount(string email, string name)
        {
            Rio_Account rio_Account = new Rio_Account();
            string createID = "FaceBookRegister";
            DateTime dt = DateTime.Now;
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999);
            email = email.Split('@')[0];

            rio_Account.ID = email + randomNumber;
            rio_Account.Name = name;
            rio_Account.Password = string.Empty;
            rio_Account.AccountContent = string.Empty;
            rio_Account.Email = string.Empty;
            rio_Account.PicSN = 0;

            rio_Account.CreateID = createID;
            rio_Account.CreateName = createID;
            rio_Account.ModifyID = createID;
            rio_Account.ModifyName = createID;
            rio_Account.CreateDate = dt;
            rio_Account.ModifyDate = dt;

            rio_Account.IsEnable = true;
            rio_Account.IsDelete = false;

            rio_Account.IsFBAccount = true;

            new AccountModel().Insert(rio_Account);

            return rio_Account;
        }

        private void addFaceBookLogin(int accountSN, string email, string id, string name)
        {
            Rio_FBLogin rio_FBLogin = new Rio_FBLogin();
            rio_FBLogin.AccountSN = accountSN;
            rio_FBLogin.Email = email;
            rio_FBLogin.Facebook_ID = id;
            rio_FBLogin.Name = name;
            new FBLoginModel().Insert(rio_FBLogin);
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