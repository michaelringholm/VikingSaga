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
using VikingSaga.Code;

namespace VikingSagaWpfApp
{
    /// <summary>
    /// Interaction logic for EditDeckWindow.xaml
    /// </summary>
    public partial class EditDeckWindow : Window
    {
        public EditDeckWindow(VikingSagaUserProfile profile)
        {
            InitializeComponent();
        }
    }
}
