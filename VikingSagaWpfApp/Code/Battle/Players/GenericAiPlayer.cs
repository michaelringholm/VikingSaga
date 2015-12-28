using System;
using System.Linq;
using VikingSagaWpfApp.Code.Battle.Cards;

namespace VikingSagaWpfApp.Code.Battle
{
    class GenericAiPlayer : Player
    {
        private Random _rnd = new Random(DateTime.UtcNow.Millisecond);

        public override void TakeTurn(Battle battle)
        {
            while (true)
            {
                int count = AiPlaceCardsSimple(battle);
                if (count == 0)
                    break;
            }
        }

        CardBasicMob GetRandomBoardCard(Player player)
        {
            var cards = Battle.Board.AllCards(player).ToList();
            return cards.Count() == 0 ? null : cards[_rnd.Next(cards.Count)];
        }

        public int AiPlaceCardsSimple(Battle battle)
        {
            int count = 0;
            var row = battle.Board.GetRow(this);

            int prevCount = 0;
            for (int handPosition = 0; handPosition < 5; ++handPosition)
            {
                if (count != prevCount)
                {
                    Observer.AiArtificialDelay();
                    prevCount = count;
                }

                var card = Hand.Cards[handPosition];

                if (card == null)
                    continue;

                if (card.Mana > this.Mana)
                    continue;

                if (card is CardBasicMob)
                {
                    int boardPosition;
                    if (!row.TryGetFreePosition(out boardPosition))
                        continue;

                    DropCardOnBoard((CardBasicMob)card, handPosition, boardPosition, true);
                    count++;
                }
                else if (card is CardInstant)
                {
                    // Can be simplified a lot
                    if (card is CardInstantCustom)
                    {
                        var c = (CardInstantCustom)card;
                        if (c.Effect == SpellProperty.Result.Negative)
                        {
                            if (c.CanTargetEnemyCard)
                            {
                                var dstCard = GetRandomBoardCard(Battle.GetOpponent(this));
                                if (dstCard == null)
                                    continue;

                                DropCardOtherCard((CardInstantCustom)card, dstCard, true);
                                count++;
                            }
                            else if (c.CanTargetEnemyPlayer)
                            {
                                DropCardOnPlayer(Battle.GetOpponent(this), c, true);
                                count++;
                            }
                        }
                        else
                        {
                            if (c.CanTargetOwnCard)
                            {
                                var dstCard = GetRandomBoardCard(this);
                                if (dstCard == null)
                                    continue;

                                DropCardOtherCard((CardInstantCustom)card, dstCard, true);
                                count++;
                            }
                            else if (c.CanTargetOwnPlayer)
                            {
                                DropCardOnPlayer(this, c, true);
                                count++;
                            }
                        }
                    }
                    else if (card is CardInstantDmgChange)
                    {
                        var c = (CardInstantDmgChange)card;
                        var targetPlayer = c.Amount < 0 ? Battle.GetOpponent(this) : this;
                        var dstCard = GetRandomBoardCard(targetPlayer);
                        if (dstCard == null)
                            continue;

                        DropCardOtherCard((CardInstantDmgChange)card, dstCard, true);
                        count++;
                    }
                    else if (card is CardInstantSpellProperty)
                    {
                        var c = (CardInstantSpellProperty)card;
                        var targetPlayer = c.Effect == SpellProperty.Result.Negative ? Battle.GetOpponent(this) : this;
                        var dstCard = GetRandomBoardCard(targetPlayer);
                        if (dstCard == null)
                            continue;

                        DropCardOtherCard((CardInstantSpellProperty)card, dstCard, true);
                        count++;
                    }
                    else if (card is CardInstantManaChange || (card is CardInstantHpChange && ((CardInstantHpChange)card).Amount > 0))
                    {
                        DropCardOnPlayer(this, (CardInstant)card, true);
                        count++;
                    }
                    else
                    {
                        DropCardOnPlayer(battle.Player1, (CardInstant)card, true);
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
