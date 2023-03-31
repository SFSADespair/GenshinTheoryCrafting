namespace GenshinTheoryCrafting.Models
{
    public class CharacterStats
    {
        public short Level { get; set; } = 1;
        public double BaseAttack { get; set; } = 26.07;
        public double BaseDefense { get; set; } = 61.03;
        public double HP { get; set; } = 1011.0;
        public double CritRate { get; set; } = 5.0;
        public decimal CritDamage { get; set; } = 50;
        public double ElementalDmgBonus { get; set; } = 0.0;
        public decimal ElementalMastery { get; set; } = 0;
        public double HPPercentage { get; set; } = 0.0;
        public double BonusDefense { get; set; } = 0.0;
        public double HealingBonus { get; set; } = 0.0;
        public double AttackPercentage { get; set; } = 0.0;
        public double EnergyRecharge { get; set; } = 0.0;
    }
}
