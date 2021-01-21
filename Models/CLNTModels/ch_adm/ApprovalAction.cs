using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ApprovalAction
    {
        public ApprovalAction()
        { }

        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        [StringLength(100)]
        public string ApprovalActionDesc { get; set; }
        [StringLength(1)]
        public string ActionStatus { get; set; }
        [StringLength(100)]
        public string Comments { get; set; } 
        public int? ApprovalProcessId { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
        [StringLength(6)]
        public string ProcessCode { get; set; }
        [StringLength(2)]
        public string ProcessSubCode { get; set; }
        public int? CallerRefId { get; set; }
        public DateTime? LastActionDate { get; set; }
        public DateTime? ActionRequestDate { get; set; }
        [StringLength(1)]
        public string CurrentScope { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped] //[ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] //[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(ApprovalProcessId))] 
        public virtual ApprovalProcess ApprovalProcess { get; set; }
        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual ChurchBody ChurchBody { get; set; }

        //[InverseProperty("ApprovalAction")]
        //public virtual List<ApprovalActionStep> ApprovalActionSteps { get; set; }
        //[InverseProperty("EApprovalAction")]
        //public virtual List<ChurchTransfer> ChurchTransferEApprovalActions { get; set; }
        //[InverseProperty("IApprovalAction")]
        //public virtual List<ChurchTransfer> ChurchTransferIApprovalActions { get; set; }
    }
}
