using System;
using System.Collections.Generic;
using VikingSagaWpfApp.Code.BattleNs.Cards;

namespace VikingSagaWpfApp.Code.BattleNs
{
    public class SpellProperty
    {
        public enum Result { Unknown, Positive, Negative };

        public enum DataName { MainMsg, Amount, IntegerValue1, IntegerValue2, IntegerValue3, String1, String2, String3, BoolValue1 };

        public Guid Id { get; private set; }
        public Result Effect { get; set; }
        public int Value { get; set; }

        private static SpellProperty Create(SpellPropertyType type, int amount, string mainMsg, Result effect)
        {
            var prop = Create(type, mainMsg, effect);
            prop.Data[DataName.Amount] = amount;
            return prop;
        }

        private static SpellProperty Create(SpellPropertyType type, string mainMsg, Result effect)
        {
            var prop = new SpellProperty();
            prop.Type = type;
            prop.Effect = effect;
            prop.Data[DataName.MainMsg] = mainMsg;
            return prop;
        }

        private SpellProperty()
        {
            Data = new Dictionary<DataName, object>();
            Id = Guid.NewGuid();
            Value = 5;
        }

        private bool IsMatchEitherWay(SpellPropertyType type1, SpellPropertyType type2, CardBasicMob card)
        {
            return (card.HasSpellProperty(type1) && this.Type == type2) || (card.HasSpellProperty(type2) && this.Type == type1);
        }

        // Score based on specific card. Ex. DoubleAttack + rage = good, Shield on protector = good
        public float SituationalBonus(CardBasicMob card)
        {
            float result = 0.0f;

            // Don't apply if already on card (no exceptions currently)
            if (card.HasSpellProperty(this.Type))
                result -= 10;

            // DoubleAttack and Rage = good
            if (IsMatchEitherWay(SpellPropertyType.Rage, SpellPropertyType.DoubleAttack, card))
                result += 5;

            if (this.Type == SpellPropertyType.Regen && card.HpPct < 50)
                result += 1;

            if (this.Type == SpellPropertyType.Taunt)
                result += card.Hp;

            if (this.Type == SpellPropertyType.DoubleAttack)
                result += card.Dmg;

            if (this.Type == SpellPropertyType.Revive)
                result += card.Dmg;

            return result;
        }

        public SpellProperty Clone()
        {
            var clone = (SpellProperty)this.MemberwiseClone();
            clone.Data = new Dictionary<DataName, object>(this.Data);
            return clone;
        }

        public int GetAmount()
        {
            return GetData<int>(DataName.Amount);
        }

        public T GetData<T>(DataName dataName)
        {
            return (T)Data[dataName];
        }

        public void SetData<T>(DataName dataName, T value)
        {
            Data[dataName] = value;
        }

        public static SpellProperty AvoidPhysicalDmg(int amount, string onAvoidMsg) { return Create(SpellPropertyType.AvoidPhysicalDmg, amount, onAvoidMsg, Result.Positive); }
        public static SpellProperty LifeDrain(int amount, string onDrainMsg) { return Create(SpellPropertyType.LifeDrain, amount, onDrainMsg, Result.Positive); }
        public static SpellProperty Regen(int amount, string onRegenMsg) { return Create(SpellPropertyType.Regen, amount, onRegenMsg, Result.Positive); }
        public static SpellProperty Charge(string onChargeMsg) { return Create(SpellPropertyType.Charge, onChargeMsg, Result.Positive); }
        public static SpellProperty Sacrifice(string onSacrifice) { return Create(SpellPropertyType.Sacrifice, onSacrifice, Result.Unknown); }
        public static SpellProperty Taunt(string onTaunt) { return Create(SpellPropertyType.Taunt, onTaunt, Result.Positive); }
        public static SpellProperty DoubleAttack() { return Create(SpellPropertyType.DoubleAttack, string.Empty, Result.Positive); }

        public static SpellProperty HealLeftAndRight(int amount, string onHeal) { return Create(SpellPropertyType.HealLeftAndRight, amount, onHeal, Result.Positive); }
        public static SpellProperty ProtectLeftAndRight(string onProtect) { return Create(SpellPropertyType.ProtectLeftAndRight, onProtect, Result.Positive); }

        public static SpellProperty Revive(bool removeAddedProperties, string onReviveMsg)
        {
            var prop = Create(SpellPropertyType.Revive, onReviveMsg, Result.Positive);
            prop.Data[DataName.BoolValue1] = removeAddedProperties;
            return prop;
        }

        public static SpellProperty Rage(string onRageMsg, int maxIncrease)
        {
            var prop = Create(SpellPropertyType.Rage, onRageMsg, Result.Positive);
            prop.Data[DataName.IntegerValue1] = maxIncrease;
            prop.Data[DataName.IntegerValue2] = 0; // Counter
            return prop;
        }

        public static SpellProperty Poisoned(int amount, int maxRounds, string onTickMsg)
        {
            var prop = Create(SpellPropertyType.Poisoned, amount, onTickMsg, Result.Negative);
            prop.Data[DataName.IntegerValue1] = maxRounds;
            prop.Data[DataName.IntegerValue2] = 0; // Counter
            return prop;
        }

        public static SpellProperty Poison(int amount, int maxRounds, string onApplyMyMsg, string onApplyOtherMsg, string onTickMsg)
        {
            var prop = Create(SpellPropertyType.Poison, onApplyMyMsg, Result.Positive);
            prop.Data[DataName.Amount] = amount;
            prop.Data[DataName.IntegerValue1] = maxRounds;
            prop.Data[DataName.String1] = onApplyOtherMsg;
            prop.Data[DataName.String2] = onTickMsg;
            return prop;
        }

        public SpellPropertyType Type { get; set; }
        public Dictionary<DataName, object> Data { get; private set; }
    }

    public enum SpellPropertyType
    {
        AvoidPhysicalDmg, // -x to physical damage taken
        LifeDrain, // Gain x life when successfully attacking
        Revive, // Instantly revive with 1 hp when dying
        Regen, // Gain x hp after every round
        Poison, // Apply poison on successful attack
        Poisoned, // Lose x hp after every round
        Charge, // Attack immediately when placed on board
        Rage, // Gain 1 dmg when attacking
        HealLeftAndRight, // Heal left and right card after attacking
        ProtectLeftAndRight, // Redirect dmg to self
        Taunt, // Force all cards to attack self
        Sacrifice, // Redirect all attacks to self but die on first redirected hit
        DoubleAttack, // Attack twice
    };

    public enum BattleFlowEvent
    {
        AfterEnterBoard,
        BeforeAttacked,
        AfterAttacked,
        BeforeAttacking,
        AfterAttacking,
        BeforeMyTurn,
        AfterMyTurn,
        BeforeOpponentTurn,
        AfterOpponentTurn,
        BeforeRound,
        AfterRound
    };
}
