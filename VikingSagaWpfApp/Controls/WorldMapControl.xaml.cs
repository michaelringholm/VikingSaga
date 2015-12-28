using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using VikingSaga.Code;
using VikingSaga.Code.Resources;

namespace VikingSagaWpfApp.Controls
{
    /// <summary>
    /// Interaction logic for WorldMapControl.xaml
    /// </summary>
    public partial class WorldMapControl : UserControl, IMapUI
    {
        public WorldMapControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                mapCanvas.Background = ResourceManager.GetImageBrush(ResourceManager.ImageEnum.WorldMap);
            }
            else
                GridBackgroundBrush.ImageSource = null;
        }

        private List<MapLocationImage> _mapLocationImages;

        public void DrawMap(Map map, Hero hero)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                HeroControl.UpdateHeroCard(hero);
                mapImage.Source = ResourceManager.GetImage(map.MapImagePath).Source;
                Opacity = 0.95;
                PlotMapLocations(map);
            }));
        }

        private void PlotMapLocations(Map map)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                _mapLocationImages = new List<MapLocationImage>();
                foreach (MapLocation mapLocation in map.Locations)
                    PlotMapLocation(map, mapLocation);
                //PlotMapLocation(50, 40, "\\markers\\green-highlighted-marker-24x24.png", new AIEncounter { Hero = new Warrior { Name = "a rabbit", CardImageURL = @"mobs\rabbit-150x150.png" } });
                //PlotMapLocation(100, 100, "\\markers\\green-marker-24x24.png", new AIEncounter { Hero = new Warrior { Name = "a rabbit", CardImageURL = @"mobs\rabbit-150x150.png" } });
            }));
        }

        private void PlotMapLocation(Map map, MapLocation mapLocation)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var markerImage = GetMarkerImage(mapLocation);

                var mapLocationImg = new MapLocationImage { Source = markerImage.Source };
                mapLocationImg.MapLocation = mapLocation;
                Canvas.SetLeft(mapLocationImg, mapLocation.Coordinates.X * map.Width / (100));
                Canvas.SetTop(mapLocationImg, mapLocation.Coordinates.Y * Height / (100));
                mapCanvas.Children.Add(mapLocationImg);
                mapLocationImg.MouseDown += mapLocation_MouseDown;
                _mapLocationImages.Add(mapLocationImg);
            }));
        }

        private Image GetMarkerImage(MapLocation mapLocation)
        {
            Image markerImage = null;
            if (mapLocation.Marker == MapLocation.MarkerTypeEnum.Path)
                markerImage = ResourceManager.GetImage("markers/green-marker-24x24.png");
            else if (mapLocation.Marker == MapLocation.MarkerTypeEnum.Hero)
                markerImage = ResourceManager.GetImage("markers/green-highlighted-marker-24x24.png");
            else if (mapLocation.Marker == MapLocation.MarkerTypeEnum.Boss)
                markerImage = ResourceManager.GetImage("markers/boss4-marker-64x47.png");
            else if (mapLocation.Marker == MapLocation.MarkerTypeEnum.Village)
                markerImage = ResourceManager.GetImage("markers/village.png");
            else
                throw new Exception("Unsupported map location marker [" + mapLocation.Marker.ToString() + "]");

            return markerImage;
        }

        void mapLocation_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mapLocationImg = (MapLocationImage)sender;
            GameController.Current.MapPositionChanged(mapLocationImg.MapLocation);

            //if(mapLocationImg.MapLocation)
            //GameController.Current.StartBattle(mapLocationImg.MapLocation.Encounter);

            //new BattleBoardWindow(_profile, mapLocation.Encounter).Show();
            //MessageBox.Show("You just stumbled on " + mapLocation.Encounter.Hero.Name);
        }

        public void ShowMap()
        {
            this.Visibility = System.Windows.Visibility.Visible;
        }

        public void MarkHeroLocation(MapLocation newMapLocation)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                //MessageBox.Show("Hero moved to X=[" + newMapLocation.X + "] Y=[" + newMapLocation.Y + "]");
                var mapLocationImage = _mapLocationImages.Where(ml => (ml.MapLocation.Coordinates.X == newMapLocation.Coordinates.X) && (ml.MapLocation.Coordinates.Y == newMapLocation.Coordinates.Y)).SingleOrDefault();
                mapLocationImage.Source = ResourceManager.GetImage("markers/green-highlighted-marker-24x24.png").Source;
                UpdateTexts();
            }));
        }

        private void UpdateTexts()
        {
            String test = "You have never been so far away from Midheim. and chills runs down you back! It feels like something is watching you  from the trees along the hills.";
            LocationText.Text = "Mad Boar Hills";
            LocationText.Text = test;
        }

        public void DrawNewHeroMapPosition(MapLocation oldMapLocation, MapLocation newMapLocation)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                var mapLocationImage = _mapLocationImages.Where(ml => (ml.MapLocation.Coordinates.X == oldMapLocation.Coordinates.X) && (ml.MapLocation.Coordinates.Y == oldMapLocation.Coordinates.Y)).SingleOrDefault();
                mapLocationImage.Source = GetMarkerImage(oldMapLocation).Source;
                MarkHeroLocation(newMapLocation);
                SoundUtil.PlaySound(SoundUtil.SoundEnum.WalkForest);
            }));
        }

        public void ShowInvalidHeroMoveEffect(MapLocation newMapLocation)
        {
            MessageBox.Show("Hero can't move to X=[" + newMapLocation.Coordinates.X + "] Y=[" + newMapLocation.Coordinates.Y + "]");
        }

        public ImageSource GetMainWindowBackgroundImage()
        {
            return ResourceManager.GetImage(@"maps\WorldMap.png").Source;
            //return null;
        }
    }
}
