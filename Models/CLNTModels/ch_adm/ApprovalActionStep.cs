using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ApprovalActionStep
    {
        [Key]
        public int Id { get; set; }
        public int ApprovalActionId { get; set; }
        public int? MemberChurchRoleId { get; set; }
        [StringLength(100)]
        public string ActionStepDesc { get; set; }
        public int ApprovalStepIndex { get; set; }
        [StringLength(1)]
        public string ActionStepStatus { get; set; }
        [StringLength(100)]
        public string Comments { get; set; }
        public bool CurrentStep { get; set; } 
        public int? ProcessStepRefId { get; set; }
        public int ChurchBodyId { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
        public int? ActionByMemberChurchRoleId { get; set; }
        public DateTime? ActionDate { get; set; }
        public DateTime? StepRequestDate { get; set; }
        [StringLength(1)]
        public string CurrentScope { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(ActionByMemberChurchRoleId))] 
        [InverseProperty("ApprovalActionStepActionBy")]
        public virtual MemberChurchRole ActionByMemberChurchRole { get; set; }
        [ForeignKey(nameof(ApprovalActionId))] 
        public virtual ApprovalAction ApprovalAction { get; set; }
        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual ChurchBody ChurchBody { get; set; }
        [ForeignKey(nameof(MemberChurchRoleId))]
        [InverseProperty("ApprovalActionStepAction")]
        public virtual MemberChurchRole MemberChurchRole { get; set; }
    }
}
