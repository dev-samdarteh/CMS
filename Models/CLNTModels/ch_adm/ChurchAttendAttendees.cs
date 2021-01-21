using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    [Table("ChurchAttend_Attendees")]
    public partial class ChurchAttendAttendees
    {
        [Key]
        public int Id { get; set; }
        public DateTime? DateAttended { get; set; }
        public int? ChurchEventId { get; set; }
        [StringLength(1)]
        public string AttendeeType { get; set; }
        public int? ChurchMemberId { get; set; }
        public int ChurchBodyId { get; set; }
        [StringLength(10)]
        public string Title { get; set; }
        [StringLength(30)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string MiddleName { get; set; }
        [StringLength(30)]
        public string LastName { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        [StringLength(1)]
        public string MaritalStatus { get; set; }
        public int? AgeBracketId { get; set; }
        public int? NationalityId { get; set; }
        [StringLength(30)]
        public string ResidenceLoc { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public int? VisitReasonId { get; set; }
        [Column("VisitReason_Other")]
        [StringLength(100)]
        public string VisitReasonOther { get; set; }
        [StringLength(200)]
        public string OtherInfo { get; set; } 
        [Column(TypeName = "decimal(18, 2)")]
        public decimal? TempRec { get; set; }
        [StringLength(50)]
        public string AttendEventDesc { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
       // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]  // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(AgeBracketId))] 
        public virtual AppUtilityNVP AgeBracket { get; set; }
       
        [ForeignKey(nameof(ChurchEventId))] 
        public virtual ChurchCalendarEvent ChurchEvent { get; set; }
        [ForeignKey(nameof(ChurchMemberId))] 
        public virtual ChurchMember ChurchMember { get; set; }
        [ForeignKey(nameof(NationalityId))] 
        public virtual Country Nationality { get; set; }
        [ForeignKey(nameof(VisitReasonId))] 
        public virtual AppUtilityNVP VisitReason { get; set; }
    }
}
