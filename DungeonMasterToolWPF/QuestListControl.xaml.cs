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
    /// Interaction logic for QuestListControl.xaml
    /// </summary>
    public partial class QuestListControl : UserControl
    {
        public QuestListControl()
        {
            InitializeComponent();

            QuestList.Items.Add(new QuestListItemControl { QuestId = "1", QuestTitle = "Quest 1" });
            QuestList.Items.Add(new QuestListItemControl { QuestId = "2", QuestTitle = "Quest 2" });
            QuestList.Items.Add(new QuestListItemControl { QuestId = "3", QuestTitle = "Quest 3" });
        }
    }
}
