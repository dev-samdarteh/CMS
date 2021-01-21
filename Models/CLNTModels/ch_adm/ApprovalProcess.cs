using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ApprovalProcess
    {
        public ApprovalProcess()
        { }

        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProcessName { get; set; }
        [StringLength(100)]
        public string ProcessDesc { get; set; }
        public int ApprovalLevels { get; set; }
        public int? OwnedByChurchBodyId { get; set; }

        [StringLength(1)]
        public string ProcessStatus { get; set; }
        [StringLength(10)]
        public string ProcessCode { get; set; }
        [StringLength(2)]
        public string ProcessSubCode { get; set; }
        public int? EscalLeaderRoleId { get; set; }
        public string RemindFreqHours { get; set; }
        [Column("EscalSLA_MaxHrs", TypeName = "decimal(18, 2)")]
        public decimal EscalSlaMaxHrs { get; set; }
        [Column("EscalSLA_MinHrs", TypeName = "decimal(18, 2)")]
        public decimal EscalSlaMinHrs { get; set; }
        public int? ChurchLevelId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
       // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]
        //[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual ChurchBody ChurchBody { get; set; }
        [NotMapped] //[ForeignKey(nameof(ChurchLevelId))] 
        public virtual ChurchLevel ChurchLevel { get; set; }
        [ForeignKey(nameof(EscalLeaderRoleId))] 
        public virtual LeaderRole EscalLeaderRole { get; set; }
        [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; } 
      //  public virtual List<ApprovalAction> ApprovalAction { get; set; } 
      //  public virtual List<ApprovalProcessStep> ApprovalProcessStep { get; set; }
    }
}
