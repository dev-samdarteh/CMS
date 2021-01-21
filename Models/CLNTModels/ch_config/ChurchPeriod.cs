using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchPeriod
    {
        public ChurchPeriod()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? From { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? To { get; set; }

        [StringLength(1)]
        public string Status { get; set; }
        [StringLength(50)]
        public string PeriodDesc { get; set; }
        
        public int LengthInDays { get; set; }
        [StringLength(2)]
        public string PeriodType { get; set; }
        [StringLength(1)]
        public string SharingStatus { get; set; }
       

        public int? OwnedByChurchBodyId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
        // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }

        // public virtual List<MemberRegistration> MemberRegistration { get; set; }

        [NotMapped]
        public string strFrom { get; set; }
        [NotMapped]
        public string strTo { get; set; }
        [NotMapped]
        public string strStatus{ get; set; }
        [NotMapped]
        public string strSharingStatus { get; set; } 
        [NotMapped]
        [StringLength(1)]
        public string strOwnerStatus { get; set; }  // I -- Inherited, O -- Originated   i.e. currChurchBody == OwnedByChurchBody
        [NotMapped]
        public string strOwnerStatusDesc { get; set; }
        [NotMapped]
        public string strOwnerChurchBody { get; set; }

    }
}
