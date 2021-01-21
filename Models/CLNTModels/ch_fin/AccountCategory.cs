using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class AccountCategory
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [StringLength(100)]
        public string CategoryDesc { get; set; }  // 1 --

        //[StringLength(5)]
        //public string CategoryCode { get; set; }  //  "INC1", Desc = "Receipts"  , "INC2", Desc = "Cost of Receipts" , "INC3", Desc = "Other Receipts" }

        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }

        [Display(Name = "Category Code")]
        [StringLength(9)]
      //  [RegularExpression("^[0-9]*$", ErrorMessage = "Account category code must be numeric")]
        public string CategoryCode { get; set; }  // 0000_0000   == parent code / category code

        [NotMapped]
        [Display(Name = "Parent Category Code")]
        [StringLength(4)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Parent category must be numeric")]
        public string strParentCategoryCode { get; set; }  // 0000_0000   == parent code / category code  ... INC, EXP, FAS, CAS, CLB, ELB ==> parent code: 0000_0001, 0000_0002, etc. Sales/Revenue: 0001_0011, Other Income/Revenue: 0001_0012, etc

        [NotMapped]
        [Display(Name = "Category Code")]
        [StringLength(4)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Account category code must be numeric")]
        public string strSubCategoryCode { get; set; }  // 0000_0000   == parent code / category code

        [Display(Name = "Account Type")]
        [StringLength(3)]  //[Range(1953, 9999)]
        public string AccountType { get; set; }  // INC, EXP, FAS, CAS, CLB, ELB

        [Display(Name = "Financial Statement")]
        [StringLength(2)]
        public string ReportArea { get; set; }  // IS - Income Statement, BS- Balance Sheet

        [Display(Name = "Balance Type")]
        [StringLength(2)]
        public string BalanceType { get; set; }  // DB, CR

        [StringLength(1)]
        public string Status { get; set; }  // A - Active, B - Block  

        [StringLength(3)]
        public string Owner { get; set; }  // SYS - System /Standard types -- use for Restore to Defaults, USR - User defined ... Create customized for Church Chart of Account, Generic Chart of Account at Setup***

        [StringLength(5)]
        public string OwnerKey { get; set; }  // null for USR-defined ... INC1, INC2, INC3... EXP1, EXP2, CAS1, CAS2, CAS3 ... "INC1", Desc = "Receipts"  , "INC2", Desc = "Cost of Receipts" , "INC3", Desc = "Other Receipts" }

        public int? LevelIndex { get; set; }

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
       
        [ForeignKey(nameof(ParentCategoryId))]
        public AccountCategory ParentAccountCategory { get; set; }



        //        [ASSET = EQUITY + LIABILITY]
        //        INCOME... [SALES, COST OF SALES, OTHER INCOME], 
        //        EXPENSE... [EXPENSES, TAX, DIVIDENDS], 
        //        FIXED ASSETS... [INVESTMENTS, OTHER FIXED ASSETS]
        //        CURRENT ASSETS... [ACC RECEIVABLE, INVENTORY [Asset Register/Properties], CASH]
        //        CURRENT LIABILITIES... [ACC PAYABLE, OTHER CURR LIABILITIES]
        //        EQUITY... [RETAINED EARNINGS(OTHER {STARTING CAPITAL}, INC.SURPLUS), LONG-TERM BORROWINGS, OTHER LONG-TERM LIABILITIES]'




    }
}
