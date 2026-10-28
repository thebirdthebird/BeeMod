using BaseLib.Abstracts;
using BeeMod.BeeModCode.Extensions;
using Godot;

namespace BeeMod.BeeModCode.Character;

public class BeeModPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => BeeMod.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}