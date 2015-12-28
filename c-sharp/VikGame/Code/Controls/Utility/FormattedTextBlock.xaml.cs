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

namespace Vik.Code.Controls.Utility
{
    public partial class FormattedTextBlock : UserControl
    {
        private string _formattedText;

        public string FormattedText
        {
            get {  return _formattedText; }
            set { setFormattedText(value); }
        }

        public FormattedTextBlock()
        {
            InitializeComponent();
        }

        private void setFormattedText(string formattedText)
        {
            _formattedText = formattedText;
            UiUtil.SetTextBlockText(this.InnerTextBlock, formattedText);
        }
    }
}
