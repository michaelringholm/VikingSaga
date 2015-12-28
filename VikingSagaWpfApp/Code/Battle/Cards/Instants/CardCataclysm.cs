using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.Battle.Cards
{
    public class CardCataclysm : CardInstantCustom
    {
        public CardCataclysm()
        {
            Name = "Cataclysm";
            Description = "Deals 50 dmg to ALL cards!";
            ExecuteAction = Do;

            CanTargetOwnPlayer = true;
            CanTargetOwnCard = true;
            CanTargetEnemyPlayer = true;
            CanTargetEnemyCard = true;

            Effect = SpellProperty.Result.Unknown;
        }

        private void Do()
        {
            var cards = this.Owner.Battle.Board.AllCards();
            foreach (var card in cards)
            {
                card.AddUiSpellOutput("<Cataclysm>");
                card.ChangeHp(-50, HpChangeType.Physical);
            }

            Observer.ShowNotifications();
        }
    }
}
