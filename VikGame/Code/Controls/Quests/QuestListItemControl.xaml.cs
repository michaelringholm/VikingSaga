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
    /// Interaction logic for QuestListItemControl.xaml
    /// </summary>
    public partial class QuestListItemControl : UserControl
    {
        [Description("The Quest object."), Category("Viking")]
        private Quest _quest;
        public Quest Quest 
        {
            get { return _quest; }
            set 
            {
                _quest = value;

                if(_quest != null)
                {
                    QuestTitle.Text = _quest.GetTitle();

                    if (_quest.Status == GameLib.Quests.Quest.QuestStatus.ReadyToTurnIn)
                    {
                        QuestStatus.Text = "(Ready to turn in)";
                        QuestStatus.Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                        QuestStatus.Visibility = System.Windows.Visibility.Hidden;
                }
            }
        }

        [Description("Raised when a quest is selected."), Category("Viking")]
        public event EventHandler<QuestEventArgs> QuestSelected;

        public QuestListItemControl()
        {
            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
                Background = new SolidColorBrush(Colors.Transparent);
        }

        private bool _selected;
        [Description("Whether or not this quest is the selected quest."), Category("Viking")]
        public bool Selected
        {
            set
            {
                _selected = value;
                if (_selected)
                    OuterBorder.BorderThickness = new Thickness(2);
                else
                    OuterBorder.BorderThickness = new Thickness(0);
            }

            get { return _selected;  }
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {            
            if (QuestSelected != null)
                QuestSelected(this, new QuestEventArgs { Quest = Quest });
        }
    }
}
