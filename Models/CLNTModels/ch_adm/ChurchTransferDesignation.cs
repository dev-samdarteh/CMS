using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchTransferDesignation
    {
        [Key]
        public int Id { get; set; }
        public int DesigChurchBodyId { get; set; }
        public int ChurchTransferId { get; set; }
        public int? ToRoleSectorId { get; set; }
        public int? ToLeaderRoleId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
       // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]
       // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey(nameof(ChurchTransferId))] 
        public virtual ChurchTransfer ChurchTransfer { get; set; }
        [ForeignKey(nameof(DesigChurchBodyId))] 
        public virtual ChurchBody DesigChurchBody { get; set; }
        [ForeignKey(nameof(ToLeaderRoleId))] 
        public virtual LeaderRole ToLeaderRole { get; set; }
        [ForeignKey(nameof(ToRoleSectorId))] 
        public virtual ChurchSector ToRoleSector { get; set; }
    }
}
