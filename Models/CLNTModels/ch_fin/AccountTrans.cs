using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class AccountTrans
    {
        [Key]
        public long Id { get; set; }
        [StringLength(10)]
        public string AccountNo { get; set; }  // 100000/000   == Main /Sub
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? AccountPeriodId { get; set; }
        public int? AccountId { get; set; }
        public int? ContraAccountId { get; set; }
        public int? CurrencyId { get; set; }
        public int? RelatedEventId { get; set; }
        //  public string TransByUserId { get; set; }
        [Display(Name = "Trans Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? TransDate { get; set; }
        [StringLength(100)]
        public string TransDesc { get; set; }
        [Display(Name = "Amount")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TransAmt { get; set; }  // petty cash etc.
        [StringLength(2)]
        public string TransType { get; set; }   //DB ... CR
        [StringLength(3)]
        public string PmntMode { get; set; }   // CSH, CHQ, MMO
        [StringLength(15)]
        public string PmntModeRefNo { get; set; }   // CSH, CHQ, MMO
        public string PmntRefPers { get; set; }

        [StringLength(3)]
        public string PostType { get; set; }   // [MAN]ual [AUT]o
        public string PostRef { get; set; }
        [Display(Name = "Date Posted")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? PostDate { get; set; } 
        public long? DblEntryRef { get; set; }    


        [StringLength(1)]
        public string TransactStatus { get; set; }  // R-eversed, A-djusted, P-osted 
        [StringLength(1)]
        public string Status { get; set; }  // A - Active, D - Deactive  
        public string Comments { get; set; }  

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

        [ForeignKey(nameof(AccountId))]
        public virtual AccountMaster AccountMain { get; set; }  
        
        [ForeignKey(nameof(ContraAccountId))]
        public virtual AccountMaster ContraAccount { get; set; } 
        
        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }    

        [ForeignKey(nameof(RelatedEventId))]
        public virtual ChurchCalendarEvent RelatedEvent { get; set; }
    }
}





