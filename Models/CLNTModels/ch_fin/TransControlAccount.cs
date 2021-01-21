using RhemaCMS.Models.Adhoc;
using RhemaCMS.Models.MSTRModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.CLNTModels
{
    public class TransControlAccount
    {
        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
        public int? ChurchBodyId { get; set; }

        [StringLength(50)]
        public string TransModule { get; set; }  // Offertory, Tithe, Cashbook, Tax etc.

        [Display(Name = "Trans Module")]
        [StringLength(4)]
        public string TransModuleCode { get; set; }  // OFT, TIT, CSH, TAX, // OFFT, TITH, DONA, PLDG, DISB, WDIS, DEPR, LEVY, SPPL, PROP
        [StringLength(10)]
        public string AccountNo { get; set; }  // 100000/000   == Main /Sub  ... public int? ControlAccountId { get; set; }
        //[Display(Name = "App Module")]
        //[StringLength(4)]
        //public string AppModCode { get; set; }   // OFFT, TITH, DONA, PLDG, DISB, WDIS, DEPR, LEVY, SPPL, PROP
        public int? ControlAccountId { get; set; }
        [StringLength(10)]
        public string ContraAccountNo { get; set; }  // 100000/000   == Main /Sub 
        public int? ContraAccountId { get; set; } // distribution can involve multiple accounts


        [StringLength(1)]
        public string Status { get; set; }  // A - Active, D - Deactive  

        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        //
        [NotMapped]// [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped]//[ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual AppGlobalOwner AppGlobalOwner { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual ChurchBody ChurchBody { get; set; }

        [ForeignKey(nameof(ControlAccountId))]
        public virtual AccountMaster ControlAccount { get; set; }

        [ForeignKey(nameof(ContraAccountId))]
        public virtual AccountMaster ContraAccount { get; set; }
    }
}

