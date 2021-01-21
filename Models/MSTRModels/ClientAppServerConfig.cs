using RhemaCMS.Models.Adhoc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class ClientAppServerConfig
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
       // public int? ChurchBodyId { get; set; }
        [StringLength(100)]
        public string ServerName { get; set; }
        [StringLength(100)]
        public string DbaseName { get; set; }
        [StringLength(100)]
        public string SvrUserId { get; set; }
        [StringLength(100)]
        public string SvrPassword { get; set; }
        // public int? UserProfileId { get; set; }

        [Display(Name = "Date Configured")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:ddd, dd-MMM-yyyy}", ApplyFormatInEditMode = false)]
        public DateTime? ConfigDate { get; set; }
        [StringLength(1)]
        public string Status { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }



        //[ForeignKey(nameof(ChurchBodyId))]
        //public virtual MSTRChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))] 
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }
    }
}
