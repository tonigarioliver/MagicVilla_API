using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MagicVilla_VillaApi.Models.DTOs
{
    public class VillaDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [NotNull]
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }

    }
}
