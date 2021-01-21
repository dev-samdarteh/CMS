using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchTransfer
    {
        public ChurchTransfer()
        { }

        [Key]
        public int Id { get; set; }
        [Column("I_ApprovalActionId")]
        public int? IApprovalActionId { get; set; }
        public int ChurchMemberId { get; set; }
        public int FromChurchBodyId { get; set; }
        public int? FromChurchPositionId { get; set; }
        public int? FromMemberChurchRoleId { get; set; }
        public int RequestorMemberId { get; set; }
        public int? RequestorRoleId { get; set; }
        public int? ToChurchBodyId { get; set; }
        public int? ToLeaderRoleId { get; set; }
        [StringLength(1)]
        public string AckStatus { get; set; }
        public DateTime? RequestDate { get; set; } 
        [Required]
        [StringLength(2)]
        public string TransferType { get; set; }
        public string Comments { get; set; }
        [StringLength(1)]
        public string ApprovalStatus { get; set; }
        public bool RequireApproval { get; set; }
        public DateTime? TransferDate { get; set; }
        public int? ReasonId { get; set; }
        public int? TransMessageId { get; set; }
        public int? RequestorChurchBodyId { get; set; }
        public int? ToRoleSectorId { get; set; }
        public string AckStatusComments { get; set; }
        public string ApprovalStatusComments { get; set; }
        [StringLength(1)]
        public string CurrentScope { get; set; }
        public DateTime? ToRequestDate { get; set; }
        [StringLength(100)]
        public string CustomPreambleMsg { get; set; }
        [StringLength(50)]
        public string CustomReason { get; set; }
        [Column("E_ApprovalActionId")]
        public int? EApprovalActionId { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public DateTime? ToReceivedDate { get; set; }
        public string Status { get; set; }
        public int? AttachedToChurchBodyId { get; set; }
        public string AttachedToChurchBodyList { get; set; }
        [StringLength(1)]
        public string MembershipStatus { get; set; }
        [Column("Fr_AckStatus")]
        [StringLength(1)]
        public string FrAckStatus { get; set; }
        [Column("Fr_AckStatusComments")]
        public string FrAckStatusComments { get; set; }
        [Column("Fr_ReceivedDate")]
        public DateTime? FrReceivedDate { get; set; }
        [StringLength(3)]
        public string TransferSubType { get; set; }
        public string DesigRolesList { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
      //  [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]
       // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }



        [ForeignKey(nameof(AttachedToChurchBodyId))] 
        public virtual ChurchBody AttachedToChurchBody { get; set; }
        [ForeignKey(nameof(ChurchMemberId))] 
        public virtual ChurchMember ChurchMember { get; set; }
        [ForeignKey(nameof(EApprovalActionId))] 
        public virtual ApprovalAction EApprovalAction { get; set; }
        [ForeignKey(nameof(FromChurchBodyId))] 
        public virtual ChurchBody FromChurchBody { get; set; }
        [ForeignKey(nameof(FromChurchPositionId))] 
        public virtual ChurchPosition FromChurchPosition { get; set; }
        [ForeignKey(nameof(FromMemberChurchRoleId))]
        [InverseProperty("ChurchTransfer_MemberChurchRole")]
        public virtual MemberChurchRole FromMemberChurchRole { get; set; }
        [ForeignKey(nameof(IApprovalActionId))] 
        public virtual ApprovalAction IApprovalAction { get; set; }
        [ForeignKey(nameof(ReasonId))] 
        public virtual AppUtilityNVP Reason { get; set; }
        [ForeignKey(nameof(RequestorChurchBodyId))] 
        public virtual ChurchBody RequestorChurchBody { get; set; }
        [ForeignKey(nameof(RequestorMemberId))] 
        public virtual ChurchMember RequestorMember { get; set; }
        [ForeignKey(nameof(RequestorRoleId))]
        [InverseProperty("ChurchTransfer_RequestorRole")]
        public virtual MemberChurchRole RequestorRole { get; set; }
        [ForeignKey(nameof(ToChurchBodyId))] 
        public virtual ChurchBody ToChurchBody { get; set; }
        [ForeignKey(nameof(ToLeaderRoleId))] 
        public virtual LeaderRole ToLeaderRole { get; set; }
        [ForeignKey(nameof(ToRoleSectorId))] 
        public virtual ChurchSector ToRoleSector { get; set; }
        [ForeignKey(nameof(TransMessageId))] 
        public virtual AppUtilityNVP TransMessage { get; set; } 

      //  public virtual List<ChurchTransferDesignation> ChurchTransferDesignations { get; set; }
    }
}
