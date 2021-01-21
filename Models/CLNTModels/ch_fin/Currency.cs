using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [StringLength(50)]
        public string CurrencyName { get; set; }  // 1 --

        [StringLength(3)]   
        public string Acronym { get; set; }  // GHS
        [StringLength(3)]
        public string SubAcronym { get; set; }  // GHP
        [StringLength(3)]
        public string Symbol { get; set; }  // GHC
        [StringLength(3)]
        public string SubSymbol { get; set; }  // P
        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }  // P
       // public int? CountryId { get; set; }

      //  [StringLength(1)]
        public bool IsBaseCurrency { get; set; }  //Y - N
        [Display(Name = "Base Rate")]
        [Column(TypeName = "decimal(18, 4)")]
        public decimal BaseRate { get; set; }  // How much of the base makes the currency e.g 5.70 GHC  == $ 1.00 

        [StringLength(1)]
        public string Status { get; set; }  // A - Active, D - Deactive  

        public int? OwnedByChurchBodyId { get; set; }
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

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [ForeignKey(nameof(CtryAlpha3Code))]
        public virtual Country Country { get; set; }

      
    }
}
