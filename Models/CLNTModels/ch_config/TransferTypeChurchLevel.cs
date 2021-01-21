using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class TransferTypeChurchLevel
    {
        [Key]
        public int Id { get; set; }
        public int? ChurchBodyId { get; set; }
        [Required]
        [StringLength(2)]
        public string TransferType { get; set; }
        public int? ChurchLevelId { get; set; }
        public int? ChurchBodyOwnerId { get; set; }
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
        [ForeignKey(nameof(ChurchBodyOwnerId))] 
        public virtual ChurchBody ChurchBodyOwner { get; set; }
        [ForeignKey(nameof(ChurchLevelId))] 
        public virtual ChurchLevel ChurchLevel { get; set; }
    }
}
