using BaseLib.Abstracts;
using BaseLib.Utils;
using BeeMod.BeeModCode.Character;

namespace BeeMod.BeeModCode.Potions;

[Pool(typeof(BeeModPotionPool))]
public abstract class BeeModPotion : CustomPotionModel;