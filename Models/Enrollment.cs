using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KeeleSystem.Models
{
    public class Enrollment
    {
        public int Id { get; set; }   // PK записи

        [Required]
        [Index("IX_Training_User", 1, IsUnique = true)]
        public int TrainingId { get; set; }  // FK -> Trainings.Id

        [ForeignKey("TrainingId")]
        public virtual Training Training { get; set; }

        [Required]
        [StringLength(128)]
        [Index("IX_Training_User", 2, IsUnique = true)]
        public string ApplicationUserId { get; set; }  // FK -> AspNetUsers.Id

        [ForeignKey("ApplicationUserId")]
        public virtual ApplicationUser User { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } // Ootel / Kinnitatud / Tühistatud
    }
}
