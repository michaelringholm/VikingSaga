using GameLib.Quests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for QuestListControl.xaml
    /// </summary>
    public partial class QuestListControl : UserControl
    {
        public QuestListControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
                Background = new SolidColorBrush(Colors.Transparent);
        }

        //[Description("A list of quest."), Category("Viking")]
        //public event List<String> SelectedQuestChanged;

        [Description("Raised when a quest is selected."), Category("Viking")]
        public event EventHandler<QuestEventArgs> SelectedQuestChanged;

        private void QuestListItemControl_QuestSelected(object sender, QuestEventArgs e)
        {
            foreach (var questItem in QuestListPanel.Children)
                if (questItem is QuestListItemControl)
                    ((QuestListItemControl)questItem).Selected = false;

            ((QuestListItemControl)sender).Selected = true;

            if (SelectedQuestChanged != null)
                SelectedQuestChanged(this, e);
        }

        private List<Quest> _quests;
        public List<Quest> Quests 
        { 
            get
            {
                return _quests;
            }
            
            set
            {
                _quests = value;
                QuestListPanel.Children.Clear();

                if (_quests == null || _quests.Count() <= 0)
                    EmptyQuestListText.Visibility = System.Windows.Visibility.Visible;
                else
                    EmptyQuestListText.Visibility = System.Windows.Visibility.Hidden;

                foreach(var quest in _quests)
                    QuestListPanel.Children.Add(new QuestListItemControl { Quest = quest });
            }
        }
    }
}
