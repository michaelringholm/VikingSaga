using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Vik.Code.Controls.Utility;

namespace Vik.Code.Controls
{
    // To inherit from this do the following:
    //
    // Code behind (one change):
    //     public partial class EnterProfileNameWindow : FakeWindow
    //
    // XAML (two changes):
    //     <vik:FakeWindow x:Class="Vik.Code.Controls.Login.EnterProfileNameWindow" ...
    // 
    //     xmlns:vik="clr-namespace:Vik.Code.Controls"
    //
    public class FakeWindow : UserControl
    {
        public event EventHandler OnClosing = delegate {};

        public bool RemoveAllAdornersOnClose { get; set; }

        public enum Result { NotSet, OK, Cancel, Yes, No };

        public Result DialogResult { get; set; }

        public FakeWindow()
            : base()
        {
        }

        public string ResultAsDisplayString(Result result)
        {
            return result.ToString();
        }

        public void Close(Result dialogResult = Result.Cancel)
        {
            if (RemoveAllAdornersOnClose)
                UiUtil.RemoveAllGlobalControlAdorners();

            OnClosing(this, EventArgs.Empty);

            DialogResult = dialogResult;
            Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
