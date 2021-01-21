using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class AccountMaster
    {
        [Key]
        public int Id { get; set; }
       
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [Display(Name = "Account Name")]
        [StringLength(100)]
        public string AccountName { get; set; }  // GL, Customer, Supplier

        
        [Display(Name = "Account code")]
        [StringLength(10)]
        //[MaxLength(12)]
        //[MinLength(1)]
        //[RegularExpression("^[0-9]*$", ErrorMessage = "Account number must be numeric")]
        public string AccountCode { get; set; }  // 1000_000   == Main /Sub

        [NotMapped]
        [Display(Name = "Main account code")]
        [StringLength(6)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account code/number must be numeric")]
        public string strMainAccountNo { get; set; }  // 100000-000   == Main /Sub
         
        [Display(Name = "Sub-account")]
        public bool IsSubAccount { get; set; } 

        [Display(Name = "Parent Account")] 
        public int? ParentAccountId { get; set; }  

        [Display(Name = "Account category")]
        public int? AccountCategoryId { get; set; }  // financial category... IsSubAccount = false [Main], ==> null
                
        //[Display(Name = "Sub account type")]
        //public int? AccountTypeCategoryId { get; set; }

        [NotMapped]
        [Display(Name = "Sub account code")]
        [StringLength(3)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account code/number must be numeric")]
        public string strSubAccountNo { get; set; }  // 100000-000   == Main /Sub

        [Display(Name = "Transaction Limit")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? PerTransLimit { get; set; }  // petty cash etc.

        [Display(Name = "Starting Period")]
        public int? StartAccountPeriodId { get; set; }

        [Display(Name = "Effective Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd, dd-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? StartingDate { get; set; }

        [Display(Name = "Opening Balance")]   //should post into the Account balances... at creation... for the specified period
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? OpenBal { get; set; }

        [StringLength(1)]
        public string Status { get; set; }  // A - Active, B - Blocked 


        [NotMapped]
        [Display(Name = "Budget Estimate")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? BudgetEst { get; set; }
 

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

        //[ForeignKey(nameof(AccountTypeCategoryId))]
        //public virtual AccountTypeCategory AccountTypeCategory { get; set; }

        [ForeignKey(nameof(AccountCategoryId))]
        public virtual AccountCategory AccountCategory { get; set; }

        [ForeignKey(nameof(ParentAccountId))]
        public virtual AccountMaster ParentAccount { get; set; }
    }
}


