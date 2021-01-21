using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.CLNTModels;
using System.Collections.Generic; 

namespace RhemaCMS.Models.ViewModels.vm_cl
{
    public class ClientSetupParametersModel
    {
        public ClientSetupParametersModel() { }

        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }
        // 

        public int? oAppGloOwnId { get; set; }
        //  public int? oChurchBodyId { get; set; }
        //  public AppGlobalOwner oAppGlobalOwn { get; set; }
        //  public ChurchLevel oChurchLevel { get; set; }
        //  public ChurchBody oChurchBody { get; set; }  // grace

        //
        public int? oAppGloOwnId_Logged { get; set; }
        public int? oChurchBodyId_Logged { get; set; }
        public int? oCurrMemberId_Logged { get; set; }
        // public int? oCurrUserId_Logged { get; set; }
        public int? oMemberId_Logged { get; set; }
        public int? oUserId_Logged { get; set; }
        public string oUserRole_Logged { get; set; }

        public AppGlobalOwner oAppGlobalOwn_Logged { get; set; }
        public ChurchBody oChurchBody_Logged { get; set; }
        public ChurchMember oCurrLoggedMember { get; set; }
        public MSTRModels.UserProfile oChurchAdminProfile { get; set; }
        public int setIndex { get; set; }
        public int subSetIndex { get; set; }
        public int pageIndex { get; set; }

        //
        public List<AppGlobalOwnerModel> lsAppGlobalOwnModels { get; set; }
        public AppGlobalOwnerModel oAppGlobalOwnModel { get; set; }
        public List<ChurchBodyModel> lsChurchBodyModels { get; set; }
        public ChurchBodyModel oChurchBodyModel { get; set; }
        public List<ChurchLevelModel> lsChurchLevelModels { get; set; }
        public ChurchLevelModel oChurchLevel { get; set; }
        public List<AppUtilityNVPModel> lsAppUtilityNVPModels { get; set; }
        public AppUtilityNVPModel oAppUtilityNVPModel { get; set; }
        public List<CountryModel> lsCountryModels { get; set; }
        public CountryModel oCountryModel { get; set; }
        public List<CountryCustomModel> lsCountryCustomModels { get; set; }
        public CountryCustomModel oCountryCustomModel { get; set; }
        public List<CurrencyCustomModel> lsCurrencyCustomModels { get; set; }
        public CurrencyCustomModel oCurrencyCustomModel { get; set; }
        public List<CountryRegionModel> lsCountryRegionModels { get; set; }
        public CountryRegionModel oCountryRegionModel { get; set; }
        public List<CountryRegionCustomModel> lsCountryRegionCustomModels { get; set; }
        public CountryRegionCustomModel oCountryRegionCustomModel { get; set; }
        public List<LanguageSpokenCustom> lsLanguageSpokenCustoms { get; set; }
        public LanguageSpokenCustom oLanguageSpokenCustom { get; set; }
        public List<ChurchPeriod> lsChurchPeriods { get; set; }
        public ChurchPeriod oChurchPeriod { get; set; }
        public List<ChurchPeriod> lsChurchPeriods_CY { get; set; }
        public ChurchPeriod oChurchPeriod_CY { get; set; } 
        public List<ChurchPeriod> lsChurchPeriods_AY { get; set; }
        public ChurchPeriod oChurchPeriod_AY { get; set; }
        public List<National_IdType> lsNational_IdTypes { get; set; }
        public National_IdType oNational_IdType { get; set; }







        //public ChurchLevel oChurchLevel { get; set; }

        //public int numChurchLevel { get; set; }
        //public string strChurchLevel { get; set; }
        //public string strAppGloOwn { get; set; }
        //// public string strStatus { get; set; }

        //public List<SelectListItem> lkpAppGlobalOwns { set; get; }
        //public List<SelectListItem> lkpStatuses { set; get; }

    }
}
