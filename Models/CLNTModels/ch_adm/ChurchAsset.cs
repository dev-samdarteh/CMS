using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public partial class ChurchAsset
    {
        public ChurchAsset()
        { }

        [Key]
        public int Id { get; set; }
        public int ChurchBodyId { get; set; }
        [StringLength(100)]
        public string AssetName { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public int? AssetCategoryId { get; set; }        
        public bool IsCapitalized { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }


        [ForeignKey(nameof(AssetCategoryId))] 
        public virtual AssetCategory AssetCategory { get; set; }
        [ForeignKey(nameof(ChurchBodyId))] 
        public virtual ChurchBody ChurchBody { get; set; } 
       // public virtual List<ChurchAssetStock> ChurchAssetStocks { get; set; }
    }
}
