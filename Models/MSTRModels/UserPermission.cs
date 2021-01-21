using RhemaCMS.Models.Adhoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class UserPermission
    {
        public UserPermission()
        {  }

        public UserPermission(int id, int? userPermCategoryId, string permissionCode, string permissionName, string permStatus, DateTime? created, DateTime? lastMod, int? createdByUserId, int? lastModByUserId)
        {
            Id = id;
            UserPermCategoryId = userPermCategoryId;
            PermissionCode = permissionCode;
            PermissionName = permissionName;
           // Crud = crud;
            PermStatus = permStatus;
           // ChurchBodyId = churchBodyId;
            Created = created;
            LastMod = lastMod; 
            CreatedByUserId = createdByUserId;
            LastModByUserId = lastModByUserId;
        }


        [Key]
        public int Id { get; set; }
        public int? UserPermCategoryId { get; set; }
        [StringLength(10)]
        public string PermissionCode { get; set; }
        [StringLength(100)]
        public string PermissionName { get; set; }
        //[Column("CRUD")]
        //[StringLength(1)]
        //public string Crud { get; set; } 
        //public int? UserGroupPermissionId { get; set; }
        //public int? UserRolePermissionId { get; set; }

        [StringLength(1)]
        public string PermStatus { get; set; }
      //  public int? ChurchBodyId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped] // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] //  [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }



        //[ForeignKey(nameof(ChurchBodyId))] 
        //public virtual MSTRChurchBody ChurchBody { get; set; }  
         

        //[ForeignKey(nameof(UserGroupPermissionId))]       
        //public virtual UserGroupPermission UserGroupPermission { get; set; }
        [ForeignKey(nameof(UserPermCategoryId))] 
        public virtual UserPermission UserPermCategory { get; set; }

        //[ForeignKey(nameof(UserRolePermissionId))] 
        //public virtual UserRolePermission UserRolePermission { get; set; } 

        //public virtual List<UserPermission> InverseUserPermCategories { get; set; } 
        //public virtual List<UserGroupPermission> UserGroupPermissions { get; set; } 
        //public virtual List<UserRolePermission> UserRolePermissions { get; set; }

        [NotMapped]
        public string strPermDesc { get; set; } 
        
        [NotMapped]
        public string strPermCategory { get; set; }
    }
}
