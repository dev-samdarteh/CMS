//using RhemaCMS.Models.Adhoc;
//using RhemaCMS.Models.MSTRModels;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace RhemaCMS.Models.CLNTModels
//{
//    public partial class ChurchSectorCategory
//    {
//        public ChurchSectorCategory()
//        { }

//        [Key]
//        public int Id { get; set; }
//        public int ChurchBodyId { get; set; }
//        [StringLength(30)]
//        public string Name { get; set; }
//        public int? OwnedByChurchBodyId { get; set; } 
//        public bool IsMainstream { get; set; }
//        [StringLength(1)]
//        public string Status { get; set; }
//        [StringLength(1)]
//        public string SharingStatus { get; set; }
//        public DateTime? Created { get; set; }
//        public DateTime? LastMod { get; set; }
//        public int? CreatedByUserId { get; set; }
//        public int? LastModByUserId { get; set; }

//        [NotMapped]
//       // [ForeignKey(nameof(CreatedByUserId))]
//        public virtual UserProfile CreatedByUser { get; set; }
//        [NotMapped]
//       // [ForeignKey(nameof(LastModByUserId))]
//        public virtual UserProfile LastModByUser { get; set; }
//        [ForeignKey(nameof(ChurchBodyId))] 
//        public virtual ChurchBody ChurchBody { get; set; }
//        [ForeignKey(nameof(OwnedByChurchBodyId))] 
//        public virtual ChurchBody OwnedByChurchBody { get; set; } 
//     //   public virtual List<ChurchSector> ChurchSector { get; set; }
//    }
//}
