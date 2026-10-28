using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace BeeMod.BeeModCode.Minions;

public abstract class Flower() : CustomPetModel(visibleHp: true)
{
    public override int MinInitialHp => 1;
    public override int MaxInitialHp => 1;

    public int MinionId { get; set; }
    
    public abstract Task OnSummon();

    public override async Task AfterCardPlayed(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        MainFile.Logger.Info("Flower " + MinionId + " reporting in!");
        string str = "";
        foreach (var n in NCombatRoom.Instance?.CreatureNodes)
        {
            str += n.Name + " | ";
        }
        MainFile.Logger.Info(str);
    }
}