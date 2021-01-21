 
using System; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    [Table("MSTRAppUtilityNVP")]
    public partial class MSTRAppUtilityNVP
    {
        public MSTRAppUtilityNVP()
        { }

        [Key]
        public int Id { get; set; }
        public int? ChurchBodyId { get; set; }  // null for the AGO_details synched from the MSTR

        [Column("NVPTag")]
        [StringLength(30)]
        public string NvpTag { get; set; }
        [Column("NVPCode")]
        [StringLength(15)]
        public string NvpCode { get; set; }

        [StringLength(10)]
        public string Acronym { get; set; }
        [Column("NVPStatus")]
        [StringLength(1)]
        public string NvpStatus { get; set; }
        [Column("NVPValue")]
        [StringLength(100)]
        public string NvpValue { get; set; }
        [Column("NVP_CategoryId")]
        public int? NvpCategoryId { get; set; }
        public int? OrderIndex { get; set; }
        public bool? RequireUserCustom { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped] // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual MSTRChurchBody ChurchBody { get; set; }
        [ForeignKey(nameof(NvpCategoryId))]
        public virtual MSTRAppUtilityNVP NvpCategory { get; set; }
        //  public virtual List<AppUtilityNVP> InverseNvpCategories { get; set; }
    }
}

