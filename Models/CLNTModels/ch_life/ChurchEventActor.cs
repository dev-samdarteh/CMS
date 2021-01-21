using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchEventActor
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }

        public int ChurchCalendarId { get; set; }
        public int? ChurchCalendarEventId { get; set; }
        [Required]
        [StringLength(1)]
        public string ChurchRelationType { get; set; }
        public int? ChurchLifeActivityId { get; set; }
        [Required]
        [StringLength(50)]
        public string EventName { get; set; }
        [StringLength(200)]
        public string Description { get; set; } 
        public int? ChurchAssociateId { get; set; }
        public bool ChurchFellow { get; set; }
        public int? RelationChurchMemberId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [ForeignKey(nameof(ChurchAssociateId))] 
        public virtual ChurchAssociate ChurchAssociate { get; set; }

        [ForeignKey(nameof(ChurchCalendarEventId))] 
        public virtual ChurchCalendarEvent ChurchCalendarEvent { get; set; }

        [ForeignKey(nameof(ChurchLifeActivityId))] 
        public virtual ChurchLifeActivity ChurchLifeActivity { get; set; }

        [ForeignKey(nameof(RelationChurchMemberId))] 
        public virtual ChurchMember RelationChurchMember { get; set; }


        [NotMapped]
        public string strChurchLifeActivity { get; set; }
        [NotMapped]
        public string strActivityTypeCode { get; set; }

        [NotMapped]
        public string strEventFullDesc { get; set; }

        [NotMapped]
        public string strChurchBodyService { get; set; }

        [NotMapped]
        public string strChurchEventCategory { get; set; }
    }
}
