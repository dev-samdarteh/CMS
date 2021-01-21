using RhemaCMS.Models.MSTRModels;
using System; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    [Table("AppUtilityNVP")]
    public partial class AppUtilityNVP
    {
        public AppUtilityNVP()
        { }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; } 

       // [Column("NVPTag")]
        [StringLength(30)]
        public string NVPCode { get; set; }
      //  [Column("NVPCode")]
        [StringLength(15)]
        public string NVPSubCode { get; set; }
        [StringLength(10)]
        public string Acronym { get; set; }
      //  [Column("NVPStatus")]
        [StringLength(1)]
        public string NVPStatus { get; set; }
      //  [Column("NVPValue")]
        [StringLength(100)]
        public string NVPValue { get; set; }
       // [Column("NVP_CategoryId")]
        public int? NVPCategoryId { get; set; }
        public int? OrderIndex { get; set; }
        public bool? RequireUserCustom { get; set; }

      //  public int? OwnedByChurchBodyId { get; set; }

        //[StringLength(1)]
        //public string SharingStatus { get; set; }

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


        [NotMapped] // [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }

        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; } 

        [ForeignKey(nameof(NVPCategoryId))] 
        public virtual AppUtilityNVP NVPCategory { get; set; }

        //  public virtual List<AppUtilityNVP> InverseNVPCategories { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        //[NotMapped] // [ForeignKey(nameof(OwnedByChurchBodyId))] 
        //public virtual ChurchBody OwnedByChurchBody { get; set; }



        [NotMapped]
        public string strNVPTag { get; set; }

        [NotMapped][StringLength(10)]
        public string strNVPStatus { get; set; }

        [NotMapped]
        public string strNVPCategory { get; set; }
    }
}
