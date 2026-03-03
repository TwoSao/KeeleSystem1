using System;
using System.ComponentModel.DataAnnotations;

namespace KeeleSystem.Models
{
    public class TrainingEmailViewModel
    {
        public int TrainingId { get; set; }

        public string CourseName { get; set; }

        public string TeacherName { get; set; }

        public DateTime AlgusKuupaev { get; set; }

        public int RecipientCount { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Teema")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Sisu")]
        [DataType(DataType.MultilineText)]
        public string Body { get; set; }
    }
}
