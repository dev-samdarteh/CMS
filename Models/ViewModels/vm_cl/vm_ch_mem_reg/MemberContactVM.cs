
using Microsoft.AspNetCore.Mvc.Rendering;
using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.CLNTModels; 
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace RhemaCMS.Models.ViewModels
{
    public class MemberContactVM
    {
    public MemberContactVM()
    { }

      public int MemberId { get; set; } 
    public MemberContact oMemberContact { get; set; }
    public ContactInfo oMemberContactInfo { get; set; }

    [Display(Name = "Contact Person Name")]
    public string strContactPersonName { get; set; }
    public ChurchMember oPersonalData { get; set; } 
        public MemberRelation oMemberRelation { get; set; }
       // public List<MemberRelation> oMemberRelationList { get; set; }

        //Lookups Lists... 
        public List<SelectListItem> ChurchMembers { set; get; }
    public List<SelectListItem> ChurchAssociates { set; get; }
    public List<SelectListItem> RelationshipTypes { set; get; }


        public List<SelectListItem> lkpFaithTypes { set; get; }
        public List<SelectListItem> lkpChurchFellowOptions { set; get; }
        public List<SelectListItem> lkpRelationshipTypes { set; get; }
        public List<SelectListItem> lkpChurchMembers_Local { set; get; }
        public List<SelectListItem> lkpCountries { set; get; }
        public List<SelectListItem> lkpCtryRegions { get; set; }
        public List<SelectListItem> lkpChurchAssociates { get; set; }
    } 
}



 