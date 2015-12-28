using System.Windows;
using System.Windows.Input;
using Vik.Code.Utility;

namespace Vik.Code.Controls.Login
{
    public partial class EnterProfileNameWindow : FakeWindow
    {
        public EnterProfileNameWindow()
        {
            InitializeComponent();
            tbName.PreviewKeyDown += tbName_PreviewKeyDown;
            tbName.TextChanged += tbName_TextChanged;
            Loaded += delegate { AnimHelper.ApplyPopInAnimation(this); };
        }

        void tbName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string cleaned = string.Empty;
            foreach(char c in tbName.Text)
            {
                if (char.IsLetterOrDigit(c))
                    cleaned += c;
            }
            tbName.Text = cleaned;
        }

        void tbName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Close(Result.OK);
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close(Result.Cancel);
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Close(Result.OK);
        }
    }
}
