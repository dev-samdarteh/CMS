using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchAssetStock
    {
        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        public DateTime? StockDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal StockQty { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cummulated { get; set; }
        [StringLength(1)]
        public string WorkingStatus { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitValue { get; set; } 
      //  [Column("UOM_Id")]
        public int? UnitOfMeasureId { get; set; }
        public int ChurchAssetId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped] // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(ChurchAssetId))] 
        public virtual ChurchAsset ChurchAsset { get; set; }
        [ForeignKey(nameof(ChurchBodyId))] 
         public virtual ChurchBody ChurchBody { get; set; }
        [ForeignKey(nameof(UnitOfMeasureId))] 
        public virtual AppUtilityNVP UnitOfMeasure { get; set; }
    }
}
