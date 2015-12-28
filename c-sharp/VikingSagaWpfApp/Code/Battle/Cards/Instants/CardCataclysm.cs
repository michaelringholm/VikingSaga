using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingSagaWpfApp.Code.BattleNs.Cards
{
    public class CardCataclysm : CardInstantCustom
    {
        public CardCataclysm()
        {
            Name = "Cataclysm";
            Description = "Deals 50 dmg to ALL cards!";

            CanTargetOwnPlayer = true;
            CanTargetOwnCard = true;
            CanTargetEnemyPlayer = true;
            CanTargetEnemyCard = true;
        }

        public override SpellProperty.Result Effect { get { return SpellProperty.Result.Positive; } }

        public override void Execute()
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
