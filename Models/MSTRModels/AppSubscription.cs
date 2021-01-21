using RhemaCMS.Models.Adhoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class AppSubscription
    {
        public AppSubscription()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        //public int? ChurchLevelId { get; set; }

        [Required]
        [StringLength(100)]
        public string UserDesc { get; set; }
        [StringLength(30)]
        public string Username { get; set; }
        [StringLength(30)]
        public string Pwd { get; set; }
        public string SubscriberPhoneNum { get; set; }
        [Column("SLADesc")]
        [StringLength(200)]
        public string Sladesc { get; set; }
        public int? AppSubscriptionPackageId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? SubscriptionPeriod { get; set; }
        [StringLength(50)]
        public string SubscriptionKey { get; set; }  // LicenseKey
        public int? TotalSubscribed { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        [Column("STRT")]
        public DateTime? Strt { get; set; }
        [Column("EXPR")]
        public DateTime? Expr { get; set; }
        [Column("SLAStatus")]
        [StringLength(1)]
        public string Slastatus { get; set; }
        [StringLength(100)]
        public string StatusReason { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }



        [ForeignKey(nameof(AppGlobalOwnerId))] 
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual MSTRChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(AppSubscriptionPackageId))] 
        public virtual AppSubscriptionPackage AppSubscriptionPackage { get; set; }

        //[ForeignKey(nameof(ChurchLevelId))] 
        //[InverseProperty("AppSubscription_ChurchLevel")]
        //public virtual ChurchLevel ChurchLevel { get; set; } 

        public virtual List<SubscriptionChurchBody> SubscriptionChurchBody { get; set; }
    }
}
