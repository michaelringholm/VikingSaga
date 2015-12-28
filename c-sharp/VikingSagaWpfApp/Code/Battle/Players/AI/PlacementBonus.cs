using System;
using System.Linq;
using System.Collections.Generic;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs;

namespace VikingSaga.Code.BattleNs.Players.AI
{
    public class AiPlacementBonus
    {
        public enum Hint { PreferFriendsLeftAndRight }

        private List<Hint> _placementHints = new List<Hint>();
        private Dictionary<SpellPropertyType, Hint> _spellPropToHintMap = new Dictionary<SpellPropertyType, Hint>();

        public AiPlacementBonus()
        {
            BuildSpellPropToHintMap();
        }

        private void BuildSpellPropToHintMap()
        {
            _spellPropToHintMap[SpellPropertyType.HealLeftAndRight] = Hint.PreferFriendsLeftAndRight;
            _spellPropToHintMap[SpellPropertyType.ProtectLeftAndRight] = Hint.PreferFriendsLeftAndRight;
        }

        public void AddHint(Hint hint)
        {
            _placementHints.Add(hint);
        }

        public float CalcBonusForAlreadyPlacedCard(CardBasicMob card)
        {
            if (card.BoardPosition == -1)
                throw new ArgumentException("Card is not placed on board");

            var result = 0.0f;

            if (_placementHints.Contains(Hint.PreferFriendsLeftAndRight))
            {
                int boardPos = card.BoardPosition;

                // Prefer not being at edges
                if (boardPos != 0 && boardPos != 4)
                    result += 1;

                var row = card.Owner.Battle.Board.GetRow(card.Owner);

                if (boardPos != 0 && row.Cards[boardPos - 1] != null)
                    result += 1;

                if (boardPos != 4 && row.Cards[boardPos + 1] != null)
                    result += 1;
            }

            return result;
        }

        internal void SpellPropertyAdded(SpellProperty prop)
        {
            Hint hint;
            if (_spellPropToHintMap.TryGetValue(prop.Type, out hint))
            {
                _placementHints.Add(hint);
            }
        }

        internal void SpellPropertyRemoved(SpellProperty prop)
        {
            Hint hint;
            if (_spellPropToHintMap.TryGetValue(prop.Type, out hint))
            {
                _placementHints.Remove(hint);
            }
        }
    }
}
