using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace KeeleSystem.Models
{

    public class Course
    {
        private static readonly Dictionary<string, string> FlagMap =
            new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
            {
                { "Saksa", "🇩🇪" },
                { "German", "🇩🇪" },
                { "Eesti", "🇪🇪" },
                { "Estonian", "🇪🇪" },
                { "Inglise", "🇬🇧" },
                { "English", "🇬🇧" },
                { "Prantsuse", "🇫🇷" },
                { "French", "🇫🇷" },
                { "Hispaania", "🇪🇸" },
                { "Spanish", "🇪🇸" },
                { "Itaalia", "🇮🇹" },
                { "Italian", "🇮🇹" },
                { "Vene", "🇷🇺" },
                { "Russian", "🇷🇺" },
                { "Soome", "🇫🇮" },
                { "Finnish", "🇫🇮" }
            };

        public int Id { get; set; }

        [Required]
        public string Nimetus { get; set; }   // "Saksa keel algajatele"

        [Required]
        public string Keel { get; set; }      // German, English, Estonian jne

        [Required]
        public string Tase { get; set; }      // A1–C2

        public string Kirjeldus { get; set; }

        [NotMapped]
        public string FlagEmoji
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Keel))
                {
                    return "🏳️";
                }

                var key = Keel.Trim();
                return FlagMap.TryGetValue(key, out var flag) ? flag : "🏳️";
            }
        }
    }
}
