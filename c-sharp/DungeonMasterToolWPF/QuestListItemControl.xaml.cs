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

namespace DungeonMasterToolWPF
{
    /// <summary>
    /// Interaction logic for QuestListItemControl.xaml
    /// </summary>
    public partial class QuestListItemControl : UserControl
    {
        public String QuestId
        {
            get { return tbQuestID.Text; }
            set { tbQuestID.Text = value; }
        }

        public String QuestTitle
        {
            get { return tbQuestTitle.Text; }
            set { tbQuestTitle.Text = value; }
        }

        public QuestListItemControl()
        {
            InitializeComponent();
        }

        private void tbEdit_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Quest clicked");
        }
    }
}
