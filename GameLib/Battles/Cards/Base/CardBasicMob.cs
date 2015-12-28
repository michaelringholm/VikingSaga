using System;
using System.Linq;
using System.Collections.Generic;
using VikingSaga.Code.Battles.Players.AI;

namespace GameLib.Battles.Cards
{
    // Base class for HP / Dmg
    public class CardBasicMob : CardBattle
    {
        private List<SpellProperty> _spellProperties = new List<SpellProperty>();
        private List<SpellProperty> _startingProperties = new List<SpellProperty>();
        private AiPlacementBonus _placementBonus = new AiPlacementBonus();

        public int BoardPosition { get; set; }

        public CardBattle.HpChangeTypeEnum HpChangeType { get; protected set; }

        public int HpBase { get; protected set; }
        public int HpSpell { get; private set; }
        public int HpMax { get { return HpBase + HpSpell; } }
        public int Hp { get { return _currentHp + HpSpell; } }
        public float HpPct { get { return ((float)Hp / HpMax) * 100; } }
        private int _currentHp;
        public bool IsDead { get { return Hp <= 0; } }

        public int DmgBase { get; protected set; }
        public int DmgSpell { get; private set; }
        public int Dmg { get { return Math.Max(DmgBase + DmgSpell, 0); } }

        public CardBasicMob()
        {
            Name = "CardBasicMob";
            HpChangeType = CardBattle.HpChangeTypeEnum.Physical;
            CanTargetOwnBoard = true;
            BoardPosition = -1;
            EnableUiOutput = true;
        }

        private IEnumerable<SpellProperty> CloneSpellProperties(IEnumerable<SpellProperty> other)
        {
            foreach (var prop in other)
                yield return prop.Clone();
        }

        public override CardBattle Clone()
        {
            var clone = (CardBasicMob)base.Clone();
            clone._spellProperties = CloneSpellProperties(_spellProperties).ToList();
            clone._startingProperties = CloneSpellProperties(_startingProperties).ToList();
            return clone;
        }

        public override void Init()
        {
            _currentHp = HpBase;
            _startingProperties.AddRange(_spellProperties);
        }

        public void AddUiSpellOutput(string msg)
        {
            if (!string.IsNullOrWhiteSpace(msg))
                AddUiOutput(msg);
        }

        public void AddMainUiSpellOutput(SpellProperty prop)
        {
            string msg = prop.GetData<string>(SpellProperty.DataName.MainMsg);
            AddUiOutput(msg);
        }

        public bool HasAnySpellProperty()
        {
            return _spellProperties.Any();
        }

        public int SpellPropertyValue()
        {
            return _spellProperties.Sum(prop => prop.Value);
        }

        public virtual float CalcLocationBonus()
        {
            return _placementBonus.CalcBonusForAlreadyPlacedCard(this);
        }

        public void AddSpellProperty(SpellProperty prop)
        {
            _placementBonus.SpellPropertyAdded(prop);
            _spellProperties.Add(prop);
        }

        public bool HasSpellProperty(SpellPropertyType propType)
        {
            return _spellProperties.Exists((sp) => sp.Type == propType);
        }

        public void RemoveSpellProperty(SpellProperty prop)
        {
            _placementBonus.SpellPropertyRemoved(prop);
            _spellProperties.Remove(prop);
        }

        public SpellProperty RemoveSpellProperty(SpellPropertyType propType)
        {
            var item = GetSpellProperty(propType);
            if (item == null)
                throw new InvalidOperationException("SpellPropertyType not found: " + propType.ToString());

            RemoveSpellProperty(item);
            return item;
        }

        public bool TryRemoveSpellProperty(SpellPropertyType propType, out SpellProperty prop)
        {
            TryGetSpellProperty(propType, out prop);
            if (prop != null)
                RemoveSpellProperty(prop);

            return prop != null;
        }

