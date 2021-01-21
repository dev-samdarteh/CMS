using RhemaCMS.Models.Adhoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class MSTRAppGlobalOwner
    {
        public MSTRAppGlobalOwner()
        { }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)] // Presbyterian Church of Ghana, International Central Gospel Church  
        public string OwnerName { get; set; }

        [StringLength(50)]
        public string Allias { get; set; }  // Basel, ICGC
        [StringLength(10)]
        public string Acronym { get; set; } // PCG, ICGC
        [StringLength(13)]
        public string PrefixKey { get; set; } // PCG, ICGC

        [Display(Name = "Church Code")]   // GlobalChurchCode ::>>  [ RootChurchCode /PrefixKey : 13 max ] - [7-digit] ... RCM0000000, RCM0000001, PCG1234567, COP1000000, ICGC9999999
        [StringLength(20)]
        public string GlobalChurchCode { get; set; }
               
        [Display(Name = "Relative Church Code")]   // repeat acronym... ensure uniqueness. add suffixes if necessary [ie. clashes]. [acronym]-[7-digit] ... RCM-0000000, RCM-0000001, PCG-1234567, COP-1000000, ICGC-9999999
        [StringLength(20)]
        public string RootChurchCode { get; set; }

        [Display(Name = "Church Levels")]
        [Range(1, 10)]
        public int? TotalLevels { get; set; }

        [StringLength(100)]
        public string Motto { get; set; } // That they all may be one (Joh 17:21), Leading a global army for Christ 

        [StringLength(100)]
        public string Slogan { get; set; } // [separate with pipe "|" ] Asomdwei nka wo!... Enka wo nso , Christ in you, the hope of glory
        public int? ContactInfoId { get; set; }
       // public int? CountryId { get; set; }

        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }
        public int? FaithTypeCategoryId { get; set; }

        [StringLength(1)]
        public string Status { get; set; }
                       
        public string ChurchLogo { get; set; }

        [StringLength(200)]
        public string Comments { get; set; }



        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped] // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey("CtryAlpha3Code")] 
        public virtual MSTRCountry Country { get; set; }

        [ForeignKey("FaithTypeCategoryId")] 
        public virtual ChurchFaithType FaithTypeCategory { get; set; }
         
        [ForeignKey(nameof(ContactInfoId))]
        public virtual MSTRContactInfo ContactInfo { get; set; }

        //public virtual List<AppSubscription> AppSubscriptions { get; set; } 
        //public virtual List<ChurchBody> ChurchBodies { get; set; } 

        [NotMapped]
        public virtual List<MSTRChurchLevel> ChurchLevels { get; set; }

        [NotMapped]
        public virtual List<MSTRChurchBody> Congregations { get; set; }
    }
}
