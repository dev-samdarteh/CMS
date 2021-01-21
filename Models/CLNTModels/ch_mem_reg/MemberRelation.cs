using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class MemberRelation
    {
        [Key]
        public int Id { get; set; }        
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }
        ///
        public int ChurchMemberId { get; set; }

        [Required]
        [StringLength(1)]
        public string RelationType { get; set; }
        [StringLength(1)]
        public string ChurchFellow { get; set; }
        public int? RelationChurchMemberId { get; set; }
        public int? ExternalNonMemAssociateId { get; set; }
        public int? RelationshipId { get; set; }
        [StringLength(1)]
        public string Status { get; set; } 
       // [Column("RegionId_ExtCon")]
        public int? RegionIdExtCon { get; set; }
        public bool IsNextOfKin { get; set; }

      //  [Column("ResAddrSameAsPostAddr_ExtCon")]
        public bool ResAddrSameAsPostAddrExtCon { get; set; }
       // [Column("City_ExtCon")]
        [StringLength(30)]
        public string CityExtCon { get; set; }
      //  [Column("ContactName_ExtCon")]
        [StringLength(100)]
        public string ContactNameExtCon { get; set; }
      //  [Column("CountryId_ExtCon")]
      //  public int? CountryIdExtCon { get; set; }

        [StringLength(3)]
        public string CtryAlpha3CodeExtCon { get; set; }
     //   [Column("Denomination_ExtCon")]
        [StringLength(100)]
        public string DenominationExtCon { get; set; }
     //   [Column("DigitalAddress_ExtCon")]
        [StringLength(30)]
        public string DigitalAddressExtCon { get; set; }
     //   [Column("Email_ExtCon")]
        public string EmailExtCon { get; set; }
     //   [Column("FaithTypeCategoryId_ExtCon")]
        public int? FaithTypeCategoryIdExtCon { get; set; }
     //   [Column("Location_ExtCon")]
        [StringLength(30)]
        public string LocationExtCon { get; set; }
     //   [Column("MobilePhone_ExtCon")]
        [StringLength(15)]
        public string MobilePhoneExtCon { get; set; }
     //   [Column("PostalAddress_ExtCon")]
        [StringLength(30)]
        public string PostalAddressExtCon { get; set; }
      //  [Column("ResidenceAddress_ExtCon")]
        [StringLength(100)]
        public string ResidenceAddressExtCon { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]//  [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }
        

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        public virtual ChurchMember ChurchMember { get; set; }
        [ForeignKey(nameof(CtryAlpha3CodeExtCon))] 
        public virtual Country CountryExtCon { get; set; }

        //[ForeignKey(nameof(FaithTypeCategoryIdExtCon))] 
        //public virtual ChurchFaithType FaithTypeCategoryIdExtConNavigation { get; set; }

        [ForeignKey(nameof(RegionIdExtCon))] 
        public virtual CountryRegion RegionExtCon { get; set; }
        [ForeignKey(nameof(RelationChurchMemberId))] 
        public virtual ChurchMember RelationChurchMember { get; set; }
        [ForeignKey(nameof(RelationshipId))] 
        public virtual RelationshipType Relationship { get; set; }

        [NotMapped]
        public string strRelationDesc { get; set; }
        [NotMapped]
        public string strRelationship { get; set; }
        [NotMapped]
        public string strFaithTypeCategory { get; set; }
        [NotMapped]
        public string strRelationLocation { get; set; }

    }
}
