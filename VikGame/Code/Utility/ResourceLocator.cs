using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;

namespace Vik.Code.Utility
{
    public class ResourceLocator
    {
        public ImageSource GetImageResource(string resourcePath, bool throwOnError = true)
        {
            string uriStr = "pack://application:,,,/" + resourcePath.ToLower();
            try
            {
                var uri = new Uri(uriStr);
                return new BitmapImage(uri);
            }
            catch(Exception e)
            {
                if (!throwOnError)
                    return null;

                ShowLocateError(resourcePath, uriStr, e);
                return new BitmapImage();
            }
        }

        public Cursor GetCursorResource(string cursorResourcePath)
        {
            StreamResourceInfo sriCurs = System.Windows.Application.GetResourceStream(GetCursorResourceUri("coins.cur"));
            return new Cursor(sriCurs.Stream);
        }

        public Uri GetCursorResourceUri(string cursorResourcePath)
        {
            string uriStr = "pack://application:,,,/Data/Gfx/Cursors/" + cursorResourcePath.ToLower();
            var uri = new Uri(uriStr);
            return uri;
        }

        public Uri GetSoundResourceUri(string soundResourcePath)
        {
            string uriStr = "pack://application:,,,/Data/Sound/" + soundResourcePath.ToLower();
            var uri = new Uri(uriStr);
            return uri;
        }

        public Stream GetSoundResourceStream(string soundResourcePath)
        {
            string uriStr = "pack://application:,,,/Data/Sound/" + soundResourcePath.ToLower();
            try
            {
                var uri = new Uri(uriStr);
                return System.Windows.Application.GetResourceStream(uri).Stream;
            }
            catch (Exception e)
            {
                ShowLocateError(soundResourcePath, uriStr, e);
            }
            return null;
        }

        private void ShowLocateError(string resourcePath, string uriStr, Exception e)
        {
            var matchInfo = VikGame.ResourceChecker.DebugInfo(resourcePath);
            var userMsg = string.Format("Resource looked up:\n\n{0}\n\nException:\n\n{1}\n\nSimilar resources found:\n\n{2}\n\n", uriStr, e.Message, matchInfo);
            MessageBox.Show(userMsg, "Error locating resource", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
