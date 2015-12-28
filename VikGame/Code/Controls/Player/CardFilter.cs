using GameLib.Battles.Cards;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Vik.Code.Controls.Player
{
    public class CardFilter
    {
        public CardFilter()
        {
            Selected = false;
        }

        public Boolean Selected { get; set; }
        public CardBattle.CardFlagsEnum Flag { get; set; }

        public static CardBattle.CardFlagsEnum BuildFlags(IEnumerable<CardFilterControl> cardFilterControls)
        {
            CardBattle.CardFlagsEnum flags = CardBattle.CardFlagsEnum.Null;

            foreach (CardFilterControl cardFilterControl in cardFilterControls)
            {
                if (cardFilterControl.Selected)
                    flags = flags | cardFilterControl.CardFlag;
            }
            return flags;
        }

        public static void FilterCards(List<CardFilterControl> cardFilterControls, Cards.CardScrollList allCardsScrollList, List<String> cards)
        {
            CardBattle.CardFlagsEnum flags = BuildFlags(cardFilterControls);

            var filteredCards = CardBattle.FilterCards(cards, flags);
            allCardsScrollList.SetCardsIds(filteredCards.ToList());
        }
    }
}
