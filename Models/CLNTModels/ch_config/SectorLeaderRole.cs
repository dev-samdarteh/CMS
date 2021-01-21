using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class SectorLeaderRole
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? LeaderRoleId { get; set; }
        public int? ChurchSectorId { get; set; }   // church sector is configured within the CHURCH_BODY cos of the church structure
        public int? MaxNumber { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? AuthorityIndex { get; set; } 
        [StringLength(1)]
        public string SharingStatus { get; set; }
        [Column("OfficeTermMax_Yrs", TypeName = "decimal(18, 2)")]
        public decimal? OfficeTermMaxYrs { get; set; }
        [StringLength(1)]
        public string OfficeTermType { get; set; }   // Year term -- 4yrs, Age e.g. @60

        public int? OwnedByChurchBodyId { get; set; }

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

        [ForeignKey(nameof(ChurchSectorId))] 
        public virtual ChurchSector ChurchSector { get; set; }

        [ForeignKey(nameof(LeaderRoleId))] 
        public virtual LeaderRole LeaderRole { get; set; }

        [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }

    }
}
