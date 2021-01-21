
using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels;
using RhemaCMS.Models.MSTRModels;
using System.Collections.Generic; 

namespace RhemaCMS.Models.ViewModels
{
    public class ChurchVisitorVM
    {
        public int MemberId { get; set; }
        public AppGlobalOwner oAppGlobalOwner { get; set; }
        public ChurchBody oChurchBody { get; set; }
        public ChurchBody oLoggedChurchBody { get; set; }

        public ChurchVisitor oChurchVisitor { get; set; }
        public ContactInfo oMemberContactInfo { get; set; }

        public string strCurrTask { get; set; }
        public string strCurrTaskStat { get; set; }
       public bool isActiveVisQry { get; set; }  //History or Active

        public bool isViewPageOnly { get; set; }
        public List<ChurchVisitorVM> ChurchVisitorList { get; set; }
        public List<ChurchBody> CurrSubChurchUnits { get; set; }
        public string strContactPersonName { get; set; }

        public bool isCurrMigrateQry { get; set; }
       // public bool isCurrNewConQry { get; set; }


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

       // public IFormFile MemPhotoFile { get; set; }
        
      //  public string PhotoPath { get; set; }
        public string strNativity { get; set; }
        public string strMaritalStat { get; set; }
        public string strGender { get; set; } 
        public string strMemberDisplayName { get; set; }
        public string strMemberFullName { get; set; }

        public string strFirstVisitDate { get; set; }
       // public string strBirthday { get; set; }
        public string strCongregation { get; set; }
        public string strVisitorType { get; set; }
        public string strCtryLocation { get; set; }
        public string strChurchActivity { get; set; }
        public string strVisitReason { get; set; }
        public string strChurchContact { get; set; }
        public string strVStatus { get; set; } 


        //Lookups Lists... 
        // public List<SelectListItem> lkpChurchMembers { set; get; }
        public List<SelectListItem> lkpChurchMembers_Local { set; get; }
        public List<SelectListItem> lkpChurchMembers_Denom { set; get; }
       // public List<SelectListItem> lkpChurchAssociates { set; get; }
        public List<SelectListItem> lkpActivStatus { set; get; }
      //  public List<SelectListItem> lkpGenderBaseTypes { set; get; }
        public List<SelectListItem> lkpGenderTypes { set; get; }
        public List<SelectListItem> lkpMaritalStatuses { set; get; }
        public List<SelectListItem> lkpVis_Statuses { set; get; }
      //  public List<SelectListItem> lkpChurchMemStatuses { set; get; }
        public List<SelectListItem> lkpVisitorTypes { set; get; }
       // public List<SelectListItem> lkpChuMemRanks { set; get; }
      //  public List<SelectListItem> lkpChuPositions { set; get; }
      //  public List<SelectListItem> lkpMaritalTypes { set; get; }
        public List<SelectListItem> lkpCountries { set; get; }
     //  public List<SelectListItem> lkpCtryRegions { get; set; }
      //  public List<SelectListItem> lkpEduLevels { set; get; }
        public List<SelectListItem> lkpLanguages { set; get; }
       // public List<SelectListItem> lkpChurchServices { set; get; }
       // public List<SelectListItem> lkpID_Types { set; get; }
        public List<SelectListItem> lkpPersTitles { set; get; }
      //  public List<SelectListItem> lkpCertificates { set; get; }
      //  public List<SelectListItem> lkpInstitutionTypes { set; get; }
      //  public List<SelectListItem> lkpChurchFellowOptions { set; get; }
      //  public List<SelectListItem> lkpContactInfoList { set; get; }
      //  public List<SelectListItem> lkpRelationshipTypes { set; get; }
      //  public List<SelectListItem> lkpRelationTypes { set; get; }
     //   public List<SelectListItem> lkpMemRelStatuses { set; get; }
     //   public List<SelectListItem> lkpAffiliationStatuses { set; get; }
    //    public List<SelectListItem> lkpChuRoles { set; get; }
    //    public List<SelectListItem> lkpChuSectors { set; get; }
      //  public List<SelectListItem> lkpSectorScopes { set; get; }
     //   public List<SelectListItem> lkpChuPeriods { set; get; }
        public List<SelectListItem> lkpChuLifeActivities { set; get; }
        public List<SelectListItem> lkpChuCalEvents { set; get; }
      //  public List<SelectListItem> lkpClergyList { set; get; }
      //  public List<SelectListItem> lkpLangProfLevels { set; get; }
      //  public List<SelectListItem> lkpChurchUnits { set; get; }
      //  public List<SelectListItem> lkpChurchUnitTypes { set; get; }
     //   public List<SelectListItem> lkpChurchGroupingTypes { set; get; }
        public List<SelectListItem> lkpFaithTypes { set; get; }
        public List<SelectListItem> lkpVisitorAgeBracket { set; get; }
        public List<SelectListItem> lkpVisitorReasons { set; get; }

    }
}
