using BeeMod.BeeModCode.Cards;
using BeeMod.BeeModCode.DynamicVars;
using BeeMod.BeeModCode.Minions;
using BeeMod.BeeModCode.Singletons;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Rooms;

namespace BeeMod.BeeModCode.Cards;

public class Bloom() : BeeModCard(1, CardType.Skill,
    CardRarity.Basic, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new PlantVar(4m)
    ];

    protected override async Task OnPlay(
        PlayerChoiceContext choiceContext,
        CardPlay play)
    {
        // dead pets stay in Allies, so this finds corpses too
        var flowerOld = CombatState!.Allies.FirstOrDefault(c => c.Monster is GreenFlower && c.PetOwner == Owner);
        /*
        if (flower is { IsAlive: true })
        {
            await CreatureCmd.SetMaxAndCurrentHp(flower, DynamicVars["Plant"].BaseValue);
            return;
        }
        */
        var flower = await Plant(choiceContext);
        if (flowerOld is not null)
        {
            // commented out the below code because i basically copied it below
            // Owner.PlayerCombatState!.AddPetInternal(flower);
            
            var _pets = (List<Creature>)AccessTools.DeclaredField(typeof(PlayerCombatState), "_pets").GetValue(Owner.PlayerCombatState);
            if (_pets == null) MainFile.Logger.Info("_pets is null. things are about to blow up. weep.");
            flower.Monster.AssertMutable();
            
            if (!_pets.Contains(flower)) // Even if this is a new flower, apparently pets "contains" it, so it always goes to the else
            {
                // this code will never run, because any flower after this will count as a duplicate for no reason
                MainFile.Logger.Info("pets does not contain flower, add it");
                if (flower.PetOwner != Owner)
                {
                    flower.PetOwner = Owner;
                }
                // flower.Died += (Action<Creature>)AccessTools.DeclaredMethod(typeof(PlayerCombatState), "OnPetDied").Invoke(Owner.PlayerCombatState, [flower]);
                _pets.Add(flower);
            }
            else
            {
                // any follow up flowers end up here. despite us trying to add it to the list forcibly...
                MainFile.Logger.Info("pets does contain flower, cry");
                if (flower.PetOwner != Owner)
                {
                    flower.PetOwner = Owner;
                }
                _pets.Add(flower); // THIS DOESNT WORK!
            }
        }

        await CreatureCmd.SetMaxAndCurrentHp(flower, DynamicVars["Plant"].BaseValue);
        flower.HpDisplay = HpDisplay.Normal;
    }
    
    async Task<Creature> Plant(PlayerChoiceContext ctx)
    {
        var v = (GreenFlower)ModelDb.Monster<GreenFlower>().MutableClone();
        v.MinionId = MinionStuff.Minions.Count + 1;
        Creature h = Owner.Creature.CombatState.CreateCreature(v, Owner.Creature.Side, null);
        MainFile.Logger.Info("New Minion ID: " + h.CombatId.ToString());
        await PlayerCmd.AddPet(h, Owner);
        // var h = (await PlayerCmd.AddPet<GreenFlower>(Owner)) ?? throw new InvalidOperationException();
        var flower = h.Monster as Flower ?? throw new InvalidOperationException();
        MinionStuff.Minions.Add(flower);
        var room = NCombatRoom.Instance;
        room?.GetCreatureNode(flower.Creature)?.Position = room.GetCreatureNode(Owner.Creature).Position + Vector2.Right *
            (room.GetCreatureNode(Owner.Creature).Hitbox.Size.X * 0.5f + 80f + (flower.MinionId * 80));
        room?.SetCreatureIsInteractable(flower.Creature, true);
        /*
        if (room?.GetCreatureNode(flower.Creature) is { } node && room.GetCreatureNode(Owner.Creature) is { } owner)
        {
            node.Position = owner.Position + Vector2.Right * (owner.Hitbox.Size.X * 0.5f + 80f + (flower.MinionId * 80));
            room.SetCreatureIsInteractable(flower.Creature, true);
        }
        */
        await flower.OnSummon();
        return flower.Creature;
    }

    protected override void OnUpgrade()
    {
        DynamicVars["PlantVar"].BaseValue += 2m;
    }
}