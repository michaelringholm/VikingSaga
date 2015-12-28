using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Player
{
    public partial class PlayerQuestLogWindow : FakeWindow
    {
        public PlayerQuestLogWindow()
        {
            InitializeComponent();

            Loaded += delegate { AnimHelper.ApplyPopInAnimation(this); };
            InitQuests();
        }

        private void InitQuests()
        {
            QuestListControl.Quests = VikGame.World.PlayerProfile.Data.ActiveQuests;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close(Result.OK);
        }

        private void QuestListControl_SelectedQuestChanged(object sender, Quests.QuestEventArgs e)
        {
            QuestTitle.Text = "" + DateTime.Now;
            QuestDescription.Text = "Lorem ipsum " + DateTime.Now;
        }
    }
}
