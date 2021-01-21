using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class MemberChurchRole
    {
        public MemberChurchRole()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }
        ///
        public int? ChurchMemberId { get; set; } 
        
        [StringLength(2)]
        public string OrganisationType { get; set; }    // role can be... church body [CR, GB, CN, CH, CO, CE, IB] or church sector [DP, SC, CG]
        public bool IsRoleUnitChurchSector { get; set; } 
        public int? ChurchSectorUnitId { get; set; }
        public int? ChurchBodyUnitId { get; set; }
        public int? LeaderRoleId { get; set; }  // filter role based on selected Church sector/body
        public bool IsCoreRole { get; set; }
        public bool IsCurrentRole { get; set; }

        [StringLength(10)]
        public string BatchCode { get; set; }   // thus... batch of leaders e.g. 2020 Session, 2019 Executive Council etc.

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Commenced { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Completed { get; set; }

        [StringLength(50)]
        public string CompletionReason { get; set; }

        [StringLength(200)]
        public string RoleProfile { get; set; } 

        //[Required]
        //public string Discriminator { get; set; }
        //  public int? AttachedChurchBodyId { get; set; }
         

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }
         

        [ForeignKey(nameof(ChurchMemberId))] 
        public virtual ChurchMember ChurchMember { get; set; }

        [ForeignKey(nameof(ChurchSectorUnitId))] 
        public virtual ChurchBody ChurchSectorUnit { get; set; }

        [ForeignKey(nameof(ChurchBodyUnitId))] 
        public virtual ChurchBody ChurchBodyUnit { get; set; }

        [ForeignKey(nameof(LeaderRoleId))] 
        public virtual LeaderRole LeaderRole { get; set; } 

        [NotMapped]
        public string strLeaderRole { get; set; } 
        [NotMapped]
        public string strRoleUnit { get; set; } 
        [NotMapped]
        public string strCommenced { get; set; } 
        [NotMapped]
        public string strCompleted { get; set; } 


        //public virtual ApprovalActionStep ApprovalActionStepActionBy { get; set; } 
        //public virtual ApprovalActionStep ApprovalActionStepAction { get; set; }
        //public virtual ChurchTransfer ChurchTransfer_MemberChurchRole { get; set; }
        //public virtual ChurchTransfer ChurchTransfer_RequestorRole { get; set; }

        //public virtual List<ApprovalActionStep> ApprovalActionStepActionByMemberChurchRoles { get; set; } 
        //public virtual List<ApprovalActionStep> ApprovalActionStepMemberChurchRoles { get; set; } 
        //public virtual List<ChurchTransfer> ChurchTransferFromMemberChurchRoles { get; set; } 
        //public virtual List<ChurchTransfer> ChurchTransferRequestorRoles { get; set; } 
        //public virtual List<EventActivityReqLog> EventActivityReqLogs { get; set; } 
        //public virtual List<MemberChurchLifeActivity> MemberChurchLifeActivities { get; set; }
    }
}
