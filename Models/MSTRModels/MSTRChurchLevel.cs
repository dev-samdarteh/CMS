 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class MSTRChurchLevel
    {
        public MSTRChurchLevel()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        [StringLength(8)] // Level 01
        public string Name { get; set; }
        [StringLength(50)]
        public string CustomName { get; set; }
        public int LevelIndex { get; set; }

        // public int? OwnedByChurchBodyId { get; set; }
        [StringLength(1)]
        public string SharingStatus { get; set; }
        [StringLength(10)]
        public string Acronym { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [NotMapped]
       // [ForeignKey(nameof(AppGlobalOwnerId))] 
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }

        // public virtual AppGlobalOwner ChurchLevelAppGlobalOwner { get; set; }

        //[ForeignKey(nameof(OwnedByChurchBodyId))] 
        //public virtual ChurchBody OwnedByChurchBody { get; set; }

        // [ForeignKey(nameof(OwnedByChurchBodyId))]
        // public virtual ChurchBody OwnedByChurchBody_CL { get; set; }

        //  public virtual ChurchBody ChurchBody { get; set; }

        //[NotMapped]//
        //public virtual AppSubscription AppSubscription_ChurchLevel { get; set; }

        [NotMapped]
        public virtual List<MSTRChurchBody> ChurchBodies { get; set; }


        //[NotMapped]
        //public virtual List<ChurchBody> ChurchBodies { get; set; }

        //public virtual List<ApprovalProcess> ApprovalProcesses { get; set; }
        //public virtual List<AppSubscription> AppSubscriptions { get; set; }
        //public virtual List<TransferTypeChurchLevel> TransferTypeChurchLevels { get; set; }

        [NotMapped]
        public virtual List<MSTRAppGlobalOwner> AppGlobalOwns { get; set; }

        [NotMapped]
        public string strAppGlobalOwner { get; set; }
    }
}
