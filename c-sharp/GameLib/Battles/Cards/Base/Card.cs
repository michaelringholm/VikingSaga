using GameLib.Battles.Cards.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLib.Battles.Cards
{
    public class Card
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        // Belongs in BasicMob (or rather a common base of follower and minion).
        public bool NeedRevive { get; set; }
        public readonly List<string> UpgradeCards = new List<string>();

        public enum CardRarity { Common, Rare, Epic };

        [Flags]
        public enum CardFlagsEnum
        {
            Null = 1 << 0,
            Druid = 1 << 1,
            Hunter = 1 << 2,
            Mage = 1 << 3,
            Priest = 1 << 4,
            Warrior = 1 << 5,
            Buff = 1 << 6,
            DeBuff = 1 << 7,
            Heal = 1 << 8,
            DoT = 1 << 9,
            DD = 1 << 10,
            Minion = 1 << 11,
            Upgrade = 1 << 12,
            NPC = 1 << 13,
            All = -1,
            Follower = Druid|Hunter|Mage|Priest|Warrior,
        };

        public CardFlagsEnum CardFlags { get; protected set; }
        public CardRarity Rarity { get; set; }
        public int Level { get; protected set; }
        public int Price { get { return Level * 10; } }
        public int ReviveCost { get { return Level * 2; } }

        public Card()
        {
            Rarity = CardRarity.Common;
            Level = 1;
        }

        public bool HasAnyFlag(CardFlagsEnum flags)
        {
            return (this.CardFlags & flags) != 0;
        }

        public bool HasAllFlags(CardFlagsEnum flags)
        {
            return (this.CardFlags & flags) == flags;
        }

        public static IEnumerable<string> FilterCards(IEnumerable<string> cards, CardFlagsEnum flags)
        {
            var filtered = cards.Where(id => !string.IsNullOrEmpty(id) && (CardFromId(id).CardFlags & flags) != 0);
            return filtered;
        }

        public static string IdFromType<T>() where T : CardBattle
        {
            var card = (Card)Activator.CreateInstance<T>();
            return IdFromCard(card);
        }

        public static string IdFromCard(Card card)
        {
            return CardSerializer.IdFromCard(card);
        }

        public static IEnumerable<string> IdsFromCards(IEnumerable<Card> cards)
        {
            foreach (var card in cards)
                yield return IdFromCard(card);
        }

        public static Card CardFromId(string id)
        {
            return CardSerializer.CardFromId(id);
        }

        public static IEnumerable<Card> CardsFromIds(IEnumerable<string> ids)
        {
            foreach (var id in ids)
                yield return CardFromId(id);
        }

        public void Upgrade(CardBattle otherCard)
        {
            UpgradeCards.Add(IdFromCard(otherCard));
            // TODO: Apply otherCard to this card
        }

        public string GetId()
        {
            return Card.IdFromCard(this);
        }

    }
}
