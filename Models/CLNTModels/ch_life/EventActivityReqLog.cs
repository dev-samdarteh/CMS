//using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class EventActivityReqLog
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }
        ///
        public int? ChurchMemberId { get; set; }
        public int ChurchCalendarId { get; set; }
        public int? ChurchEventId { get; set; }
        public int? RequirementDefId { get; set; }
        public int? MemberChurchRoleId { get; set; }  // tie responsibility to person-role [ cos he/she will be performing some function ]
        [StringLength(1)]
        public string Status { get; set; }   //  P-Pending, I-In Progress, D-Deactive

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Completed { get; set; }
        public int? OrderIndex { get; set; }
        [StringLength(100)]
        public string Details { get; set; }
        
        public string PhotoUrl { get; set; }

        [StringLength(200)]
        public string Notes { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [ForeignKey(nameof(RequirementDefId))] 
        public virtual ChurchLifeActivityReqDef ActivityRequirementDef { get; set; }
        [ForeignKey(nameof(ChurchEventId))] 
        public virtual ChurchCalendarEvent ChurchEvent { get; set; }
        [ForeignKey(nameof(ChurchMemberId))] 
        public virtual ChurchMember ChurchMember { get; set; }
        [ForeignKey(nameof(MemberChurchRoleId))] 
        public virtual MemberChurchRole MemberChurchRole { get; set; }
    }
}
