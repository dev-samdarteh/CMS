 // using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class Country
    {
        public Country()
        { }

        //[Key]
        //public int Id { get; set; }

        [Key]
        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }

        [StringLength(3)]
        public string CtryAlpha2Code { get; set; }

        // public int? AppGlobalOwnerId { get; set; }
        // public int? ChurchBodyId { get; set; }               


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
        public string CurrSymbol { get; set; }  // symbol varry in chars
        [StringLength(3)]
        public string Curr3LISOSymbol { get; set; }

        //public bool IsDisplay { get; set; }         // subject to the local cong
        //public bool IsDefaultCountry { get; set; }  // the most used country
        //public bool IsChurchCountry { get; set; }  // subject to the AGO regions of operation

        //public int? OwnedByChurchBodyId { get; set; }

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

        //[ForeignKey(nameof(AppGlobalOwnerId))]
        //public virtual  AppGlobalOwner CountryAppGlobalOwner { get; set; }

        //[ForeignKey(nameof(ChurchBodyId))]
        //public virtual ChurchBody ChurchBody { get; set; }

        //[InverseProperty("Country")]
        //public virtual List<AppGlobalOwner> AppGlobalOwners { get; set; } 
        //[InverseProperty("Country")]
        //public virtual List<ChurchBody> ChurchBodies { get; set; }
        //[InverseProperty("Country")]
        //public virtual List<ContactInfo> ContactInfos { get; set; }

       [NotMapped]  // [InverseProperty("Country")]
        public virtual List<CountryRegion> CountryRegions { get; set; }
    }
}
