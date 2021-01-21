using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class MemberContact
    {
        [Key]
        public int Id { get; set; }

        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }
        public int? ChurchMemberId { get; set; }    
        ///
        public bool IsCurrentContact { get; set; }   
        public bool IsChurchFellow { get; set; }       
        public int? InternalContactId { get; set; }   
        public int? RelationshipId { get; set; }
        ///
        
        [StringLength(100)]
        public string ExtConContactName { get; set; }   
        [StringLength(50)]
        public string CityExtCon { get; set; }    
      //  public int? ExtConCountryId { get; set; }  
        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }  
        public int? ExtConRegionId { get; set; }
        [StringLength(100)]
        public string ExtConDenomination { get; set; }
        [StringLength(50)]
        public string ExtConFaithCategory { get; set; }
        [StringLength(50)]
        public string ExtConDigitalAddress { get; set; }   
        [StringLength(50)]
        public string ExtConLocation { get; set; }   
        [StringLength(15)]
        public string ExtConMobilePhone { get; set; }
        [StringLength(50)][EmailAddress]
        public string ExtConEmail { get; set; }       
        [StringLength(100)]
        public string ExtConResidenceAddress { get; set; }
        public bool ExtConResAddrSameAsPostAddr { get; set; }
        [StringLength(100)]
        public string ExtConPostalAddress { get; set; }
        

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }



        [ForeignKey(nameof(ChurchMemberId))] 
        public virtual ChurchMember ChurchMember { get; set; }

        [ForeignKey(nameof(CtryAlpha3Code))] 
        public virtual Country ExtConCountry { get; set; }

        //[ForeignKey(nameof(FaithTypeCategoryIdExtCon))] 
        //public virtual ChurchFaithType FaithTypeCategoryIdExtConNavigation { get; set; }

        [ForeignKey(nameof(InternalContactId))] 
        public virtual ChurchMember InternalContact { get; set; }

        [ForeignKey(nameof(ExtConRegionId))] 
        public virtual CountryRegion ExtConRegion { get; set; }

        [ForeignKey(nameof(RelationshipId))] 
        public virtual RelationshipType RelationshipType { get; set; }


        [NotMapped]
        public string strFaithTypeCategory { get; set; }
        [NotMapped]
        public string strContactName { get; set; }
        [NotMapped]
        public string strLocDesc { get; set; }   // City, Country
        [NotMapped]
        public string strRelationshipType { get; set; }

    }
}
