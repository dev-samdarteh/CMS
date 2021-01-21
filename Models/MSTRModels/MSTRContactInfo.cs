   
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class MSTRContactInfo
    {
        public MSTRContactInfo()
        { }

        [Key]
        public int Id { get; set; }
       
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        //public bool ChurchFellow { get; set; }
        //public int? ChurchMemberId { get; set; }
                
        public int? RefUserId { get; set; }

        [StringLength(100)]
        public string ContactName { get; set; }
        [StringLength(100)]
        public string ResidenceAddress { get; set; }
        [StringLength(30)]
        public string Location { get; set; }
        [StringLength(30)]
        public string City { get; set; }
        public int? RegionId { get; set; }
        // public int? CountryId { get; set; }
        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }
        public bool ResAddrSameAsPostAddr { get; set; }
        [StringLength(30)]
        public string PostalAddress { get; set; }
        [StringLength(30)]
        public string DigitalAddress { get; set; }
        [StringLength(15)]
        public string Telephone { get; set; }
        [StringLength(15)]
        public string MobilePhone1 { get; set; }
        [StringLength(15)]
        public string MobilePhone2 { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }



        [NotMapped] // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] //[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [NotMapped] //[ForeignKey(nameof(ChurchBodyId))]
        public virtual MSTRChurchBody ChurchBody { get; set; }

        [NotMapped]
       // [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }

        //[ForeignKey(nameof(ChurchMemberId))] 
        //[NotMapped]
        //public virtual ChurchMember ChurchMember { get; set; }

        [ForeignKey(nameof(CtryAlpha3Code))]
        public virtual MSTRCountry Country { get; set; }
        [ForeignKey(nameof(RegionId))]
        public virtual MSTRCountryRegion CountryRegion { get; set; }

        //[InverseProperty("ContactInfo")]
        //public virtual List<ChurchBody> ChurchBodies { get; set; }
    }
}
