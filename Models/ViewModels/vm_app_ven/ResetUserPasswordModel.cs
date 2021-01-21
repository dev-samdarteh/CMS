using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RhemaCMS.Models.ViewModels
{
    public class ResetUserPasswordModel
    {
        public ResetUserPasswordModel() { }

        public int UserId { get; set; }

        [Required]
        [Display(Name = "Church code")]
        public string ChurchCode { get; set; }

        [Required]
        [Display(Name = "User name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repeat Password")]
        public string RepeatPassword { get; set; }

      

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        // public int BasicCredentialsVerified { get; set; }
        public string strLogUserDesc { get; set; }
        public string IsValidated { get; set; }


        public string currChurchCode { get; set; }


        [Display(Name = "Verification code")]
        public string VerificationCode { get; set; }

        public string SentVerificationCode { get; set; }  // encrypt

        //allow security que option if set by user
        [Display(Name = "Authentication Type")]
        public int? AuthTypeUsed { get; set; } // 1-Two way verification, 2-Security question, 3-Is Human capture

        [Display(Name = "Security Question")]
        public string SecurityQue { get; set; }

        [Display(Name = "Security Answer")]
        public string SecurityAns { get; set; }  //encrypted  [que + ans]

        //  public string currUsername { get; set; }
        //  public string currPassword { get; set; }

      //  public int? oAppGlolOwnId { get; set; }
      //  public int? oChurchBodyId { get; set; }
      //  public AppGlobalOwner oDenomination { get; set; }
        public MSTRChurchLevel oChurchLevel { get; set; }
      //  public MSTRChurchBody oChurchBody { get; set; }
        public UserProfile oUserProfile { get; set; }
      //  public CLNTModels.ChurchMember oCurrLoggedMember { get; set; }
     //   public int setIndex { get; set; }



        public int? oAppGloOwnId { get; set; }
        public int? oChurchBodyId { get; set; }
        public AppGlobalOwner oAppGlobalOwn { get; set; }
        // public ChurchLevel oChurchLevel { get; set; }
        public MSTRChurchBody oChurchBody { get; set; }  // grace
        //
        public int? oAppGloOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oCurrMemberId_Logged { get; set; }
        public int? oCurrUserId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }

        public AppGlobalOwner oAppGlobalOwn_Logged { get; set; }
        public MSTRChurchBody oChurchBody_Logged { get; set; }
        public ChurchMember oCurrLoggedMember { get; set; }
        public UserProfile oChurchAdminProfile { get; set; }
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }
        public int pageIndex { get; set; }

        [StringLength(1)]
        public string profileScope { get; set; }
       
        [StringLength(1)]
        public string subScope { get; set; }

        [StringLength(50)]
        public string userMess { get; set; }

        public List<SelectListItem> lkpAuthTypes { set; get; }
        public List<SelectListItem> lkpSecurityQuestions { set; get; }
    }
}
