using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class LanguageSpokenCustom
    {
        public LanguageSpokenCustom()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
                 
        public int LanguageSpokenId { get; set; }

        public bool IsDisplay { get; set; }         // subject to the local cong
        public bool IsDefaultLanguage { get; set; }  // the most used lang
        public bool IsChurchLanguage { get; set; }  // subject to the AGO lang of operation

        //public int? OwnedByChurchBodyId { get; set; }   // the church unit that configured

        //[StringLength(1)]
        //public string SharingStatus { get; set; }


        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped] //[ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped] //[ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }

        //[NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        //public virtual ChurchBody OwnedByChurchBody { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(LanguageSpokenId))]
        public virtual LanguageSpoken LanguageSpoken { get; set; }


        [NotMapped]
        public string strLanguage { get; set; }         

        [NotMapped]
        public string strCountry { get; set; }
    }
}
