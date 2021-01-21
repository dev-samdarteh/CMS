//using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace RhemaCMS.Models.CLNTModels
//{
//    public partial class MemberChurchUnit
//    {
//        [Key]
//        public int Id { get; set; }
//        public int? AppGlobalOwnerId { get; set; }
//        public int? ChurchBodyId { get; set; }
//        public int? OwnedByChurchBodyId { get; set; }
//        public int? ChurchMemberId { get; set; }

//        public int? ChurchUnitId { get; set; }


//        [DataType(DataType.Date)]
//        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
//        public DateTime? Joined { get; set; }

//        public bool IsMainArea { get; set; }
//        public bool IsCurrUnit { get; set; }

//        [DataType(DataType.Date)]
//        [DisplayFormat(DataFormatString = "{0:d-MMM-yyyy}", ApplyFormatInEditMode = false)]
//        public DateTime? Departed { get; set; }

//        public bool IsCurrentMember { get; set; }
//        public bool IsPioneer { get; set; } 
//        [StringLength(300)]
//        public string Profile { get; set; }


//        public DateTime? Created { get; set; }
//        public DateTime? LastMod { get; set; }
//        public int? CreatedByUserId { get; set; }
//        public int? LastModByUserId { get; set; }

//        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
//        public virtual UserProfile CreatedByUser { get; set; }
//        [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
//        public virtual UserProfile LastModByUser { get; set; }

//        [ForeignKey(nameof(ChurchMemberId))] 
//        public virtual ChurchMember ChurchMember { get; set; }

//        [ForeignKey(nameof(ChurchUnitId))] 
//        public virtual ChurchBody ChurchUnit { get; set; }


//        [ForeignKey(nameof(AppGlobalOwnerId))]
//        public virtual AppGlobalOwner CountryAppGlobalOwner { get; set; }

//        [ForeignKey(nameof(ChurchBodyId))]
//        public virtual ChurchBody ChurchBody { get; set; }

//        [NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
//        public virtual ChurchBody OwnedByChurchBody { get; set; }
//    }
//}
