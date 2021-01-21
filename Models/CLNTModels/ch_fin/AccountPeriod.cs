using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class AccountPeriod
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }

        public int PeriodIndex { get; set; }  // 1 --
        [StringLength(2)]  //[Range(1953, 9999)]
        public string PeriodCode { get; set; }  // 01
        [StringLength(15)]
        public string PeriodDesc { get; set; }  //Year 1 --

        public int? ChurchPeriodId { get; set; }     // PeriodType = "CY" AccountYearId

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? EndDate { get; set; }

        public int LengthInDays { get; set; }   // Church Year (365) ... = End Date - Start Date
        [StringLength(2)]
        public string PeriodType { get; set; }  // AY - Accout Year, Church Year - CY        

        [StringLength(1)]
        public string PeriodStatus { get; set; }  //Status = Open (O), Blocked (B), Closed (C)        /////  C-Current [Active], B - Blocked [Active], P - Previous [Closed], X-Closed [History]  
        [StringLength(1)]
        public string LongevityStatus { get; set; }  //  Longevity = Current (A), Previous (P), History (H) 
        [StringLength(1)]
        public string SharingStatus { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        //
        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }
         
        [ForeignKey(nameof(ChurchPeriodId))]
        public virtual ChurchPeriod ChurchPeriod { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }

    }
}


 