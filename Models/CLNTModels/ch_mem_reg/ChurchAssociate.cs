using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchAssociate
    {
        public ChurchAssociate()
        { }

        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        [StringLength(10)]
        public string Title { get; set; }
        [Required]
        [StringLength(90)]
        public string Name { get; set; }
        public int? LanguageSpokenId { get; set; }
        public int? ContactInfoId { get; set; }
        public string PhotoUrl { get; set; }
        [StringLength(100)]
        public string OtherInfo { get; set; }
        [StringLength(1)]
        public string Status { get; set; } 
        public int? ChurchFaithTypeId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(100)]
        public string Denomination { get; set; }
        public int? FaithTypeCategoryId { get; set; }
        [StringLength(1)]
        public string Gender { get; set; }
        [StringLength(1)]
        public string MaritalStatus { get; set; }
        public int NationalityId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }
        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual ChurchBody ChurchBody { get; set; }
        [ForeignKey(nameof(ChurchFaithTypeId))] 
        public virtual ChurchFaithType ChurchFaithType { get; set; }
        [ForeignKey(nameof(ContactInfoId))] 
        public virtual ContactInfo ContactInfo { get; set; }
        [ForeignKey(nameof(FaithTypeCategoryId))] 
        public virtual ChurchFaithType FaithTypeCategory { get; set; }
        [ForeignKey(nameof(LanguageSpokenId))] 
        public virtual LanguageSpoken LanguageSpoken { get; set; }
        [ForeignKey(nameof(NationalityId))] 
        public virtual Country Nationality { get; set; } 
       // public virtual List<ChurchEventActor> ChurchEventActor { get; set; }
    }
}
