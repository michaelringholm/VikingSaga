using System.Windows;

namespace Vik
{
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            VikGame.ApplicationStartup();
        }
    }
}
