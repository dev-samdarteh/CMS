using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class LanguageSpoken
    {
        public LanguageSpoken()
        { }

        /// <summary>
        /// LanguageSpoken possibly MUST be created at the TOP hierarchy and synched down... 
        /// Temporal [non-active] profile may be created by VENDOR to do this until subuscription is active at the TOP level of Church -- Not Operationalized --- and after deactivate Top CB
        /// Church Body may customize as Display or NOT Display ...
        /// </summary>
        /// 

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
       // public int? ChurchBodyId { get; set; }
        public int? OwnedByChurchBodyId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }
        //   public int? CountryId { get; set; } 

        
        [StringLength(1)]
        public string SharingStatus { get; set; }
                

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]//[ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        
        [ForeignKey(nameof(CtryAlpha3Code))] 
        public virtual Country Country { get; set; }


        //public virtual List<ChurchAssociate> ChurchAssociate { get; set; } 
        //public virtual List<ChurchMember> ChurchMember { get; set; } 
        //public virtual List<ChurchVisitor> ChurchVisitor { get; set; } 
        //public virtual List<MemberLanguageSpoken> MemberLanguageSpoken { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        //[ForeignKey(nameof(ChurchBodyId))]
        //public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }
    }
}
