using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class MemberChurchLife
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId  { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; } 
        public int ChurchMemberId { get; set; } 

        

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Joined { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]         
        public DateTime? Departed { get; set; }

        public bool IsPioneer { get; set; }       
        public bool IsCurrentMember { get; set; }
        public bool IsMemBaptized { get; set; }        
        public bool IsMemConfirmed { get; set; }   // only for protestant and catholic churches -- check the faith type
        public int? ChurchBodyServiceId { get; set; }   // thus the preferred service
        public bool IsMemCommunicant { get; set; }

        [StringLength(100)]
        public string NonCommReason { get; set; }  

        [StringLength(100)]
        public string EnrollReason { get; set; }
        [StringLength(100)]
        public string DepartReason { get; set; }
        public bool IsDeceased { get; set; }

        [StringLength(200)]
        public string MemberLifeSummary { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }



        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }


        [ForeignKey(nameof(ChurchBodyServiceId))] 
        public virtual ChurchBodyService ChurchBodyService { get; set; }

        [ForeignKey(nameof(ChurchMemberId))] 
        public virtual ChurchMember ChurchMember { get; set; }
    }
}
