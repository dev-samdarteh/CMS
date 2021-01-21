using RhemaCMS.Models.Adhoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class UserGroup
    {
        public UserGroup()
        {  }

        public UserGroup(int? id, int? appGlobalOwnId, int? churchBodyId, string groupName, string groupDesc, int? groupCategoryId, string status, DateTime? created, DateTime? lastMod, int? createdByUserId, int? lastModByUserId)
        {
            Id = (int)id;
            AppGlobalOwnerId = appGlobalOwnId;
            ChurchBodyId = churchBodyId; 
            UserGroupCategoryId = groupCategoryId; 
            Status = status;  
            GroupName = groupName;
            GroupDesc = groupDesc;
            Created = created;
            LastMod = lastMod;
            CreatedByUserId = createdByUserId;
            LastModByUserId = lastModByUserId;
        }

        [Key]
        public int Id { get; set; } 
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        [StringLength(50)]
        public string GroupName { get; set; }
        [StringLength(200)]
        public string GroupDesc { get; set; }
        public int? UserGroupCategoryId { get; set; }
        [StringLength(1)]
        public string Status { get; set; } 
        //public int? UserProfileGroupId { get; set; }
        //public int? UserProfileId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
       // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]
        //[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual MSTRChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(UserGroupCategoryId))] 
        public virtual UserGroup UserGroupCategory { get; set; }
         

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }

        //[ForeignKey(nameof(UserProfileId))] 
        //public virtual UserProfile UserProfile { get; set; }
        //[ForeignKey(nameof(UserProfileGroupId))] 
        //public virtual UserProfileGroup UserProfileGroup { get; set; } 

        //public virtual List<UserGroup> InverseUserGroupCategory { get; set; } 
        //public virtual List<UserGroupPermission> UserGroupPermission { get; set; } 
        //public virtual List<UserProfileGroup> UserProfileGroupNavigation { get; set; }
    }
}
