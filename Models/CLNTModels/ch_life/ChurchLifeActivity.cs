using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchLifeActivity
    {
        public ChurchLifeActivity()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }

        ///
        [Required]
        [StringLength(50)]
        public string Description { get; set; } 
        [StringLength(2)]
        public string ActivityType { get; set; }   //  MA - Member Activity, GA - General Activity, EV - Event-related, MR - Member Related
        [StringLength(50)]
        public string Tag { get; set; }  // Baptised, Confirmed, Bereaved, Married etc.
        [Required]
        [StringLength(10)]
        public string ShortCode { get; set; }  // BAP, CNF, BRV, MRD, WED, etc.
        public bool IsMainline { get; set; }
        public bool IsService { get; set; }
        [StringLength(1)]
        public string SharingStatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }



        [NotMapped]//[ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        //public virtual List<ChurchCalendarEvent> ChurchCalendarEvent { get; set; } 
        //public virtual List<ChurchEventActor> ChurchEventActor { get; set; } 
        //public virtual List<ChurchLifeActivityReqDef> ChurchLifeActivityReqDef { get; set; } 
        //public virtual List<ChurchVisitor> ChurchVisitor { get; set; } 
        //public virtual List<MemberChurchLifeActivity> MemberChurchLifeActivity { get; set; }
    }
}
