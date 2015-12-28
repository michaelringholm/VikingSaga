using System;

namespace VikingSagaWpfApp.Code.BattleNs
{
    public class TestPlayer : Player
    {
        public void CopyFrom(Player other)
        {
            this.Turn = other.Turn;
            this.Hp = other.Hp;
            this.Mana = other.Mana;
            this.Deck.CopyFrom(other.Deck);
            this.Hand.CopyFrom(other.Hand, this);
        }

        public override void TakeTurn()
        {
        }
    }
}