        public SpellProperty GetSpellProperty(SpellPropertyType propType)
        {
            var item = _spellProperties.FirstOrDefault((sp) => sp.Type == propType);
            if (item == null)
                throw new InvalidOperationException("SpellPropertyType not found: " + propType.ToString());

            return item;
        }

        public bool TryGetSpellProperty(SpellPropertyType propType, out SpellProperty prop)
        {
            prop = _spellProperties.FirstOrDefault((sp) => sp.Type == propType);
            return (prop != null);
        }

        public void ForceSetHp(int amount)
        {
            _currentHp = amount;
        }

        private void CheckRevive()
        {
            if (!HasSpellProperty(SpellPropertyType.Revive))
                return;

            if (Hp <= 0)
            {
                var prop = GetSpellProperty(SpellPropertyType.Revive);
                bool removeAddedProperties = prop.GetData<bool>(SpellProperty.DataName.BoolValue1);
                if (removeAddedProperties)
                {
                    _spellProperties.Clear();
                    _spellProperties.AddRange(_startingProperties);
                }

                // Remove all Revive. Multiple could exist via buffs.
                while (HasSpellProperty(SpellPropertyType.Revive))
                {
                    RemoveSpellProperty(SpellPropertyType.Revive);
                }

                AddMainUiSpellOutput(prop);
                ForceSetHp(1);
            }
        }

        private void CheckDodge(ref int amount, HpChangeTypeEnum changeType)
        {
            if (amount >= 0 || changeType != CardBattle.HpChangeTypeEnum.Physical)
                return;

            SpellProperty prop;
            if (TryGetSpellProperty(SpellPropertyType.AvoidPhysicalDmg, out prop))
            {
                int dodgeAmount = prop.GetAmount();
                amount += dodgeAmount;
                if (amount > 0)
                    amount = 0;

                AddMainUiSpellOutput(prop);
            }
        }

        private void CheckLifeDrain(int amount)
        {
            if (!HasSpellProperty(SpellPropertyType.LifeDrain) || amount > -1)
                return;

            var prop = GetSpellProperty(SpellPropertyType.LifeDrain);
            int drainAmount = -prop.GetAmount();
            if (drainAmount < amount)
                drainAmount = amount;

            SpellPropertyHpChange(-drainAmount, prop, CardBattle.HpChangeTypeEnum.Physical); // Physical, or?
        }

        private void CheckRage()
        {
            SpellProperty prop;
            if (TryGetSpellProperty(SpellPropertyType.Rage, out prop))
            {
                int max = prop.GetData<int>(SpellProperty.DataName.IntegerValue1);
                int counter = prop.GetData<int>(SpellProperty.DataName.IntegerValue2);
                if (counter < max)
                {
                    DmgSpell++;
                    AddMainUiSpellOutput(prop);
                    Observer.ShowNotifications();
                    Observer.ShowCardDmgChange(this, 1);
                    if (++counter < max)
                    {
                        prop.SetData<int>(SpellProperty.DataName.IntegerValue2, counter);
                    }
                }

                if (counter >= max)
                {
                    RemoveSpellProperty(prop);
                }
            }
        }

        private void CheckRegen()
        {
            if (!HasSpellProperty(SpellPropertyType.Regen))
                return;

            var prop = GetSpellProperty(SpellPropertyType.Regen);
            int amount = prop.GetAmount();
            SpellPropertyHpChange(amount, prop, CardBattle.HpChangeTypeEnum.Physical);
        }

        private void CheckPoisoned()
        {
            if (!HasSpellProperty(SpellPropertyType.Poisoned))
                return;

            var prop = GetSpellProperty(SpellPropertyType.Poisoned);
            int amount = prop.GetAmount();
            int maxRounds = prop.GetData<int>(SpellProperty.DataName.IntegerValue1);
            int count = prop.GetData<int>(SpellProperty.DataName.IntegerValue2);
            if (count < maxRounds)
            {
                SpellPropertyHpChange(-amount, prop, CardBattle.HpChangeTypeEnum.Poison);
                if (++count < maxRounds)
                {
                    prop.SetData<int>(SpellProperty.DataName.IntegerValue2, count);
                }
            }

            if (count >= maxRounds)
            {
                RemoveSpellProperty(prop);
            }
        }

