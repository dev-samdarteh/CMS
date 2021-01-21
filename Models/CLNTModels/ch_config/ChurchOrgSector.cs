//using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace RhemaCMS.Models.CLNTModels
//{
//    public partial class ChurchSector
//    {
//        public ChurchSector()
//        { }

//        [Key]
//        public int Id { get; set; }
//        public int ChurchBodyId { get; set; }
//        [StringLength(50)]
//        public string Name { get; set; }
//        public DateTime? Formed { get; set; }
//        public DateTime? Innaug { get; set; } 
//        [StringLength(100)]
//        public string PrimaryFunction { get; set; }
//        public DateTime? Dissolved { get; set; }
//        [StringLength(500)]
//        public string BriefHistory { get; set; }
//        public bool? Generational { get; set; }
//        public int? MinimumAge { get; set; }
//        public int? MaximumAge { get; set; }
//        [StringLength(1)]
//        public string GenderStatus { get; set; }
//        public string GroupLogo { get; set; }
//        public bool SubSector { get; set; }
//        public int? OwnedByChurchBodyId { get; set; }
//        public int? SectorCategoryId { get; set; }
//        [StringLength(1)]
//        public string Status { get; set; }
//        [StringLength(1)]
//        public string SharingStatus { get; set; }
//        public DateTime? Created { get; set; }
//        public DateTime? LastMod { get; set; }
//        public int? CreatedByUserId { get; set; }
//        public int? LastModByUserId { get; set; }

//        [NotMapped]
//        //[ForeignKey(nameof(CreatedByUserId))]
//        public virtual UserProfile CreatedByUser { get; set; }
//        [NotMapped]
//       // [ForeignKey(nameof(LastModByUserId))]
//        public virtual UserProfile LastModByUser { get; set; }
//        [ForeignKey(nameof(ChurchBodyId))] 
//        public virtual ChurchBody ChurchBody { get; set; }
//        [ForeignKey(nameof(OwnedByChurchBodyId))] 
//        public virtual ChurchBody OwnedByChurchBody { get; set; }
//        [ForeignKey(nameof(SectorCategoryId))] 
//        public virtual ChurchSectorCategory SectorCategory { get; set; } 
//        //public virtual List<ChurchTransfer> ChurchTransfer { get; set; } 
//        //public virtual List<ChurchTransferDesignation> ChurchTransferDesignation { get; set; } 
//        //public virtual List<MemberChurchSector> MemberChurchSector { get; set; } 
//        //public virtual List<SectorLeaderRole> SectorLeaderRole { get; set; }
//    }
//}
