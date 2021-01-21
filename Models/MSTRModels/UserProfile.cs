using Microsoft.AspNetCore.Http;
//using RhemaCMS.Models.Adhoc; 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class UserProfile
    {
        public UserProfile()
        {  }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int? OwnerUserId { get; set; } 
        public bool IsChurchMember { get; set; }   // some users may jux be employees... not church members
        public int? ChurchMemberId { get; set; }
        //public string UserId { get; set; }
                
        [StringLength(50)]
        public string UserDesc { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string Email { get; set; }

        //Hashed key [church code + username] == unique key across denominations. E.g. user: sam_1 in Grace chapel != sam_1 in Salem chapel
       // [PasswordPropertyText]
        [StringLength(256)]
        public string UserKey { get; set; }

        //Hashed key [church code + username + pwd] == unique pwd across denominations. E.g. user: sam_1|123456 in Grace chapel != sam_1|123456 in Salem chapel
        //[Required]
        [PasswordPropertyText]
        [StringLength(256)]
        public string Pwd { get; set; }

        [NotMapped]
        [PasswordPropertyText]
        [StringLength(256)]
        public string ConfirmPwd { get; set; }

        [StringLength(30)]
        public string PwdSecurityQue { get; set; }
        [StringLength(256)]
        public string PwdSecurityAns { get; set; }

        // public int maxActiveDaysBfExpr { get; set; }  // 30 days   ... reset pwd via email /sms
               
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? PwdExpr { get; set; }  // 30 days   ... reset pwd via email /sms
                
        public bool ResetPwdOnNextLogOn { get; set; }  
        
       // [Required]
       // public string PhoneNum { get; set; }

        [Display(Name = "Active from")]
        [Column("STRT")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)] 
        public DateTime? Strt { get; set; }

        [Display(Name = "Expires")]
        [Column("EXPR")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Expr { get; set; }

        [StringLength(1)]
        public string UserStatus { get; set; }  
        public string UserPhoto { get; set; }

        //[StringLength(256)]
        //public string RootProfileCode { get; set; }
        [StringLength(1)]
        public string UserScope { get; set; }

        public int? ContactInfoId { get; set; }
       

        [StringLength(1)]
        public string ProfileScope { get; set; }

        public int? ProfileLevel { get; set; }   // jux to determine user role category [SYS-1, SUP_ADMN-2, SYS_ADMN-3,CUST_ADMN-4... CH_ADMN-6,.. , CF_ADMN-11.. ]... bcos user profile can be created with no roles assigned yet.

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime? Created { get; set; }
         
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = false)]
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        //[NotMapped]
        //[ForeignKey(nameof(CreatedByUserId))]
        //public virtual UserProfile CreatedByUser { get; set; }
        //[NotMapped]
        //[ForeignKey(nameof(LastModByUserId))]
        //public virtual UserProfile LastModByUser { get; set; }


        // public virtual UserProfile InverseOwner { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))] 
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }   

        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual MSTRChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(ContactInfoId))]
        public virtual MSTRContactInfo ContactInfo { get; set; }

        //   [ForeignKey(nameof(ChurchMemberId))]
        // [NotMapped]
        // public virtual ChurchMember ChurchMember { get; set; }

        [NotMapped] //[ForeignKey(nameof(OwnerUserId))] 
        public virtual UserProfile OwnerUser { get; set; }

       
        [NotMapped]
        public string strStrt { get; set; }
       
        [NotMapped]
        public string strExpr { get; set; } 
        
        [NotMapped]
        public string strUserStatus { get; set; }

        [NotMapped]
        public int numCLIndex { get; set; }

        
        [NotMapped]
        public int? oCBChurchLevelId { get; set; }

        [NotMapped]
        public UserRole oUserRole { get; set; }

        [NotMapped]
        public string strChurchCode_CB { get; set; }

        [NotMapped]
        public string strChurchCode_AGO { get; set; }

        //public virtual List<ClientAppServerConfig> ClientAppServerConfigCreatedBy { get; set; } 
        //public virtual List<ClientAppServerConfig> ClientAppServerConfigLastModBy { get; set; } 
        //public virtual List<ClientAppServerConfig> ClientAppServerConfigUserProfiles { get; set; }

        //public virtual List<UserProfile> InverseOwners { get; set; } 
        //public virtual List<UserAuditTrail> UserAuditTrails { get; set; } 
        //public virtual List<UserGroup> UserGroups { get; set; } 
        //public virtual List<UserProfileGroup> UserProfileGroups { get; set; } 
        //public virtual List<UserProfileRole> UserProfileRoles { get; set; } 
        //public virtual List<UserRole> UserRoles { get; set; }
    }
}
