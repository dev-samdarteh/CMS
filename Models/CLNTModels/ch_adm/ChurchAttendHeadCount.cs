using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    [Table("ChurchAttend_HeadCount")]
    public partial class ChurchAttendHeadCount
    {
        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        public DateTime? CountDate { get; set; }
        public int? ChurchEventId { get; set; }
        [StringLength(1)]
        public string CountType { get; set; }
        public int? ChurchGroupChurchBodyId { get; set; }
        [Column("Tot_M")]
        public long TotM { get; set; }
        [Column("Tot_F")]
        public long TotF { get; set; }
        public long TotCount { get; set; } 
        [StringLength(50)]
        public string CountEventDesc { get; set; }
        [Column("Tot_O")]
        public long TotO { get; set; }
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
       
        [NotMapped] //[ForeignKey(nameof(ChurchGroupChurchBodyId))] 
        public virtual ChurchBody ChurchGroupChurchBody { get; set; }
    }
}
