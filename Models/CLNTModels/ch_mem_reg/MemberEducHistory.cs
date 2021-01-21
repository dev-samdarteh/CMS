using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class MemberEducHistory
    {
        [Key]
        public int Id { get; set; }       
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }
        public int ChurchMemberId { get; set; }

        [StringLength(50)]
        public string InstitutionName { get; set; }
        [StringLength(50)] 
        public int InstitutionTypeId { get; set; }
        public int? CertificateId { get; set; }

        public string Location { get; set; }
       // public int? CountryId { get; set; } 


        [StringLength(3)]
        public string CtryAlpha3Code { get; set; }
        public bool? IsCompleted { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Since { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? Until { get; set; }
        
        [StringLength(100)]
        public string Discipline { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual MSTRModels.UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual MSTRModels.UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        public virtual ChurchBody OwnedByChurchBody { get; set; }



        [ForeignKey(nameof(CertificateId))] 
        public virtual CertificateType Certificate { get; set; } 

        [ForeignKey(nameof(ChurchMemberId))] 
        public virtual ChurchMember ChurchMember { get; set; }

        [ForeignKey(nameof(CtryAlpha3Code))] 
        public virtual Country Country { get; set; }

        [ForeignKey(nameof(InstitutionTypeId))] 
        public virtual InstitutionType InstitutionType { get; set; }

        [NotMapped]
        public string strInstitutionType { get; set; }
        [NotMapped]
        public  string  strCertificate { get; set; }
        [NotMapped]
        public  string  strCountry { get; set; } 
        [NotMapped]
        public  string  strChurchMember { get; set; }
    }
}
