using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using VikingSagaWpfApp;

namespace VikingSaga.Code.Resources
{
    class ResourceManager
    {
        internal static MediaPlayer player;
        internal static void CheckResources()
        {
            var packUri = new Uri("pack://application:,,,/VikingSagaWpfApp;component/Resources/backgrounds/dead-wood.jpg");
            var bitmap = new BitmapImage(packUri);

            player = new MediaPlayer();
            player.Open(ResourceManager.getPackedUri("music/medieval-fantasy1.mp3"));
            player.MediaFailed += (o, args) =>
            {
                MessageBox.Show("Media Failed!!");
            };
            player.Play();
            //MediaTimeline timeline = new MediaTimeline(ResourceManager.getPackedUri("music/medieval-fantasy1.mp3"));
            //timeline.RepeatBehavior = RepeatBehavior.Forever;
            //MediaClock clock = timeline.CreateClock();
            //
            //_mediaPlayer.Clock = clock;
        }

        internal static Uri getPackedUri(String relativeFileName)
        {
            var packUri = new Uri("pack://application:,,,/VikingSagaWpfApp;component/Resources/" + relativeFileName);
            return packUri;
        }

        public enum ImageEnum { BloodSkull_150, VikingBattle_1600, GeneralBackground_1600, VikingVillage_1280, UnknownHeroCard, BattleWonBackground, BattleLostBackground, WorldMap }

        internal static Image GetImage(String imageFileName)
        {
            if (String.IsNullOrEmpty(imageFileName))
                throw new Exception("imageFileName was null or empty.");

            var image = new Image();
            var src = new BitmapImage();
            src.BeginInit();
            src.UriSource = getPackedUri(imageFileName);
            src.CacheOption = BitmapCacheOption.OnLoad;
            src.EndInit();
            image.Source = src;

            return image;
        }

        internal static Image GetImage(ImageEnum imageEnum)
        {
            String imageFileName = null;

            if (imageEnum == ImageEnum.BloodSkull_150)
                imageFileName = "blood-skull-150x150.png";
            else if (imageEnum == ImageEnum.VikingBattle_1600)
                imageFileName = @"backgrounds/battle-form-background-1900x1200.jpg";
            else if (imageEnum == ImageEnum.GeneralBackground_1600)
                imageFileName = @"backgrounds/general-form-background-1900x1200.jpg";
            else if (imageEnum == ImageEnum.VikingVillage_1280)
                imageFileName = @"backgrounds/viking-village-1280x720.jpg";
            else if (imageEnum == ImageEnum.UnknownHeroCard)
                imageFileName = @"cards/card-unknown.png";
            else if (imageEnum == ImageEnum.BattleWonBackground)
                imageFileName = @"backgrounds/loot2.png";
            else if (imageEnum == ImageEnum.BattleLostBackground)
                imageFileName = @"backgrounds/you-were-defeated.jpg";

            return GetImage(imageFileName);
        }

        internal static Brush GetImageBrush(ImageEnum imageEnum)
        {
            String imageFileName = null;

            if (imageEnum == ImageEnum.BloodSkull_150)
                imageFileName = "blood-skull-150x150.png";
            else if (imageEnum == ImageEnum.VikingBattle_1600)
                imageFileName = @"backgrounds/battle-form-background-1900x1200.jpg";
            else if (imageEnum == ImageEnum.GeneralBackground_1600)
                imageFileName = @"backgrounds/general-form-background-1900x1200.jpg";
            else if (imageEnum == ImageEnum.VikingVillage_1280)
                imageFileName = @"backgrounds/viking-village-1280x720.jpg";
            else if (imageEnum == ImageEnum.UnknownHeroCard)
                imageFileName = @"cards/card-unknown.png";
            else if (imageEnum == ImageEnum.WorldMap)
                imageFileName = @"maps/map1_viking_saga.png";

            return GetImageBrush(imageFileName);
        }

        /*private static string GetResourcePath()
        {
            //return "/VikingSagaWpfApp;component/Resources/";
            return ConfigurationManager.AppSettings["ResourceBasePath"];
        }*/

        internal static Brush GetImageBrush(string relativeResourcePath)
        {
            // Remove starting '\', this makes Path.Combine fail (stackoverflow.com/questions/53102/why-does-path-combine-not-properly-concatenate-filenames-that-start-with-path-di)
            if (relativeResourcePath.StartsWith(@"\"))
                relativeResourcePath = relativeResourcePath.Substring(1);

            var brush = new ImageBrush();
            /*var resourcePath = GetResourcePath();
            if (resourcePath == null)
                MessageBox.Show("GetResourcePath() is null - check app settings");*/

            var uri = getPackedUri(relativeResourcePath);
            brush.ImageSource = new BitmapImage(uri);
            return brush;
        }

        internal static void ForceUIUpdate(UserControl control)
        {
            control.Dispatcher.Invoke(new Action(() => { }), DispatcherPriority.ContextIdle, null);
        }
    }
}
