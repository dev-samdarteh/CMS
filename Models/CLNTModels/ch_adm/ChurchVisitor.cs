//using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RhemaCMS.Models.Adhoc;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchVisitor
    {
        [Key]
        public int Id { get; set; } 
        public int AppGlobalOwnerId { get; set; }
        public int ChurchBodyId { get; set; }
        public int OwnedByChurchBodyId { get; set; }
        ///
        public int? ChurchVisitorTypeId { get; set; }
        public int? ChurchActivityId { get; set; }
        public int? AgeBracketId { get; set; }
        [StringLength(100)]
        public string VisitReason { get; set; }
        [StringLength(1)]
        public string Vis_Status { get; set; }

        [StringLength(200)]
        public string Comments { get; set; }
        ///

        [StringLength(10)]
        public string Title { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string MiddleName { get; set; }
        [StringLength(30)]
        public string LastName { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        [Required]
        [StringLength(1)]
        public string MaritalStatus { get; set; }
         
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? FirstVisitDate { get; set; } 

        [StringLength(3)]
        public string NationalityId { get; set; }   //CtryAlpha3Code
        public int? PrimaryLanguageId { get; set; }
        [StringLength(100)]
        public string ResidenceAddress { get; set; }
        [StringLength(30)]
        public string Location { get; set; }
        [StringLength(50)]
        public string DigitalAddress { get; set; }
        public string MobilePhone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [StringLength(100)]
        public string Denomination { get; set; }
        public int? MSTR_FaithTypeCategoryId { get; set; }
       

        // public int? VisitReasonId { get; set; } 

        public bool? IsContactPersChurchFellow { get; set; }
        public int? ChurchContactId { get; set; }
        [StringLength(100)]
        public string ExtConChurchContactName { get; set; }
        [StringLength(50)]
        public string ExtConChurchContactLocation { get; set; }
        public string ExtConChurchContactMobilePhone  { get; set; }
        [EmailAddress]
        public string ExtConChurchContactEmail { get; set; }

        // [Column("VisitReason_Other")]

        //[StringLength(100)]
        //public string VisitReasonOther { get; set; }
        //  [Column("ChurchContactLocation_ExtCon")] 

        // [Column("ChurchContactMobilePhone_ExtCon")]

        //  [Column("ChurchContactName_ExtCon")] 

        //  [Column("VStatus")] 

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
        //[ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped]
       // [ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [ForeignKey(nameof(AgeBracketId))] 
        public virtual AppUtilityNVP AgeBracket { get; set; }

        [ForeignKey(nameof(ChurchActivityId))] 
        public virtual ChurchLifeActivity ChurchActivity { get; set; }
     
        [ForeignKey(nameof(ChurchContactId))]         
        public virtual ChurchMember ChurchContact { get; set; }

        [ForeignKey(nameof(ChurchVisitorTypeId))] 
        public virtual ChurchVisitorType ChurchVisitorType { get; set; }

        //[ForeignKey(nameof(FaithTypeCategoryId))] 
        //public virtual ChurchFaithType FaithTypeCategory { get; set; }

        [ForeignKey(nameof(NationalityId))] 
        public virtual Country Nationality { get; set; }

        [ForeignKey(nameof(PrimaryLanguageId))] 
        public virtual LanguageSpoken PrimaryLanguage { get; set; }

        //[ForeignKey(nameof(VisitReasonId))] 
        //public virtual AppUtilityNVP VisitReason { get; set; }

        [NotMapped]
        public string strMSTR_FaithTypeCategory { get; set; }
        [NotMapped]
        public string strFirstVisitDate { get; set; } 
        [NotMapped]
        public string strChurchVisitorType { get; set; }
        [NotMapped]
        public string strAgeBracket { get; set; }
        [NotMapped]
        public string strChurchActivity { get; set; }
        [NotMapped]
        public string strVis_Status { get; set; }
        [NotMapped]
        public string strPrimaryLanguage { get; set; }
        [NotMapped]
        public string strNationality { get; set; }
        [NotMapped]
        public string strContactPersName { get; set; }
        [NotMapped]
        public string strContactPersPhone { get; set; }
        [NotMapped]
        public string strContactPersLoc { get; set; }
        [NotMapped]
        public string strContactPersEmail { get; set; }
    }
}
