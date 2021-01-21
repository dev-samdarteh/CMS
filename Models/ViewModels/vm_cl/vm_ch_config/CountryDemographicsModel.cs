using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.CLNTModels;
using System;
using System.Collections.Generic;

namespace RhemaCMS.Models.ViewModels.vm_cl
{
    public abstract class CountryDemographicsModel
    {
        public CountryDemographicsModel() { }

        public string strAppName { get; set; }
        public string strAppNameMod { get; set; }
        public string strAppCurrUser { get; set; }
        public string strCurrTask { get; set; }
        // 

        public int? oAppGloOwnId { get; set; }
        public int? oChurchBodyId { get; set; }
        public AppGlobalOwner oAppGlobalOwn { get; set; }
        public ChurchBody oChurchBody { get; set; }  // grace
        //public ChurchLevel oChurchLevel { get; set; }
        

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
        public int filterIndex { get; set; } 
    }

    public class CountryModel : CountryDemographicsModel   //for CTRY and CURR
    {
        public CountryModel() { } 
        //
        public List<CountryModel> lsCountryModels { get; set; }
        public List<Country> lsCountries { get; set; }
       // public CountryModel oCountryModel { get; set; }
        public Country oCountry { get; set; }
        public string strCountry { get; set; }  
        public bool isCustomDisplay { get; set; }
        ///
       // public bool IsDisplay { get; set; }
        public bool isCustomDefaultCountry { get; set; }
        public bool isCustomChurchCountry { get; set; }
    }
    public class CountryCustomModel : CountryDemographicsModel
    {
        public CountryCustomModel() { }
        //
        public List<CountryCustomModel> lsCountryCustomModels { get; set; }
        public List<CountryCustom> lsCountriesCustom { get; set; }
        public CountryCustom oCountryCustom { get; set; }

        public string strAppGloOwn { get; set; }
        public string strCountry { get; set; }

        public List<SelectListItem> lkpCountries { set; get; } 
    }
    public class CurrencyCustomModel : CountryDemographicsModel
    {
        public CurrencyCustomModel() { }
        //
        public List<CurrencyCustomModel> lsCurrencyCustomModels { get; set; }
        public List<CurrencyCustom> lsCountriesCustom { get; set; }
        public CurrencyCustom oCurrencyCustom { get; set; }

        public string strAppGloOwn { get; set; }
        public string strCountry { get; set; }
        public string strCurrEngName { get; set; }
        public string strCurrSymbol { get; set; }
        public string strCurr3LISOSymbol { get; set; }
        public bool isCustomDisplay { get; set; }

        public List<SelectListItem> lkpCountries { set; get; }
        public List<SelectListItem> lkpCurrencies { set; get; }
    }
    public class CountryRegionModel : CountryDemographicsModel
    {
        public CountryRegionModel() { }
        //
        public List<CountryRegionModel> lsCountryRegionModels { get; set; }
        public List<CountryRegion> lsCountryRegions { get; set; }
        public CountryRegion oCountryRegion { get; set; }

        public string strAppGloOwn { get; set; }
        public string strCountry { get; set; } 
        public string strSharingStatus { get; set; } 
        public string strCountryRegion { get; set; }
        public bool isCustomDisplay { get; set; }

        public List<SelectListItem> lkpCountries { set; get; } 
    }
    public class CountryRegionCustomModel : CountryDemographicsModel
    {
        public CountryRegionCustomModel() { }
        //
        public List<CountryRegionCustomModel> lsCountryRegionCustomModels { get; set; }
        public List<CountryRegionCustom> lsCountryRegionsCustom { get; set; }
        public CountryRegionCustom oCountryRegionCustom { get; set; }

        public string strAppGloOwn { get; set; }
        public string strCountry { get; set; }
        public string strCountryRegion { get; set; }
        public string strCountryRegionCustom { get; set; }
        public bool isCustomDisplay { get; set; }

        public List<SelectListItem> lkpCountries { set; get; }
        public List<SelectListItem> lkpCountryRegions { set; get; }
       // public List<SelectListItem> lkpCountryRegionsCustom { set; get; }
    }

}



