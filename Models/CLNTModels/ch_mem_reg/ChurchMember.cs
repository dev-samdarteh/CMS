using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchMember
    {
        public ChurchMember()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }
        public int? ChurchBodyId { get; set; }
        ///

        [StringLength(50)]
        public string MemberCode { get; set; }  // RCM0000001 :- Member Global Code -- same across denomination... to allow transfers and retaining of member data

        [Display(Name = "Relative Member Code")]
        [StringLength(200)]
        public string RootChurchCode { get; set; }  // READ ONLY --- ChurchCodeFullPath ::>> RCM-000001--RCM-000001--RCM-000001--RCM-000001--M0000001

        [StringLength(10)]
        public string Title { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string MiddleName { get; set; }
        [StringLength(30)]
        public string LastName { get; set; }
        [StringLength(100)]
        public string MaidenName { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? DateOfBirth { get; set; }

       // [Required]
        [StringLength(1)]
        public string MaritalStatus { get; set; }
        [StringLength(1)]
        public string MarriageType { get; set; }
       // public int? NationalityId { get; set; }

        [StringLength(3)]
        public string NationalityId { get; set; }  //CtryAlpha3Code e.g. GHA
        public int? ContactInfoId { get; set; }
        [StringLength(50)]
        public string Hobbies { get; set; }
        [StringLength(30)]
        public string Hometown { get; set; }
        public int? HometownRegionId { get; set; }
        // [Column("IDTypeId")]
        public int? IdTypeId { get; set; }
        public string PhotoUrl { get; set; }
        [StringLength(300)]
        public string OtherInfo { get; set; } 
        //[Required]
        //public string Discriminator { get; set; } 
        public int? MotherTongueId { get; set; }
        //[Column("NationalIDNum")]
        [StringLength(30)]
        public string National_IdNum { get; set; }
        public bool IsActivated { get; set; } 
        [StringLength(50)]
        public string MarriageRegNo { get; set; }
        [StringLength(50)]
        public string MemberGlobalId { get; set; }
        public int? MemberTypeId { get; set; }
        [StringLength(50)]
        public string MemberCustomCode { get; set; }
        [StringLength(1)]
        public string MemberClass { get; set; }

        [StringLength(1)]
        public string Status { get; set; } // A - Active, B-??, D-Deactive

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped] //[ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }

       [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }
         
        
        [ForeignKey(nameof(HometownRegionId))] 
        public virtual CountryRegion HometownRegion { get; set; }

        [ForeignKey(nameof(IdTypeId))] 
        public virtual National_IdType IdType { get; set; }

        [ForeignKey(nameof(MotherTongueId))] 
        public virtual LanguageSpoken MotherTongue { get; set; }

        [ForeignKey(nameof(NationalityId))] 
        public virtual Country Nationality { get; set; } 


        [NotMapped]
        public string  strMemberName { get; set; } 


        //[NotMapped]
        //public virtual List<ChurchAttendAttendees> ChurchAttendAttendees { get; set; }
        //[NotMapped]
        //public virtual List<ChurchBodyAssociate> ChurchBodyAssociates { get; set; }
        //[NotMapped]
        //public virtual List<ChurchEventActor> ChurchEventActors { get; set; }

        //[NotMapped] //  [InverseProperty("ChurchMember")]
        //public virtual List<ChurchTransfer> ChurchTransferChurchMembers { get; set; }
        //[NotMapped] // [InverseProperty("RequestorMember")]
        //public virtual List<ChurchTransfer> ChurchTransferRequestorMembers { get; set; }
        //[NotMapped]
        //public virtual List<ChurchVisitor> ChurchVisitors { get; set; }
        //[NotMapped]
        //public virtual List<ContactInfo> ContactInfos { get; set; }
        //[NotMapped]
        //public virtual List<EventActivityReqLog> EventActivityReqLogs { get; set; }
        //[NotMapped]
        //public virtual List<MemberChurchLife> MemberChurchLifes { get; set; }
        //[NotMapped]
        //public virtual List<MemberChurchLifeActivity> MemberChurchLifeActivities { get; set; }
        //[NotMapped]
        //public virtual List<MemberChurchSector> MemberChurchSectors { get; set; }
        //[NotMapped]
        //public virtual List<MemberChurchUnit> MemberChurchUnits { get; set; }
        //[NotMapped]  // [InverseProperty("ChurchMember")]
        //public virtual List<MemberContact> MemberContactChurchMembers { get; set; }
        //[NotMapped] // [InverseProperty("InternalContact")]
        //public virtual List<MemberContact> MemberContactInternalContacts { get; set; }
        //[NotMapped]
        //public virtual List<MemberEducHistory> MemberEducHistories { get; set; }
        //[NotMapped]
        //public virtual List<MemberLanguageSpoken> MemberLanguagesSpoken { get; set; }
        //[NotMapped]
        //public virtual List<MemberChurchRole> MemberChurchRoles { get; set; }
        //[NotMapped]
        //public virtual List<MemberPosition> MemberPositions { get; set; }
        //[NotMapped]
        //public virtual List<MemberProfessionBrand> MemberProfessionBrands { get; set; }
        //[NotMapped]
        //public virtual List<MemberRank> MemberRanks { get; set; }
        //[NotMapped]
        //public virtual List<MemberRegistration> MemberRegistrations { get; set; }
        //[NotMapped]   // [InverseProperty("ChurchMember")]
        //public virtual List<MemberRelation> MemberRelationChurchMembers { get; set; }
        //[NotMapped] // [InverseProperty("RelationChurchMember")]
        //public virtual List<MemberRelation> MemberRelationRelationChurchMembers { get; set; }
        //[NotMapped]
        //public virtual List<MemberStatus> MemberStatuses { get; set; }
        //[NotMapped]
        //public virtual List<MemberType> MemberTypes { get; set; }
        //[NotMapped]
        //public virtual List<MemberWorkExperience> MemberWorkExperiences { get; set; }
    }
}
