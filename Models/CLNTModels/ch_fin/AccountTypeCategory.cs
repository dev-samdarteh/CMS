using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class AccountTypeCategory
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [StringLength(100)]
        public string CategoryDesc { get; set; }  // 1 --

        [StringLength(5)]  
        public string CategoryCode { get; set; }  //  "INC1", Desc = "Receipts"  , "INC2", Desc = "Cost of Receipts" , "INC3", Desc = "Other Receipts" }

        [Display(Name = "Account Category")]
        public int? AccountCategoryId { get; set; }


        [StringLength(3)]  //[Range(1953, 9999)]
        public string AccountType  { get; set; }  // INC, EXP, FAS, CAS, CLB, ELB
        [StringLength(2)]
        public string ReportArea  { get; set; }  // IS - Income Statement, BS- Balance Sheet
        [StringLength(2)]
        public string BalanceType  { get; set; }  // DB, CR

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


        //        [ASSET = EQUITY + LIABILITY]
        //        INCOME... [SALES, COST OF SALES, OTHER INCOME], 
        //        EXPENSE... [EXPENSES, TAX, DIVIDENDS], 
        //        FIXED ASSETS... [INVESTMENTS, OTHER FIXED ASSETS]
        //        CURRENT ASSETS... [ACC RECEIVABLE, INVENTORY [Asset Register/Properties], CASH]
        //        CURRENT LIABILITIES... [ACC PAYABLE, OTHER CURR LIABILITIES]
        //        EQUITY... [RETAINED EARNINGS(OTHER {STARTING CAPITAL}, INC.SURPLUS), LONG-TERM BORROWINGS, OTHER LONG-TERM LIABILITIES]'




    }
}
