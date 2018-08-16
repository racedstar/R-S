using System;
using System.Collections.Generic;
using System.Linq;
using RioManager.Models;
using System.Web;
using System.Web.SessionState;

namespace RioManager.Views.Tools
{
    /// <summary>
    /// trackUser 的摘要描述
    /// </summary>
    public class trackUser : IHttpHandler ,IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if(context.Request.QueryString.Get("vid") != null && context.Session["UserSN"] != null)
            {
                string vid = context.Request.QueryString.Get("vid");
                int clientSN = 0;
                int userSN = 0;
                int.TryParse(new AccountModel().getAccountByID(vid).SN.ToString(), out clientSN);
                int.TryParse(context.Session["UserSN"].ToString(), out userSN);

                Vw_UserTrack vw_track = new UserTrackModel().getUserTrackBySN(userSN, clientSN);                
                if (vw_track != null)
                {
                    //delete
                    new UserTrackModel().Delete(vw_track.trackSN);
                    context.Response.Write(false);
                }
                else
                {
                    //insert
                    Rio_UserTrack track = new Rio_UserTrack();
                    track.AccountSN = userSN;
                    track.TrackAccountSN = clientSN;
                    context.Response.Write(true);

                    new UserTrackModel().Insert(track);
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