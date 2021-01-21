using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class ChurchRole
    {

        public ChurchRole()
        { }

        [Key]
        public int Id { get; set; }

        public int? AppGlobalOwnerId { get; set; }   // local copy synced
        public int? OwnedByChurchBodyId { get; set; }
        public int? ParentChurchBodyId { get; set; }

        public int? TargetChurchLevelId { get; set; }    // local copy synced

        [StringLength(2)]
        public string OrganisationType { get; set; }    // Church Root, [ GB--Government Body ], CU--Congregation Head-unit, CN--Congregation 

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Display(Name = "Church Code")]   // READ-ONLY --- GlobalChurchCode ::>>  [ RootChurchCode /Acronym ] - [7-digit] ... RCM0000000, RCM0000001, PCG1234567, COP1000000, ICGC9999999
        [StringLength(20)]
        public string GlobalChurchCode { get; set; }

        [Display(Name = "Relative Church Code")]
        [StringLength(200)]
        public string RootChurchCode { get; set; }  // READ ONLY --- ChurchCodeFullPath ::>> RCM-000001--RCM-000001--RCM-000001--RCM-000001
                                                    //[Display(Name = "Custom Code")]
                                                    //[StringLength(20)]
                                                    //public string ChurchCodeCustom { get; set; }
 
        public int? RankIndex { get; set; }  // Grade the CP's  ... Minister is higher than Catechist, then Presyter, then Group Presidents...
        public bool ApplyToClergyOnly { get; set; }    // CP :-- Minister, Catechist  
        public int? MaxNumAllowed { get; set; }  // --- CP ... President - 1, Presbyter - 13
        public int? MinNumAllowed { get; set; }    // ---  CP ... President - 1, Presbyter - 5
            
        [StringLength(200)]
        public string PrimaryFunction { get; set; }   // brief func of position

        [StringLength(100)]
        public string Comments { get; set; }
        public bool? IsActivated { get; set; }

      
        [StringLength(1)]
        public string SharingStatus { get; set; }   // N-Not shared C-shared with Child CB unit only A-Shared with all  per parent body

        [StringLength(1)]
        public string ChurchWorkStatus { get; set; }   // Operationalized - O, Structure only - S   ... Directors [structure only], District Evangelism Coordinators [Operationalized]

        [StringLength(1)]
        public string OwnershipStatus { get; set; }  // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }




        [NotMapped] //[ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }

        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ParentChurchBodyId))]
        public virtual ChurchBody ParentChurchBody { get; set; }

        [ForeignKey(nameof(OwnedByChurchBodyId))]
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [ForeignKey(nameof(TargetChurchLevelId))]
        //[InverseProperty("OwnedByChurchBody_CL")]
        public virtual ChurchLevel TargetChurchLevel { get; set; }

         
        [NotMapped]
        public string strOwnerChurchBody { get; set; }
        [NotMapped]
        public string strAppGlobalOwn { get; set; }
        [NotMapped]
        public string strParentChurchBody { get; set; }
      
        [NotMapped]
        public string strOrgType { get; set; }
        [NotMapped]
        public string strTargetChurchLevel { get; set; }
        [NotMapped]
        public string strChurchWorkStatus { get; set; }


        [NotMapped]
        public string strStatus { get; set; }
        [NotMapped]
        public string strSharingStatus { get; set; }

        [NotMapped]
        public string strOwnershipStatus { get; set; }   // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody


    }
}
