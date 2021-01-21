using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class LeaderRole
    {
        public LeaderRole()
        { }

        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        [Required]
        [StringLength(30)]
        public string RoleName { get; set; }
        public int? LeaderRoleCategoryId { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AuthorityIndex { get; set; } 
        public int? OwnedByChurchBodyId { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
        public bool ScopeRestricted { get; set; }
        [StringLength(1)]
        public string SharingStatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }
        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual ChurchBody ChurchBody { get; set; }
        [ForeignKey(nameof(LeaderRoleCategoryId))] 
        public virtual LeaderRoleCategory LeaderRoleCategory { get; set; }
        [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; } 
        //public virtual List<ApprovalProcess> ApprovalProcess { get; set; } 
        //public virtual List<ApprovalProcessStep> ApprovalProcessStep { get; set; } 
        //public virtual List<ChurchTransfer> ChurchTransfer { get; set; } 
        //public virtual List<ChurchTransferDesignation> ChurchTransferDesignation { get; set; } 
        //public virtual List<MemberChurchRole> MemberChurchRole { get; set; } 
        //public virtual List<SectorLeaderRole> SectorLeaderRole { get; set; }
    }
}
