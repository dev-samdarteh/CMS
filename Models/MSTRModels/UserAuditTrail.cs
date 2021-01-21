using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RhemaCMS.Models.MSTRModels
{
    public partial class UserAuditTrail
    {
        public UserAuditTrail() { }
        public UserAuditTrail(int? id, int? appGlobalOwnId, int? churchBodyId, string auditType, string oUI_Desc, string url, string eventDetail, DateTime eventDate, int? userProfileId, DateTime? created, DateTime? lastMod, int? createdByUserId, int? lastModByUserId)
        {
           // Id = (int)id;
            AppGlobalOwnerId = appGlobalOwnId;
            ChurchBodyId = churchBodyId;
            AuditType = auditType;
            UI_Desc = oUI_Desc;
            Url = url;
            EventDate = eventDate;
            EventDetail = eventDetail;
            UserProfileId = userProfileId;
            Created = created;
            LastMod = lastMod;
            CreatedByUserId = createdByUserId;
            LastModByUserId = lastModByUserId;
        }

        [Key]
        public int Id { get; set; }
        public int? AppGlobalOwnerId { get; set; }
       
        public int? ChurchBodyId { get; set; }

        [StringLength(1)]  //AuditType -- TRANSACTIONAL = T, NAVIGATIONAL = N, LOGIN /LOGOUT = L
        public string AuditType { get; set; }

        //[Column("UI_Desc")]  
        [StringLength(50)] // Membership = MBR, Church Transfer = TNF...
        public string UI_Desc { get; set; }

        [Display(Name = "Source Url")]
        public string Url { get; set; }
        public DateTime EventDate { get; set; }
        public string EventDetail { get; set; }
        public int? UserProfileId { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastMod { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? LastModByUserId { get; set; }

        [NotMapped]  //  [ForeignKey(nameof(CreatedByUserId))]
        public virtual UserProfile CreatedByUser { get; set; }
        [NotMapped] // [ForeignKey(nameof(LastModByUserId))]
        public virtual UserProfile LastModByUser { get; set; }

        [ForeignKey(nameof(UserProfileId))] 
        public virtual UserProfile UserProfile { get; set; }

        [ForeignKey(nameof(ChurchBodyId))]
        public virtual MSTRChurchBody ChurchBody { get; set; } 
        
        [ForeignKey(nameof(AppGlobalOwnerId))]
        public virtual MSTRAppGlobalOwner AppGlobalOwner { get; set; }


        [NotMapped]
        public string strEventDate { get; set; }

        [NotMapped]
        public string strEventUser { get; set; }

        [NotMapped]
        public string strAuditType { get; set; }

        [NotMapped]
        public string strSubscriber { get; set; }
         
    }
}
