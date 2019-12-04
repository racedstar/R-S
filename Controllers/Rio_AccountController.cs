using Newtonsoft.Json.Linq;
using PagedList;
using RioManager.Models;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace RioManager.Controllers
{
    public class Rio_AccountController : Controller
    {
        private Entities db = new Entities();

        private int pageSize = 20;
        #region 系統產生
        // GET: Rio_Account
        public ActionResult Index(int? page)
        {
            if(HttpContext.Session["UserID"] != null)
            {
                if(HttpContext.Session["UserID"].ToString() == "Rio")
                { 
                    var data = db.Rio_Account.Where(o => o.IsDelete == false).OrderBy(o => o.SN);

                    var pageNumber = page ?? 1;

                    var pageData = data.ToPagedList(pageNumber, pageSize);
                    return View(pageData);
                }
            }
            return RedirectToAction("Login");
        }

        // GET: Rio_Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int SN = 0;
            int.TryParse(id.ToString(), out SN);
            Vw_Account rio_Account = AccountModel.getVwAccount(SN);
            if (rio_Account == null)
            {
                return HttpNotFound();
            }
            return View(rio_Account);
        }

        // GET: Rio_Account/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Rio_Account/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(string ID,string Password, string Name,string AccountContent, bool IsEnable)
        {
            Rio_Account rio_Account = new Rio_Account();
            string createID = string.Empty;
            if (HttpContext.Session["UserID"] != null)
            {
                createID = HttpContext.Session["UserID"].ToString();
            }
            DateTime dt = DateTime.Now;

            rio_Account.ID = ID;
            rio_Account.Name = Name;
            rio_Account.Password = App_Code.Coding.stringToSHA512(Password);
            rio_Account.AccountContent = AccountContent;
            rio_Account.Email = string.Empty;
            rio_Account.PicSN = 0;

            rio_Account.CreateID = createID;
            rio_Account.CreateName = createID;
            rio_Account.ModifyID = createID;
            rio_Account.ModifyName = createID;
            rio_Account.CreateDate = dt;
            rio_Account.ModifyDate = dt;

            rio_Account.IsEnable = IsEnable;
            rio_Account.IsDelete = false;

            AccountModel.Insert(rio_Account);

            return RedirectToAction("Index");
        }

        // GET: Rio_Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Account rio_Account = db.Rio_Account.Find(id);
            rio_Account.Password = string.Empty;
            if (rio_Account == null)
            {
                return HttpNotFound();
            }
            return View(rio_Account);
        }

        // POST: Rio_Account/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int SN,string Name,string Password, string AccountContent,int PicSN,bool IsEnable)
        {
            if (ModelState.IsValid)
            {
                Rio_Account rio_Account = db.Rio_Account.Find(SN);
                string modifyID = string.Empty;                
                DateTime dt = DateTime.Now;

                if (HttpContext.Session["UserID"] != null)
                {
                    modifyID = HttpContext.Session["UserID"].ToString();
                }

                rio_Account.Name = Name;
                rio_Account.Password = App_Code.Coding.stringToSHA512(Password);
                rio_Account.AccountContent = AccountContent;
                rio_Account.PicSN = PicSN;

                rio_Account.ModifyID = modifyID;
                rio_Account.ModifyName = modifyID;
                rio_Account.ModifyDate = dt;

                rio_Account.IsEnable = IsEnable;

                AccountModel.Update(rio_Account);

            }
            return RedirectToAction("Index");
        }

        // GET: Rio_Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rio_Account rio_Account = db.Rio_Account.Find(id);
            if (rio_Account == null)
            {
                return HttpNotFound();
            }
            return View(rio_Account);
        }

        // POST: Rio_Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rio_Account rio_Account = db.Rio_Account.Find(id);
            //db.Rio_Account.Remove(rio_Account);
            rio_Account.IsDelete = true;
            AccountModel.Update(rio_Account);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region 登入、登出、註冊
        public ActionResult Login(string ID, string Password)
        {
            if (Session["UserID"] != null && Session["UserSN"] != null)
            {
                return RedirectToAction("Index","Home",new { vid = HttpContext.Session["UserID"].ToString() });
            }

            if(Request.HttpMethod == "POST")
            {
                #region Google reCAPTCHA驗證
                var response = Request["g-recaptcha-response"];
                string secretKey = "6LdrUTgUAAAAAC-zzRKYaXa4KjCJSon9K6K9gaJr";
                var client = new WebClient();
                var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
                var obj = JObject.Parse(result);
                var status = (bool)obj.SelectToken("success");
                #endregion

                if (status == true)
                {
                    int accountSN = 0;
                    int.TryParse(AccountModel.LoginCheck(ID, Password).ToString(), out accountSN);
                    if (ModelState.IsValid && accountSN != 0)
                    {
                        HttpContext.Session["UserSN"] = accountSN;
                        HttpContext.Session["UserID"] = ID;                        
                        return RedirectToAction("Index", "Home", new { vid = HttpContext.Session["UserID"].ToString() });
                    }
                    ModelState.AddModelError("Password", "帳號密碼錯誤，請重新輸入");
                }
                else
                {
                    ModelState.AddModelError("reCAPTCHA", "請勾選我不是機器人");
                }
            }
            return View();
        }        

        public void LoingChecked()
        {            
            if (System.Web.HttpContext.Current.Session["UserID"] == null || System.Web.HttpContext.Current.Session["IsLogin"] == null)
            {
                Response.Redirect("/Rio_Account/Login");
            }
         
        }

        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Login");
        }

        public ActionResult RioAccountRegister()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RioAccountRegister(string ID, string Password, string Name, string AccountContent)
        {

            #region Google reCAPTCHA驗證
            var response = Request["g-recaptcha-response"];
            string secretKey = "6LdrUTgUAAAAAC-zzRKYaXa4KjCJSon9K6K9gaJr";
            var client = new WebClient();
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response));
            var obj = JObject.Parse(result);
            var status = (bool)obj.SelectToken("success");
            #endregion

            if (status == true)
            { 
                Vw_Account Account = AccountModel.getVwAccountByID(ID);
                if (Account == null)
                {
                    if (!ID.Equals(string.Empty) && !Password.Equals(string.Empty) && !Name.Equals(string.Empty))
                    {
                        Rio_Account rio_Account = new Rio_Account();
                        string createID = "UserRegister";
                        DateTime dt = DateTime.Now;

                        rio_Account.ID = ID;
                        rio_Account.Name = Name;
                        rio_Account.Password = App_Code.Coding.stringToSHA512(Password);
                        rio_Account.AccountContent = AccountContent;
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

                        AccountModel.Insert(rio_Account);

                        HttpContext.Session["UserID"] = ID;
                        HttpContext.Session["IsLogin"] = "true";
                    }
                }
                else
                {
                    ModelState.AddModelError("ID", "已有相同帳號。");
                }
            }
            return View();
        }

        public ActionResult fbLogin(string id, string name, string email)
        {
            if (email != null)
            {
                Vw_Account account = AccountModel.getVwAccountByFBEmail(email);
                if (account == null)
                {
                    Rio_Account rio_Account = saveFBAccount(email, name); //註冊新帳號
                    int accountSN = AccountModel.getVwAccountByID(rio_Account.ID).SN; // get帳號SN
                    saveFacebookAccount(accountSN, email, id, name); //加入FB使用者資訊(id, name, email)
                    Session["UserSN"] = accountSN;
                    Session["UserID"] = rio_Account.ID;
                }
                else
                {
                    Session["UserSN"] = account.SN;
                    Session["UserID"] = account.ID;
                }
            }

            return RedirectToAction("Login");
        }

        private Rio_Account saveFBAccount(string email, string name)
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

            AccountModel.Insert(rio_Account);

            return rio_Account;
        }

        private void saveFacebookAccount(int accountSN, string email, string id, string name)
        {
            Rio_FBLogin rio_FBLogin = new Rio_FBLogin();
            rio_FBLogin.AccountSN = accountSN;
            rio_FBLogin.Email = email;
            rio_FBLogin.Facebook_ID = id;
            rio_FBLogin.Name = name;
            FBLoginModel.Insert(rio_FBLogin);
        }
        #endregion
        [HttpGet]
        //[ValidateAntiForgeryToken]
        #region 使用者自行設定個人資料        
        public ActionResult UserSetting()
        {            
            if (Session["UserID"] != null)
            {
                string UserID = string.Empty;
                string accountCoverPath = string.Empty;
                string indexCoverPath = string.Empty;

                UserID = Session["UserID"].ToString();
                #region 個人資料設定            
                Vw_Account Account = AccountModel.getVwAccountByID(UserID);
                Account.Password = string.Empty;

                if(Account.CoverSN == 0)
                {
                    accountCoverPath = "../Content/img/icon/questionMark.png";
                }
                else
                {
                    accountCoverPath = Account.PicPath + "/Scaling/" + @Account.PicName;
                }

                ViewBag.accountCoverPath = accountCoverPath;
                ViewBag.Account = Account;
                #endregion

                #region 首頁資料設定
                int SN = AccountModel.getAccountByID(UserID).SN;
                if(UserIndexSettingMode.getUserIndexSettingBySN(SN) == null)
                {
                    Rio_UserIndexSetting userSetting = new Rio_UserIndexSetting();
                    userSetting.AccountSN = SN;
                    userSetting.Title = UserID;
                    userSetting.SubTitle = "Index";
                    userSetting.CoverSN = 0;

                    UserIndexSettingMode.Insert(userSetting);                    
                }
                Vw_UserIndexSetting indexSetting = UserIndexSettingMode.getVwUserIndexSettingBySN(SN);

                if(indexSetting.CoverSN == 0)
                {
                    indexCoverPath = "../Content/img/Froncover/indexcover.png";
                }
                else
                {
                    indexCoverPath = indexSetting.CoverPicPath + "/" + indexSetting.CoverName;
                }

                ViewBag.indexCoverPath = indexCoverPath;
                ViewBag.IndexSetting = indexSetting;
                #endregion
                return View();
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UserSetting(string Password, string Name, string AccountContent)
        {
            string UserID = string.Empty;
            if (Session["UserID"] != null)
            {
                UserID = Session["UserID"].ToString();
            }
            int SN = AccountModel.getAccountByID(UserID).SN;

            Rio_Account Account = db.Rio_Account.Find(SN);
            if (!Password.Equals(string.Empty))
            {
                Account.Password = App_Code.Coding.stringToSHA512(Password);
            }
            Account.Name = Name;
            Account.AccountContent = AccountContent;
            AccountModel.Update(Account);

            return RedirectToAction("UserSetting");
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult IndexSetting(string Title, string SubTitle)
        {
            string UserID = string.Empty;
            if (Session["UserID"] != null)
            {
                UserID = Session["UserID"].ToString();
            }
            int SN = AccountModel.getAccountByID(UserID).SN;
            Rio_UserIndexSetting userSetting = UserIndexSettingMode.getUserIndexSettingBySN(SN);
            userSetting.Title = Title;
            userSetting.SubTitle = SubTitle;
            UserIndexSettingMode.Update(userSetting);

            return RedirectToAction("UserSetting");
        }

        public ActionResult SelectCover()
        {
            string UserID = string.Empty;
            if (Session["UserID"] != null)
            {
                UserID = Session["UserID"].ToString();
            }
            ViewBag.Pic = PicModel.getUserPicEnableByID(UserID);

            return View();
        }

        [HttpPost]
        public ActionResult SelectCover(string type, int SN)
        {
            int accountSN = 0;
            int.TryParse(Session["UserSN"].ToString(), out accountSN);
            string userID = Session["UserID"].ToString() ?? string.Empty;

            if (type.Equals("Account"))
            {
                Rio_Account Account = AccountModel.getAccountByID(userID);
                Account.PicSN = SN;
                AccountModel.Update(Account);
            }
            else if (type.Equals("Index"))
            {
                Rio_UserIndexSetting userSetting = UserIndexSettingMode.getUserIndexSettingBySN(accountSN);
                userSetting.CoverSN = SN;
                UserIndexSettingMode.Update(userSetting);
            }
            return Content("Save Success");
        }
        #endregion

        //所有使用者連結
        public ActionResult AllUserLink(int? page)
        {            
            ViewBag.VwAlbumCount = AlbumModel.getUserVwAlbumList();
            
            var data = AccountModel.getAccountList();
            var pageNumeber = page ?? 1;
            var pageData = data.ToPagedList(pageNumeber, pageSize);

            return View(pageData);
        }

        #region 使用者追蹤功能
        public ActionResult userTrack(int? page)
        {
            if (Session["UserSN"] != null)
            { 
                int SN = 0;
                int.TryParse(Session["UserSN"].ToString(), out SN);
                var data = UserTrackModel.getUserTrackListBySN(SN);

                var pageNumeber = page ?? 1;
                var pageData = data.ToPagedList(pageNumeber, pageSize);
                ViewBag.userTrackList = pageData;
                return View(pageData);
            }
            return View();
        }

        [HttpPost]
        public ActionResult userTrack(string id)
        {            
            int clientSN = 0;
            int userSN = 0;
            int.TryParse(AccountModel.getAccountByID(id).SN.ToString(), out clientSN);
            int.TryParse(Session["UserSN"].ToString(), out userSN);

            Vw_UserTrack vw_track = UserTrackModel.getUserTrackBySN(userSN, clientSN);
            if (vw_track != null)
            {
                //delete
                UserTrackModel.Delete(vw_track.trackSN);
                return Content("false");
            }
            else
            {
                //insert
                Rio_UserTrack track = new Rio_UserTrack();
                track.AccountSN = userSN;
                track.TrackAccountSN = clientSN;
                
                UserTrackModel.Insert(track);
                return Content("true");
            }
            
        }
        #endregion
        
    }
}
