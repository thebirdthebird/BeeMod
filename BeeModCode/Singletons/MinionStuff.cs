using BaseLib.Abstracts;
using BeeMod.BeeModCode.Minions;
using MegaCrit.Sts2.Core.Entities.Creatures;

namespace BeeMod.BeeModCode.Singletons;

public class MinionStuff() : CustomSingletonModel(HookType.Combat)
{
    public static List<Flower> Minions = [];
}