using RhemaCMS.Models.MSTRModels;
using System; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class CountryRegion
    {
        public CountryRegion()
        { }

        /// <summary>
        /// Country region possibly MUST be created at the TOP hierarchy and synched down... 
        /// Temporal [non-active] profile may be created by VENDOR to do this until subuscription is active at the TOP level of Church -- Not Operationalized --- and after deactivate Top CB
        /// Church Body may customize as Display or NOT Display ...
        /// </summary>
        
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [Required]
        public string Name { get; set; }
      //  public int? CountryId { get; set; }

        [StringLength(3)]  
        public String CtryAlpha3Code { get; set; }   // GHA, USA etc use this as key standard globally

        [StringLength(5)]
        public string RegCode { get; set; }
       
        [StringLength(1)]
        public string SharingStatus { get; set; }

        public int? OwnedByChurchBodyId { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped] // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] //[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(CtryAlpha3Code))] 
        public virtual Country Country { get; set; }

        
        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        //[InverseProperty("CountryRegion")]
        //public virtual List<ChurchBody> ChurchBodies { get; set; }

        //[InverseProperty("CountryRegion")]
        //public virtual List<ContactInfo> ContactInfos { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }



        //[NotMapped]
        //public Country oCountry { get; set; }

        [NotMapped]
        public string strCountry { get; set; }
    }
}
