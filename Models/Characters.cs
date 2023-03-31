namespace GenshinTheoryCrafting.Models
{
    public class Characters
    {
        public int id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CHRVision Vision { get; set; } = CHRVision.Pyro;
        public CHRCLass Class { get; set; } = CHRCLass.Sword;
       
    }
}
