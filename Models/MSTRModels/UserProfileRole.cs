using RhemaCMS.Models.Adhoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class UserProfileRole
    {
        public UserProfileRole()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int UserProfileId { get; set; }
        public int UserRoleId { get; set; }
        [StringLength(1)]
        public string ProfileRoleStatus { get; set; }
        [Column("STRT")]
        public DateTime? Strt { get; set; }
        [Column("EXPR")]
        public DateTime? Expr { get; set; } 
        public int? ChurchBodyId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]
        // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]
        // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual MSTRChurchBody ChurchBody { get; set; }

        
        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(UserProfileId))] 
        public virtual UserProfile UserProfile { get; set; }

        [ForeignKey(nameof(UserRoleId))] 
        public virtual UserRole UserRole { get; set; }

        [NotMapped]
        public virtual List <UserRole> UserRoles { get; set; }
    }
}
