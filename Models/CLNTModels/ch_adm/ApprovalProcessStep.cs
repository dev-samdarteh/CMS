using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ApprovalProcessStep
    {
        public ApprovalProcessStep()
        { }

        [Key]
        public int Id { get; set; }
        public int ApprovalProcessId { get; set; }
        public int StepIndex { get; set; }
        [StringLength(7)]
        public string ProcessStepName { get; set; }
        [StringLength(100)]
        public string StepDesc { get; set; }
        public int? ApproverLeaderRoleId { get; set; }
        public bool IsConcurrentWithOther { get; set; }
        public int? ConcurrProcessStepId { get; set; }

        [StringLength(1)]
        public string StepStatus { get; set; }
        public int ChurchBodyId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]  //  [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(ApprovalProcessId))]
        public virtual ApprovalProcess ApprovalProcess { get; set; }
        [ForeignKey(nameof(ApproverLeaderRoleId))]
        public virtual LeaderRole ApproverLeaderRole { get; set; }
        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }
        [ForeignKey(nameof(ConcurrProcessStepId))]
         public virtual ApprovalProcessStep ConcurrProcessStep { get; set; } 
        // public virtual List<ApprovalProcessStep> InverseConcurrProcessStep { get; set; }

    }
}
