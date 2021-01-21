using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchAttendance
    {
        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        public DateTime? DateRecorded { get; set; }
        [Column("M_Total")]
        public int MTotal { get; set; }
        [Column("F_Total")]
        public int FTotal { get; set; }
        [StringLength(100)]
        public string Occasion { get; set; }
        public int? ChurchEventId { get; set; }
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
        [ForeignKey(nameof(ChurchEventId))] 
        public virtual ChurchCalendarEvent ChurchEvent { get; set; }
    }
}
