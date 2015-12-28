using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameLib.Battles.Cards.Base
{
    public static class CardSerializer
    {
        // [cardId]|[dead 0/1]|*[upgrades]
        private const char SplitChar = '|';

        public static string IdFromCard(Card card)
        {
            var sb = new StringBuilder(50);
            sb.Append(card.GetType().FullName);
            sb.Append(SplitChar);
            sb.Append(card.NeedRevive);
            sb.Append(SplitChar);
            sb.Append(string.Join(SplitChar.ToString(), card.UpgradeCards));
            return sb.ToString();
        }

        public static Card CardFromId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return null;

            var items = id.Split(SplitChar);
            var mainCard = CreateCardInstance(items[0]);
            if (items.Count() > 1)
            {
                var needRevive = bool.Parse(items[1]);
                mainCard.NeedRevive = needRevive;

                for (int i = 2; i < items.Length; ++i)
                {
                    var item = items[i];
                    if (string.IsNullOrEmpty(item))
                        continue;

                    var upgradeCard = CreateCardInstance(item);
                    if (upgradeCard is CardBattle)
                        mainCard.Upgrade((CardBattle)upgradeCard);
                }
            }

            return mainCard;
        }

        private static Card CreateCardInstance(string id)
        {
            var cardType = Type.GetType(id);
            if (!typeof(CardBattle).IsAssignableFrom(cardType))
                throw new ArgumentException("Type must inherit from " + typeof(CardBattle).Name);

            var result = Activator.CreateInstance(cardType);
            return (CardBattle)result;
        }
    }
}
