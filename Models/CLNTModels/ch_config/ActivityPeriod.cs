using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ActivityPeriod
    {
        public ActivityPeriod()
        {  }

        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        [StringLength(1)]
        public string Status { get; set; } 
        [StringLength(50)]
        public string PeriodDesc { get; set; }
        public int OwnedByChurchBodyId { get; set; }
        public int LengthInDays { get; set; }
        [StringLength(2)]
        public string PeriodType { get; set; }
        [StringLength(1)]
        public string SharingStatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
       // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; } 
        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [NotMapped] // [ForeignKey("ChurchBodyId")] 
        public virtual ChurchBody ChurchBody { get; set; }
        [NotMapped] // [ForeignKey("OwnedByChurchBodyId")] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }

       // public virtual List<MemberRegistration> MemberRegistration { get; set; }
    }
}
