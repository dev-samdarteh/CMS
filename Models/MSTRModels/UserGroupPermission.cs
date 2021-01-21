using RhemaCMS.Models.Adhoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class UserGroupPermission
    {
        public UserGroupPermission()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int UserGroupId { get; set; }
        public int UserPermissionId { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
       // [Column("STRT")]
        public DateTime? Strt { get; set; }
       // [Column("EXPR")]
        public DateTime? Expr { get; set; } 
       

        //
        public bool ViewPerm { get; set; }  // Read
        public bool CreatePerm { get; set; }   //Add
        public bool EditPerm { get; set; }   //Modify
        public bool DeletePerm { get; set; }   //Remove
        public bool ManagePerm { get; set; }   //Full control
        //
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]  // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]  // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual MSTRChurchBody ChurchBody { get; set; }
        [ForeignKey(nameof(UserGroupId))] 
        public virtual UserGroup UserGroup { get; set; }
        [ForeignKey(nameof(UserPermissionId))] 
        public virtual UserPermission UserPermission { get; set; }

        [NotMapped]
        public virtual List <UserPermission> UserPermissions { get; set; }
    }
}
