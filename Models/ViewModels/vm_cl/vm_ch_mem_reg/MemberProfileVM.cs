using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RhemaCMS.Models.ViewModels
{
    public class MemberProfileVM
    {
        public MemberProfileVM() { } 

        public int MemberId { get; set; } 
        public AppGlobalOwner oAppGlobalOwner { get; set; }
        public ChurchBody oChurchBody { get; set; }
        public ChurchBody oLoggedChurchBody { get; set; }
        public string strCurrTask { get; set; }
        public bool isCurrMemberQry { get; set; }
        public bool isMemberQry { get; set; }
        
        public bool isViewPageOnly { get; set; }
        public bool isMultiView { get; set; }
        public List<MemberProfileVM> MemberProfiles { get; set; }
        public List<ChurchBody> CurrSubChurchUnits { get; set; }
        public string strContactPersonName { get; set; }

        public int? oAppGlolOwnId { get; set; }
     //   public ChurchBody oChurchBody { get; set; }  // grace
        public AppGlobalOwner oAppGlolOwn { get; set; }
        public int? oAppGloOwnId_Logged { get; set; }
        public ChurchBody oChurchBody_Logged { get; set; }
        //public ChurchMember oCurrLoggedMember { get; set; } 
        public int? oChurchBodyId { get; set; }


        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strChurchType { get; set; }
      //  public string strCurrTask { get; set; }

        public int? oAppGlolOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string strConfirmUserCode { get; set; }


        public UserProfile oUserLogged { get; set; }


        // public string strChurchUnit { get; set; }
        // public string strChurchUnit_InCharge { get; set; }
        // public long ChurchUnit_TotSubUnits{ get; set; }  // of CH or CF type  ... where members are

        //  public long CHCF_TotSubUnits { get; set; }
        public long CHCF_TotCong { get; set; }
        public long CHCF_TotMem { get; set; }
        public long CHCF_TotNewMem { get; set; }
        public long CHCF_TotMaleMem { get; set; }
        public long CHCF_TotFemMem { get; set; }
        public long CHCF_TotOtherMem { get; set; }       
        

        public List<SelectListItem> lkpChurchLevels { set; get; }
        public List<SelectListItem> lkpCongregations { set; get; } // _CF , _CH else _CU


        [Display(Name = "Member Code")]
        public string MemberCode { get; set; }
        public string strCongregation { get; set; }

        [Display(Name = "Member Name")]
        public string strMemberFullName { get; set; } 
        public string strMemberDisplayName { get; set; }
        
        [StringLength(100)]
        public string strEducLevelDesc { get; set; }   //pick current from Education history : MSc from Harvard

        [StringLength(100)]
        public string strProBrandDesc { get; set; }  // Accountant or Multiple: Lawyer, Accountant, Auditor, Administrator

       // [StringLength(100)]
        public string strJobStatusDesc { get; set; }   //incl WorkPlace @Work History...  :: Chief Director, Ministry of Finance

       // [StringLength(100)]
        public string strFamRelaDesc { get; set; }

        public string strMemSpouse { get; set; }
        public string strMemChildren { get; set; }
        public string strMemNextOfKin { get; set; }

        [Display(Name = "Core Area")]
        public string strCoreArea { get; set; }

        [Display(Name = "Core Role")]
        public string strCoreRole { get; set; }

        [Display(Name = "Church Rank")]
        public string strChurchRank { get; set; }

        [Display(Name = "Position")]
        public string strChurchPos { get; set; }

        [Display(Name = "Church Role Desc")]
        public string strMemChuRoleDesc { get; set; }

        [Display(Name = "Status")]
        public string strMemberStat { get; set; }

        [Display(Name = "Age Bracket")]
        public string strAgeGroup { get; set; }

        [StringLength(6)]
        public string strGender { get; set; }

        [StringLength(8)]
        public string strMaritalStat { get; set; }
        public string PhotoPath { get; set; }       

        public int? intMemberAge { get; set; }
        public string strMemberAge { get; set; }
        public string strAgeDesc { get; set; }
        public string strMemberClass { get; set; }
        public string strBirthday { get; set; }
        public string strLangProfiency { get; set; }
        public string strNativity { get; set; }
        public string strJoined { get; set; }
        public string strLongevity { get; set; }
        public string strLongevityDesc { get; set; }
        public string strBirthdayTag { get; set; }
        public string strDeparted { get; set; }

        public string strMemberType { get; set; }
        public string strMemTypeAssigned { get; set; }
        public string strMemTypeLongevity { get; set; }
        public string strMemTypeLongevityDesc { get; set; }

        public string strEducLevel { get; set; }
        public string strPrimLang { get; set; }

        public bool IsMemConfirmAllowed { get; set; }  //  depends on the church... most charismatics are exempted... check if pentecostals at church class

        public IFormFile MemPhotoFile { get; set; }


        //properties... additional to Member
        //[Required]
        //[Display(Name = "Church Body")]
        //public int? ChurchBodyId { get; set; }

        //[ForeignKey("ChurchBodyId")]
        //public virtual ChurchBody ChurchBody { get; set; }  //CF: ongregation level

        [Display(Name = "Primary Language Spoken")]
        public int? PrimaryLanguageId { get; set; }
        [ForeignKey("PrimaryLanguageId")]
        public virtual LanguageSpoken oPrimaryLanguage { get; set; }

        [Display(Name = "Education Level")]
        public int? EducationLevelId { get; set; }

        [ForeignKey("EducationLevelId")]
        public virtual InstitutionType oEducationLevel { get; set; }  //Education history 


        //[StringLength(100)]
        //[Display(Name = "Work Place")]
        //public string WorkPlace { get; set; }


        [Display(Name = "Church Rank")]                    // MemberPosition table
        public int? RankId { get; set; }

        [ForeignKey("RankId")]
        public virtual ChurchRank oChurchRank { get; set; }


        [Display(Name = "Church Position")]                    // MemberPosition table
        public int? PositionId { get; set; }

        [ForeignKey("PositionId")]
        public virtual ChurchPosition oChurchPosition { get; set; }

        [Display(Name = "Membership Status")]               // MemberStatus table
        public int? ChurchMemStatusId { get; set; }

        [ForeignKey("ChurchMemStatusId")]
        public virtual ChurchMemStatus oChurchMemStatus { get; set; }


        //entities



        //Personal information
        public ChurchMember oPersonalData { get; set; } 

        public ChurchAssociate oChurchAssociate { get; set; }
        public ContactInfo oMemberLocAddr { get; set; }
        public ContactInfo oMemberContactInfo { get; set; }
        public MemberContactVM oMemberContactVM { get; set; }
        public MemberContact oMemberContact { get; set; }
        public List<MemberContact> oMemberContactList { get; set; }
        public string strMemContactsList { get; set; }
        public MemberLanguageSpoken oMemberLangSpkn { get; set; } 
        public List<MemberLanguageSpoken> oMemberLangSpknList { get; set; }      
       // public string strMemLangSpknList { get; set; }  // delimited by pipe and commas for each table row
       // public MemberLanguageSpoken[] strMemberLangSpkn { get; set; } 

        public MemberEducHistory oMemberEducHistory { get; set; }
        public string strMemEduHistList { get; set; }
        public List<MemberEducHistory> oMemberEducHistoryList { get; set; }
        public MemberProfessionBrand oMemberProBrand { get; set; }
        public List<MemberProfessionBrand> oMemberProBrandList { get; set; }
        public string strMemProBrandList { get; set; }
        public MemberWorkExperience oMemberWorkExpr { get; set; }
        public string strMemWorkExprList { get; set; }
        public List<MemberWorkExperience> oMemberWorkExprList { get; set; }        
        public MemberRelation oMemberRelation { get; set; }
        public List<MemberRelation> oMemberRelationList { get; set; }
        public string strMemFamRelaList { get; set; }

        //Member church related      
        public List<ChurchAssociate> oChurchAssociateList { get; set; }
        public MemberChurchLife oMemberChurchlife { get; set; }
        public MemberChurchLifeActivity oMemberChurchlifeActv { get; set; }
        public string strMemCLAList { get; set; }
        public List<MemberChurchLifeActivity> oMemberChurchlifeActvList { get; set; }

        public MemberPosition oMemberPos { get; set; }
        public List<MemberPosition> oMemberPosList { get; set; }

        public MemberRank oMemberRank { get; set; }
        public List<MemberRank> oMemberRankList { get; set; }
        public string strMemRankList { get; set; }

        public MemberType oMemberType { get; set; }
        public List<MemberType> oMemberTypeList { get; set; }
        public string strMemTypeList { get; set; }

        public MemberStatus oMemberStatus { get; set; }
        public List<MemberStatus> oMemberStatusList { get; set; }
        public string strMemStatList { get; set; }

        public List<MemberChurchSector> oMemberChurchUnitList { get; set; }
        public MemberChurchSector oMemberChurchUnit { get; set; }       
        public MemberChurchSector oMemberChurchUnit_AgeGroup { get; set; }
        public string strMemChUnitList { get; set; }

        public MemberChurchSector oMemberSector  { get; set; }
        public List<MemberChurchSector> oMemberSectorList { get; set; }   
        
        public MemberChurchRole oMemberRole { get; set; }        
        public int? oMemberRoleId { get; set; }
        public List<MemberChurchRole> oMemberRolesList { get; set; }
        public string strMemDesigRoleList { get; set; }

        public MemberRegistration oMemberReg { get; set; }
        public List<MemberRegistration> oMemberRegList { get; set; }
        public string strMemRegList { get; set; }


        //Lookups Lists... 
        public List<SelectListItem> lkpChurchMembers { set; get; }
        public List<SelectListItem> lkpChurchMembers_Local { set; get; }
        public List<SelectListItem> lkpChurchMembers_Denom { set; get; }
        public List<SelectListItem> lkpChurchAssociates { set; get; }
        public List<SelectListItem> lkpActivStatus { set; get; }
        public List<SelectListItem> lkpGenderBaseTypes { set; get; }
        public List<SelectListItem> lkpGenderTypes { set; get; }
        public List<SelectListItem> lkpMaritalStatuses { set; get; }
        public List<SelectListItem> lkpChurchMemStatuses { set; get; }
        public List<SelectListItem> lkpChuMemTypes { set; get; }
        public List<SelectListItem> lkpChuMemRanks { set; get; }
        public List<SelectListItem> lkpChuPositions { set; get; }
        public List<SelectListItem> lkpMaritalTypes { set; get; }
        public List<SelectListItem> lkpCountries { set; get; }
        public List<SelectListItem> lkpCtryRegions { get; set; }
        public List<SelectListItem> lkpEduLevels { set; get; }
        public List<SelectListItem> lkpLanguages { set; get; }
        public List<SelectListItem> lkpChurchServices { set; get; }
        public List<SelectListItem> lkpID_Types { set; get; }
        public List<SelectListItem> lkpPersTitles { set; get; }
        public List<SelectListItem> lkpCertificates { set; get; }
        public List<SelectListItem> lkpInstitutionTypes { set; get; }
        public List<SelectListItem> lkpChurchFellowOptions { set; get; }
        public List<SelectListItem> lkpContactInfoList { set; get; }
        public List<SelectListItem> lkpRelationshipTypes { set; get; }
        public List<SelectListItem> lkpRelationTypes { set; get; }
        public List<SelectListItem> lkpMemRelStatuses { set; get; }
        public List<SelectListItem> lkpAffiliationStatuses { set; get; }
        public List<SelectListItem> lkpChuRoles { set; get; }
        public List<SelectListItem> lkpChuSectors { set; get; }
        public List<SelectListItem> lkpSectorScopes { set; get; }
        public List<SelectListItem> lkpChuPeriods { set; get; }
        public List<SelectListItem> lkpChuLifeActivities { set; get; }
        public List<SelectListItem> lkpClergyList { set; get; }
        public List<SelectListItem> lkpLangProfLevels { set; get; }
        public List<SelectListItem> lkpChurchUnits { set; get; }
        public List<SelectListItem> lkpChurchUnitTypes { set; get; }
        public List<SelectListItem> lkpChurchGroupingTypes { set; get; }
        public List<SelectListItem> lkpFaithTypes { set; get; }
        public List<SelectListItem> lkpchurchMemTypes { set; get; }


        public string ReturnMaritalStatusDesc(string mCode)
    {
        switch (mCode)
        {
            case "S": return "Single";
            case "M": return "Married";
            case "X": return "Separated";
            case "D": return "Divorced";
            case "W": return "Widowed";
            case "O": return "Other";

            default: return "N/A";
        }
    }

        public string ReturnLangProficiency(decimal lvl)
        {
            switch (lvl)
            {
                case 1: return "Basics"; //1-3
                case 2: return "Intermediate"; //4-6
                case 3: return "Fluent"; //7-8
                case 4: return "Proficient"; // 9-10

                default: return string.Empty;
            }
        }

    } 
}
