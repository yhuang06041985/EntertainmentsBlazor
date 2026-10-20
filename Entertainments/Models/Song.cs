

using System.ComponentModel.DataAnnotations;

namespace Entertainments.Models
{
    public class Song
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Artist { get; set; }

        [Required]
        public string FilePath { get; set; }
    }
}
