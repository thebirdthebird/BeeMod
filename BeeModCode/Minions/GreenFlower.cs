using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace BeeMod.BeeModCode.Minions;

public class GreenFlower() : Flower
{

    public override string? CustomVisualPath => "res://BeeMod/scenes/green_flower.tscn";
    
    public override async Task OnSummon()
    {
        this.Creature.HpDisplay = HpDisplay.Normal;
        
        NCombatRoom.Instance!.SetCreatureIsInteractable(this.Creature, true);
    }
}