using RhemaCMS.Models.MSTRModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class ChurchFaithType
    {
        public ChurchFaithType()
        { }

        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string FaithDescription { get; set; }
        public int Level { get; set; }
        public int? FaithTypeClassId { get; set; }

        [StringLength(2)]
        public string Category { get; set; }  // FS == Faith Stream/Sect.... FC= Faith Category

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }


       [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
       [NotMapped]// [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        //self-refs
        //public virtual ChurchFaithType InverseFaithType { get; set; }
        //public virtual ChurchFaithType InverseFaithTypeClass { get; set; }

        [ForeignKey(nameof(FaithTypeClassId))] 
        public virtual ChurchFaithType FaithTypeClass { get; set; }  
      //  public virtual List<AppGlobalOwner> AppGlobalOwners { get; set; } 
        public virtual List<ChurchFaithType> SubFaithTypes { get; set; }

    }
}
