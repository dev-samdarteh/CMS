using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class AccountBalance
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        [Display(Name = "Account Year")]
        public int? AccountYearId { get; set; }

        [Display(Name = "Account Period")]
        public int? AccountPeriodId { get; set; }

        [Display(Name = "Account")]
        public int? AccountId { get; set; }

        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }

        //[Display(Name = "Date Recorded")]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        //public DateTime? DateRec { get; set; }

        [Display(Name = "Opening Balance")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? OpenBal { get; set; } //closing bal for previous period, or at starting amt @ creation of account

        [Display(Name = "Closing Balance")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? ClosingBal { get; set; }  // only when period closes


        [Display(Name = "Budget Estimate")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BudgetEst { get; set; } // budget for the period or the year [sum of period budgets for all |A-B-C| accounts]

        [NotMapped]
        [Display(Name = "Balance as Of")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? CurrentBal { get; set; }  // ie. Open Bal + [Current period bal]

        [NotMapped]
        [Display(Name = "Variance")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AccountVariance { get; set; }


        [StringLength(1)]
        public string Status { get; set; }  // |A-B-C|  ...  A-Active, B-Blocked, C-Closed  

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
       
        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }


        [ForeignKey(nameof(AccountId))]
        public virtual AccountMaster Account { get; set; }

        [ForeignKey(nameof(AccountYearId))]
        public virtual AccountYear AccountYear { get; set; }

        [ForeignKey(nameof(AccountPeriodId))]
        public virtual AccountPeriod AccountPeriod { get; set; }

    }
}

