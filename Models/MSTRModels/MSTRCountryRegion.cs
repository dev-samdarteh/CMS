 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class MSTRCountryRegion
    {
        public MSTRCountryRegion()
        { }

        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
       // public int? CountryId { get; set; }

        [StringLength(3)]
        public String CtryAlpha3Code { get; set; }

        [StringLength(3)]
        public string RegCode { get; set; }
      //  public int? OwnedByChurchBodyId { get; set; }
        [StringLength(1)]
        public string SharingStatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped] // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] //[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(CtryAlpha3Code))]
        public virtual MSTRCountry Country { get; set; }


       
    //   [NotMapped]  // [ForeignKey(nameof(OwnedByChurchBodyId))]
     //   public virtual MSTRChurchBody OwnedByChurchBody { get; set; }

        //[InverseProperty("CountryRegion")]
        //public virtual List<ChurchBody> ChurchBodies { get; set; }

        //[InverseProperty("CountryRegion")]
        //public virtual List<ContactInfo> ContactInfos { get; set; }



        [NotMapped]
        public MSTRCountry oCountry { get; set; }

        [NotMapped]
        public string strCountry { get; set; }
    }
}
