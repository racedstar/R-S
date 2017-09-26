using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RioManager.Models;

namespace RioManager.Controllers
{
    public class ToolsController : Controller
    {
        // GET: Tools
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult fmUpload()
        {
            string UploadType = string.Empty;
            string Extension = string.Empty;
            if (HttpContext.Request.QueryString.Get("t") != null)
            {
                UploadType = HttpContext.Request.QueryString.Get("t").ToString();
            }

            if (UploadType.Equals("img"))
            {
                Extension = RioManager.App_Code.WebConfig.UploadPicType;
            }
            else if (UploadType.Equals("Doc"))
            {
                Extension = RioManager.App_Code.WebConfig.UploadDocumentType;
            }

            ViewBag.UploadType = Extension;

            return View();
        }

        public ActionResult ConvertText()
        {        

            return View();
        }

        [HttpPost]
        public ActionResult ConvertText(string input, string coding)
        {       
            if (input != null)
            {
                if (!input.Equals(string.Empty))
                {
                    if (coding == "Encrypt")
                        ViewBag.output = App_Code.Coding.Encrypt(input);
                    else if (coding == "Decrypt")
                        ViewBag.output = App_Code.Coding.Decrypt(input);
                }
            }
            return View();
        }
    }
}