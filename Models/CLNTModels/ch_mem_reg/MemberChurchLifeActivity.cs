using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class MemberChurchLifeActivity
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }
        public int? ChurchMemberId { get; set; }

        public int? ChurchLifeActivityId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? EventDate { get; set; }

        [StringLength(50)]
        public string Venue { get; set; }
        public int? MemberChurchRoleId { get; set; } 
       
        [StringLength(100)]
        public string OfficiatedByName { get; set; }
        [StringLength(100)]
        public string OfficiatedByRole { get; set; } 
        
        [StringLength(100)]
        public string ActivityVenue { get; set; }
        public bool IsActivity { get; set; }
        public bool IsOfficiatedByChurchFellow { get; set; }
        // [Column("ChurchBodyId_Cong")]
        public int? VenueChurchBodyId { get; set; }
       // [Column("Congregation_Ext")]
        [StringLength(100)]
        public string CongregationExt { get; set; }
       // [Column("IsOfficiatedBy_Ext")]
        public bool IsOfficiatedByExt { get; set; }
       // [Column("OfficiatedByCong_Ext")]
        [StringLength(100)]
        public string OfficiatedByCongExt { get; set; }
       // [Column("OfficiatedByName_Ext")]
        [StringLength(100)]
        public string OfficiatedByNameExt { get; set; }
       // [Column("OfficiatedByRole_Ext")]
        [StringLength(100)]
        public string OfficiatedByRoleExt { get; set; }
        public string PhotoUrl { get; set; }

        [StringLength(300)]
        public string Comments { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }

        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }

         

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [ForeignKey(nameof(VenueChurchBodyId))] 
        public virtual ChurchBody VenueChurchBody { get; set; }

        [ForeignKey(nameof(ChurchLifeActivityId))] 
        public virtual ChurchLifeActivity ChurchLifeActivity { get; set; }

        [ForeignKey(nameof(ChurchMemberId))] 
        public virtual ChurchMember ChurchMember { get; set; }

        [ForeignKey(nameof(MemberChurchRoleId))] 
        public virtual MemberChurchRole MemberChurchRole { get; set; }
    }
}
