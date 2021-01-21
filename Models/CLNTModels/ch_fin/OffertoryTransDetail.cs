using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class OffertoryTransDetail
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [Display(Name = "Reference #")]
        public long? OffertoryTransId { get; set; }

        [Display(Name = "Payment Mode")]
        [StringLength(4)]
        public string PaymentMode { get; set; }   // BNK, CSH, CHQ, MMO, POS, OTH

        [Display(Name = "PayMode Carrier")]
        [StringLength(100)]
        public string PayModeCarrier { get; set; }  // Bank name e.g. Nima GCB Bank Ltd, Mobile telco e.g. MTN, POS name

        [Display(Name = "PayMode Trans Date")]    // cheque date, POS trx date, Momo trx date etc.
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd, dd-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? PayModeTransDate { get; set; }

        [Display(Name = "Payment Mode Number")]
        [StringLength(15)]
        public string PmntModeRefNo { get; set; }  // cheque number...

        [Display(Name = "Payment Mode Amount")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PayModeAmt { get; set; }

        [Display(Name = "Currency")]
        public int? CurrencyId { get; set; }

        [Display(Name = "Recipient Name")]
        [StringLength(100)]
        public string PayToName { get; set; }
              

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

        [ForeignKey(nameof(OffertoryTransId))]
        public virtual OffertoryTrans OffertoryTrans { get; set; }
    }
}





