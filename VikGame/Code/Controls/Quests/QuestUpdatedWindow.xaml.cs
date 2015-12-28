using GameLib.Quests;
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

namespace Vik.Code.Controls.Quests
{
    /// <summary>
    /// Interaction logic for NewQuestWindow.xaml
    /// </summary>
    public partial class QuestUpdatedWindow : FakeWindow
    {
        public QuestUpdatedWindow()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Close(Result.OK);
        }

        private Quest _quest;
        public void SetQuest(Quest quest)
        {
            _quest = quest;
            tbTitle.Text = quest.GetTitle();
            tbDescription.Text = quest.GetQuestCompletedDescription();
            var reward = quest.GetQuestReward();

            tbGoldReward.Text = reward.Gold.ToString();
            foreach (var card in reward.Cards)
                tbRewardedCards.Text = card.Name;
        }
    }
}
