using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class RelationshipType
    {
        public RelationshipType()
        {  }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }     // Great grandfather, Grandfather, Father ...
        public int? RelationIndex { get; set; }
        //[Column("RelationshipType_FemalePairId")]
        public int? RelationshipTypeFemalePairId { get; set; }
       // [Column("RelationshipType_GenericPairId")]
        public int? RelationshipTypeGenericPairId { get; set; }
       // [Column("RelationshipType_MalePairId")]
        public int? RelationshipTypeMalePairId { get; set; }
        
        public bool IsChild { get; set; }
        public bool IsSpouse { get; set; }


        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        // public virtual List<MemberContact> MemberContact { get; set; } 
        // public virtual List<MemberRelation> MemberRelation { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }

    }
}