        private void CheckPoison(int amount, CardBasicMob dst)
        {
            if (!HasSpellProperty(SpellPropertyType.Poison) || amount > -1 || dst == null || dst.IsDead)
                return;

            // Cannot poison if already poisened. Should check for bigger amount
            if (dst.HasSpellProperty(SpellPropertyType.Poisoned))
                return;

            var poisonProp = GetSpellProperty(SpellPropertyType.Poison);
            int newAmount = poisonProp.GetAmount();
            int maxRounds = poisonProp.GetData<int>(SpellProperty.DataName.IntegerValue1);

            var newPoison = SpellProperty.Poisoned(newAmount, maxRounds, poisonProp.GetData<string>(SpellProperty.DataName.String2));
            dst.AddSpellProperty(newPoison);

            dst.AddUiSpellOutput(poisonProp.GetData<string>(SpellProperty.DataName.String1)) ; // Show 'poisened' message on dst when applied
            AddMainUiSpellOutput(poisonProp); // Show msg on self, if any
            Observer.ShowNotifications();
        }

        private void DoHeal(CardBasicMob dst, int amount)
        {
            if (dst == null)
                return;

            dst.ChangeHp(amount, CardBattle.HpChangeTypeEnum.Magic);
            Observer.ShowCardHpChange(dst, amount);
        }

        private void CheckHealLeftRight()
        {
            if (!HasSpellProperty(SpellPropertyType.HealLeftAndRight))
                return;

            var prop = GetSpellProperty(SpellPropertyType.HealLeftAndRight);
            var amount = prop.GetAmount();

            var left = Owner.Battle.Board.GetCardAt(this.Owner, this.BoardPosition - 1);
            var right = Owner.Battle.Board.GetCardAt(this.Owner, this.BoardPosition + 1);
            DoHeal(left, amount);
            DoHeal(right, amount);

            if (left != null || right != null)
            {
                AddMainUiSpellOutput(prop); // Show msg on self, if any
                Observer.ShowNotifications();
            }
        }

        private void SpellPropertyHpChange(int amount, SpellProperty prop, CardBattle.HpChangeTypeEnum amountType)
        {
            ChangeHp(amount, amountType);

            AddMainUiSpellOutput(prop);

            Observer.ShowNotifications();
            Observer.ShowCardHpChange(this, amount);
        }

        public override void OnDamageDone(int amount, CardBasicMob dst)
        {
            CheckLifeDrain(amount);
            CheckPoison(amount, dst);
            CheckRage();
        }

        public override void OnDamageDone(int amount)
        {
            OnDamageDone(amount, null);
        }

        public override void OnBattleFlowEvent(BattleFlowEvent flowEvent, object data = null)
        {
            if (IsDead)
                return;

            switch(flowEvent)
            {
                case BattleFlowEvent.AfterAttacking:
                    CheckHealLeftRight();
                    break;

                case BattleFlowEvent.AfterRound:
                    CheckRegen();
                    CheckPoisoned();
                    break;
            }
        }

        public int ChangeHp(int amount, HpChangeTypeEnum changeType)
        {
            CheckDodge(ref amount, changeType);

            _currentHp += amount;

            if (_currentHp > HpBase)
            {
                // Cannot heal higher than base HP
                _currentHp = HpBase;
            }

            if (Hp <= 0)
            {
                CheckRevive();
            }

            return amount;
        }

        public void ChangeHpSpell(int amount)
        {
            HpSpell += amount;
        }

        public void ChangeDmgSpell(int amount)
        {
            DmgSpell += amount;
        }
    }
}
