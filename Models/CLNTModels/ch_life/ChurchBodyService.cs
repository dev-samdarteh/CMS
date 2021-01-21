using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchBodyService
    {
        public ChurchBodyService()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }
        ///
        [Required]
        [StringLength(50)]
        public string ServiceName { get; set; }
        public bool IsTimed { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? ServiceStart { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime ? ServiceEnd { get; set; }
        [StringLength(2)]
        public string MeetingDay { get; set; }
        [StringLength(2)]
        public string Frequency { get; set; }
        [StringLength(1)]
        public string ServiceType { get; set; }  // S-service, C-category
        public int? ServiceCategoryId { get; set; }   // self ref
        public int OrderIndex { get; set; }
        public long MaxPersCapacity { get; set; }  // of persons

        [StringLength(1)]
        public string SharingStatus { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }



        [NotMapped]//[ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }
         

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }

        [ForeignKey(nameof(ServiceCategoryId))] 
        public virtual ChurchBodyService ServiceCategory { get; set; } 


      //  public virtual List<ChurchCalendarEvent> ChurchCalendarEvent { get; set; } 
     //   public virtual List<ChurchBodyService> InverseServiceCategory { get; set; } 
      //  public virtual List<MemberChurchLife> MemberChurchLife { get; set; }
    }
}
