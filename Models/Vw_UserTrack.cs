//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace RioManager.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vw_UserTrack
    {
        public Nullable<int> SN { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string AccountContent { get; set; }
        public Nullable<bool> IsEnable { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<int> PicSN { get; set; }
        public string PicPath { get; set; }
        public string PicName { get; set; }
        public int trackSN { get; set; }
        public int AccountSN { get; set; }
        public int TrackAccountSN { get; set; }
    }
}
