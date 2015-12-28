using GameLib.Encounters;
using System.Windows;
using Vik.Code.Controls.Utility;

namespace Vik.Code.Controls.Battle
{
    public partial class EncounterLoseWindow : FakeWindow
    {
        public EncounterLoseWindow(Encounter encounter)
        {
            InitializeComponent();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            Close(Result.OK);
        }
    }
}
