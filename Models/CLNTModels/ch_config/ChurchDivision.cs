using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchDivision
    {
        public ChurchDivision()
        { }

        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        public int ChurchLevelId { get; set; }
        [Required]
        [StringLength(50)]
        public string ChurchDivisionName { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        [StringLength(2)]
        public string OrganisationType { get; set; }
        public int? ChurchDivisionTypeId { get; set; }
        public int? ParentChurchDivisionId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }
        [StringLength(1)]
        public string SharingStatus { get; set; }
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
        [ForeignKey(nameof(ChurchDivisionTypeId))] 
        public virtual ChurchDivisionType ChurchDivisionType { get; set; }
        [ForeignKey(nameof(ChurchLevelId))] 
        public virtual ChurchLevel ChurchLevel { get; set; }
        [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }
        [ForeignKey(nameof(ParentChurchDivisionId))] 
        public virtual ChurchDivision ParentChurchDivision { get; set; } 
      //  public virtual List<ChurchDivision> InverseParentChurchDivisions { get; set; }
    }
}
