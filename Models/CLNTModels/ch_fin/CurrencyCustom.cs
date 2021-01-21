using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class CurrencyCustom
    {
        public CurrencyCustom()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }   // get the currency details via... Country

        public bool IsDisplay { get; set; }         // subject to the local cong
        public bool IsDefaultCurrency { get; set; }  // the most used country
       // public bool IsChurchCurrency { get; set; }  // subject to the AGO regions of operation
         
        public bool IsBaseCurrency { get; set; }  //Y - N

        [Display(Name = "Base Rate")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal BaseRate { get; set; }  // How much of the base makes the currency e.g 5.70 GHC  == $ 1.00 

        //[StringLength(1)]
        //public string Status { get; set; }  // A - Active, D - Deactive  


        // public int? OwnedByChurchBodyId { get; set; }   // the church unit that configured

        //[StringLength(1)]
        //public string SharingStatus { get; set; }


        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped] //[ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped] //[ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }

        //[NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        //public virtual ChurchBody OwnedByChurchBody { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }


        [ForeignKey(nameof(CtryAlpha3Code))]
        public virtual Country Country { get; set; }




        [NotMapped]
        public string strCountry { get; set; }

    }
}

