﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Rio_Account> Rio_Account { get; set; }
        public virtual DbSet<Rio_Album> Rio_Album { get; set; }
        public virtual DbSet<Rio_AlbumJoinPic> Rio_AlbumJoinPic { get; set; }
        public virtual DbSet<Rio_Doc> Rio_Doc { get; set; }
        public virtual DbSet<Rio_Pic> Rio_Pic { get; set; }
        public virtual DbSet<Rio_SystemList> Rio_SystemList { get; set; }
        public virtual DbSet<Vw_Account> Vw_Account { get; set; }
        public virtual DbSet<Vw_Album> Vw_Album { get; set; }
        public virtual DbSet<Vw_AlbumJoinPic> Vw_AlbumJoinPic { get; set; }
        public virtual DbSet<Rio_UserIndexSetting> Rio_UserIndexSetting { get; set; }
        public virtual DbSet<Vw_UserIndexSetting> Vw_UserIndexSetting { get; set; }
    }
}
