using GameLib.Encounters;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Vik.Code.Controls.Utility;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Battle
{
    public partial class PreEncounterWindow : FakeWindow
    {
        public PreEncounterWindow(Encounter encounter)
        {
            InitializeComponent();

            if(DesignerProperties.GetIsInDesignMode(this))
                Background = new SolidColorBrush(Colors.Transparent);

            UiUtil.SetTextBlockText(tbTitle, encounter.Title);
            UiUtil.SetTextBlockText(tbDescription, encounter.Description);
            EncounterCard.SetCard(encounter.DisplayCard, Cards.CardControl.StatDisplayFlags.None);

            Loaded += delegate { AnimHelper.ApplyPopInAnimation(this); };
        }

        private void btnFight_Click(object sender, RoutedEventArgs e)
        {
            Close(Result.OK);
        }
    }
}
