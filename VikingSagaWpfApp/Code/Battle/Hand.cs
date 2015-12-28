using VikingSaga.Code;
using VikingSagaWpfApp.Code.Battle.Cards;

namespace VikingSagaWpfApp.Code.Battle
{
    public class Hand
    {
        public BattleCard[] Cards { get; private set; }

        public Hand()
        {
            Cards = new BattleCard[Board.CardsPerRow];
        }

        public bool TryGetFreePosition(out int position)
        {
            return BattleUtil.TryGetFirstNull(Cards, out position);
        }

        public bool TryGetFirstCardPosition(out int position)
        {
            return BattleUtil.TryGetFirstNotNull(Cards, out position);
        }

        public bool TryRemoveFirstCard(out BattleCard card, out int position)
        {
            card = null;

            if (!TryGetFirstCardPosition(out position))
                return false;

            card = Cards[position];
            Cards[position] = null;

            return true;
        }
    }
}
