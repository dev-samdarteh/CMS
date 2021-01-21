  
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class MSTRCountry
    {
        public MSTRCountry()
        { }

        //[Key]
        //public int Id { get; set; }

        [Key]
        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }

        [StringLength(3)]
        public string CtryAlpha2Code { get; set; }

        ////[Required]
        [StringLength(100)]
        public string EngName { get; set; }

        //[StringLength(3)]
        //public string Acronym { get; set; }

        [StringLength(50)]
        public string CurrEngName { get; set; }
        [StringLength(50)]
        public string CurrLocName { get; set; }
        [StringLength(15)]
        public string CurrSymbol { get; set; }
        [StringLength(3)]
        public string Curr3LISOSymbol { get; set; }


        //[Required]
        //[StringLength(100)]
        //public string Name { get; set; }

        //[StringLength(3)]
        //public string Acronym { get; set; }
        //[StringLength(3)]
        //public string Currency { get; set; }
       // public bool Display { get; set; }
      //  public bool DefaultCtry { get; set; }
      //  public int? OwnedByChurchBodyId { get; set; }


        //[StringLength(1)]
        //public string SharingStatus { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped] //[ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] //[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


       // [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))]
      //  public virtual MSTRChurchBody OwnedByChurchBody { get; set; }

      //  public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        //[InverseProperty("Country")]
        //public virtual List<AppGlobalOwner> AppGlobalOwners { get; set; } 
        //[InverseProperty("Country")]
        //public virtual List<ChurchBody> ChurchBodies { get; set; }
        //[InverseProperty("Country")]
        //public virtual List<ContactInfo> ContactInfos { get; set; }


        [NotMapped] //[InverseProperty("Country")]
        public virtual List<MSTRCountryRegion> CountryRegions { get; set; }


    }
}
