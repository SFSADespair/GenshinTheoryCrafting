using System.ComponentModel.DataAnnotations;

namespace GenshinTheoryCrafting.Models
{
    public class Characters
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public CHRVision Vision { get; set; } = CHRVision.Pyro;
        public CHRCLass Class { get; set; } = CHRCLass.Sword;
    }
}
