using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class DonationTrans
    {
        [Key]
        public long Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? AccountPeriodId { get; set; }
        public int? CurrencyId { get; set; }
        [Display(Name = "Amount Equivalent")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AmtEquiv { get; set; }
        [Display(Name = "Date Donated")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DateDonated { get; set; }
        public int? ChurchAssetId { get; set; }
        public int? RelatedEventId { get; set; }
        [StringLength(1)]
        public string ChurchFellow { get; set; }   // Local, Denom, External
        public int? ChurchMemberId { get; set; }

        [StringLength(100)]
        public string DonatedByAdhoc { get; set; }

        [StringLength(3)]
        public string PmntMode { get; set; }   // CSH, CHQ, MMO
        [StringLength(15)]
        public string PmntModeRefNo { get; set; }   // CSH, CHQ, MMO
        public string PmntRefPers { get; set; }
        [Display(Name = "Date Posted")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? PostedDate { get; set; }
        public string PostStatus { get; set; }  //Open Posted Cancelled


        [StringLength(100)]
        public string Comments { get; set; }

        [StringLength(1)]
        public string Status { get; set; }  // A - Active, D - Deactive  

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        //
        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(AccountPeriodId))]
        public virtual AccountPeriod AccountPeriod { get; set; }

        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; } 

        [ForeignKey(nameof(RelatedEventId))]
        public virtual ChurchCalendarEvent RelatedEvent { get; set; }

        [ForeignKey(nameof(ChurchMemberId))]
        public virtual ChurchMember ChurchMember { get; set; } 

        [ForeignKey(nameof(ChurchAssetId))]
        public virtual ChurchAsset ChurchAsset{ get; set; }
    }
}







