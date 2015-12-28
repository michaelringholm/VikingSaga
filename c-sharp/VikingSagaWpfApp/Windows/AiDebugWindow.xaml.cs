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
using System.Windows.Shapes;
using VikingSagaWpfApp.Code.BattleNs.Players.AI;

namespace VikingSaga.Windows
{
    /// <summary>
    /// Interaction logic for AiDebugWindow.xaml
    /// </summary>
    public partial class AiDebugWindow : Window
    {
        public AiDebugWindow()
        {
            InitializeComponent();
        }

        private List<AiPlay> _plays;

        public void SetPlays(IEnumerable<AiPlay> plays, int ms)
        {
            _plays = plays.OrderByDescending(p => p.score).ToList();

            ListBox2.Items.Clear();
            ListBox.Items.Clear();

            foreach(var play in _plays)
            {
                ListBox.Items.Add(play.TargetString());
            }

            Title = string.Format("AiDebugWindow - Showing {0} possible plays, tested in {1} ms", _plays.Count, ms);
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox2.Items.Clear();
            if (_plays == null || ListBox.SelectedIndex == -1)
                return;

            var play = _plays[ListBox.SelectedIndex];
            foreach(var line in play.scoreDebugInfoMe)
            {
                ListBox2.Items.Add(line);
            }

            foreach (var line in play.scoreDebugInfoOther)
            {
                ListBox2.Items.Add(line);
            }
        }
    }
}
