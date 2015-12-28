using GameLib.Encounters;
using System.Windows;
using Vik.Code.Controls.Utility;

namespace Vik.Code.Controls.Battle
{
    public partial class EncounterWinWindow : FakeWindow
    {
        public EncounterWinWindow(Encounter encounter)
        {
            InitializeComponent();

            UiUtil.SetTextBlockText(tbGold, string.Format("You receive <B><C ORANGE>{0}</B><C DEFAULT> gold!", encounter.Treasure.Gold));

            string cards = "";
            if (encounter.Treasure.Cards != null)
            {
                foreach (var card in encounter.Treasure.Cards)
                    cards += card.Name;
            }

            UiUtil.SetTextBlockText(tbCards, string.Format("You receive <B><C ORANGE>{0}</B><C DEFAULT> cards!", cards));
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Close(Result.OK);
        }
    }
}
