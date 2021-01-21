 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchLevel
    {
        public ChurchLevel()
        {  }

        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Secondary keys from Vendor CB
        ///  

        public int? MSTR_AppGlobalOwnerId { get; set; }  // == MSTR_AppGlobalOwner.Id   ... central AGO
        public int? MSTR_ChurchLevelId { get; set; }    // == MSTR_ChurchLevel.Id ... local copy

        /// 
        /// </summary>

        public int? AppGlobalOwnerId { get; set; }
        [StringLength(8)]
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
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }

        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }   


        /// <summary>
        /// Secondary keys from Vendor CB
        /// 

        [NotMapped] // [ForeignKey(nameof(MSTR_AppGlobalOwnerId))]
        public virtual MSTRModels.MSTRAppGlobalOwner MSTRAppGlobalOwner { get; set; } 

        [NotMapped] // [ForeignKey(nameof(MSTR_ChurchLevelId))]
        public virtual MSTRModels.MSTRChurchLevel MSTRChurchLevel { get; set; }

        /// 
        /// </summary>


        [ForeignKey(nameof(AppGlobalOwnerId))] 
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

       // public virtual AppGlobalOwner ChurchLevelAppGlobalOwner { get; set; }

        //[ForeignKey(nameof(OwnedByChurchBodyId))] 
        //public virtual ChurchBody OwnedByChurchBody { get; set; }

       // [ForeignKey(nameof(OwnedByChurchBodyId))]
       // public virtual ChurchBody OwnedByChurchBody_CL { get; set; }

        //  public virtual ChurchBody ChurchBody { get; set; }

        //[NotMapped]//
        //public virtual AppSubscription AppSubscription_ChurchLevel { get; set; } 

        [NotMapped]
        public virtual List<ChurchBody> ChurchBodies { get; set; }   

        //[NotMapped]
        //public virtual List<ChurchBody> ChurchBodies { get; set; }

        //public virtual List<ApprovalProcess> ApprovalProcesses { get; set; }
        //public virtual List<AppSubscription> AppSubscriptions { get; set; }
        //public virtual List<TransferTypeChurchLevel> TransferTypeChurchLevels { get; set; }

        [NotMapped]
        public virtual List<AppGlobalOwner> AppGlobalOwns { get; set; }

        [NotMapped]
        public string strAppGlobalOwner { get; set; }

        [NotMapped]
        public string strChurchLevelName { get; set; }
    }
}
