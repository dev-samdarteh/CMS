using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchCalendarEvent
    {
        public ChurchCalendarEvent()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
        public int? ChurchLifeActivityId { get; set; }
        public int? ChurchEventCategoryId { get; set; } 
        public bool IsChurchServiceEvent { get; set; } 
        public int? ChurchBodyServiceId { get; set; }

        [StringLength(100)]
        public string Venue { get; set; }
        public bool IsFullDay { get; set; }
        public DateTime? EventTo { get; set; }   // include time...
        public DateTime? EventFrom { get; set; }
        
        [StringLength(30)]
        public string ThemeColor { get; set; }  // get the color palettes  
        public bool IsEventActive { get; set; }  // can be deactivated ... 

        [StringLength(1)]
        public string SharingStatus { get; set; }

        public string PhotoUrl { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }
        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [ForeignKey(nameof(ChurchBodyServiceId))] 
        public virtual ChurchBodyService ChurchBodyService { get; set; }

        [ForeignKey(nameof(ChurchEventCategoryId))] 
        public virtual ChurchEventCategory ChurchEventCategory { get; set; }

        [ForeignKey(nameof(ChurchLifeActivityId))] 
        public virtual ChurchLifeActivity ChurchLifeActivity { get; set; }


        //public virtual List<ChurchAttendAttendees> ChurchAttendAttendees { get; set; } 
        //public virtual List<ChurchAttendHeadCount> ChurchAttendHeadCount { get; set; } 
        //public virtual List<ChurchAttendance> ChurchAttendance { get; set; } 
        //public virtual List<ChurchEventActor> ChurchEventActor { get; set; } 
        //public virtual List<EventActivityReqLog> EventActivityReqLog { get; set; }

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
