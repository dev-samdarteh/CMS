using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchPosition
    {
        public ChurchPosition()
        { }

        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        [Required]
        [StringLength(50)]
        public string PositionName { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public int GradeLevel { get; set; } 
        public int? OwnedByChurchBodyId { get; set; }
        [StringLength(1)]
        public string SharingStatus { get; set; }
        public bool ApplyToClergyOnly { get; set; }
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

        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual ChurchBody ChurchBody { get; set; }
        [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; } 
      //  public virtual List<ChurchTransfer> ChurchTransfer { get; set; } 
       // public virtual List<MemberPosition> MemberPosition { get; set; }
    }
}
