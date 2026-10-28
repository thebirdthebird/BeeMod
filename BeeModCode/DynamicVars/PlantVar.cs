using MegaCrit.Sts2.Core.Localization.DynamicVars;

namespace BeeMod.BeeModCode.DynamicVars;

public class PlantVar : DynamicVar
{
    public const string defaultName = "Plant";

    public PlantVar(decimal summonAmount)
        : base(defaultName, summonAmount)
    {
    }

    public PlantVar(string name, decimal summonAmount)
        : base(name, summonAmount)
    {
    }
    
}