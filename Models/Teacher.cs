using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KeeleSystem.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string Nimi { get; set; }

        public string Kvalifikatsioon { get; set; }
        public string FotoPath { get; set; }

        // связь с Identity
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}