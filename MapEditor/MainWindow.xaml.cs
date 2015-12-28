using GameLib.Utility;
using GameLib.World.Maps;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace MapEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LocationSerialization _data;

        private int _idSuggestion = 1;

        public IEnumerable<string> ExistingIds
        {
            get
            {
                if (_data == null || _data.LocationData == null)
                {
                    yield return "<None>";
                }
                else
                {
                    foreach (var id in _data.LocationData.Select(d => d.Id))
                        yield return id;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            DataContext = this;
            mapControl.InMapEditor = true;

            try
            {
                //_data = LocationSerialization.Deserialize(SampleXml.Data);
                _data = new LocationSerialization();
            }
            catch(Exception e)
            {
                MessageBox.Show("Could not deserialize sample data: " + e.InnerException.Message);
                _data = new LocationSerialization();
            }

            OnModelChanged();
        }

        void imageContextMenu_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                AddNewLocation(_addNewPoint);
            }
        }

        private Point _addNewPoint;
        private void mapControl_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            _addNewPoint = mapControl.ControlToCanvas(Mouse.GetPosition(mapControl));
        }

        private void OnModelChanged()
        {
            mapControl.SetNodes(_data.LocationData, _data.LocationLinks);
        }

        private void EditXmlClick(object sender, RoutedEventArgs e)
        {
            string serialized = LocationSerialization.Serialize(_data);

            while (true)
            {
                EditXml win = new EditXml();
                win.tbXml.Text = serialized;
                if (win.ShowDialog() ?? false)
                {
                    string xml = win.tbXml.Text;
                    try
                    {
                        _data = LocationSerialization.Deserialize(xml);
                        OnModelChanged();
                        break;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Parse error");
                    }
                }
                else
                {
                    break;
                }
            }
        }

        public static BitmapImage GetBitmapImage(Uri imageAbsolutePath)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.UriSource = imageAbsolutePath;
            image.EndInit();

            return image;
        }

        private void LoadImageClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //var exePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            var dir = System.IO.Path.GetFullPath(@".\..\..\..\VikGame\Data\Gfx\Maps\");
            dlg.InitialDirectory = dir;

            if ((bool)dlg.ShowDialog())
            {
                Uri uri = new Uri(dlg.FileName);
                mapControl.imgMap.Source = GetBitmapImage(uri);
            }
        }

        private void mapControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _pathInProgress)
            {
                var pos = mapControl.ControlToCanvas(e.GetPosition(mapControl));
                pos.X /= mapControl.MapW;
                pos.Y /= mapControl.MapH;

                _path.Add(pos);
            }
        }

        private List<Point> _path = new List<Point>();
        private bool _pathInProgress;

        private void BeginPathClick(object sender, RoutedEventArgs e)
        {
            _path.Clear();
            _pathInProgress = true;
        }

        private void EndPathClick(object sender, RoutedEventArgs e)
        {
            _pathInProgress = false;

            if (_path.Count < 2)
                return;

            var count = _path.Count;

            var closestStart = FindClosestNode(_path[0]);
            var closestEnd = FindClosestNode(_path[count - 1]);
            _path[0] = closestStart.Location;
            _path[count - 1] = closestEnd.Location;

            SetMapLink(closestStart, closestEnd, _path);

            OnModelChanged();
        }

        private void SetMapLink(MapLocationData a, MapLocationData b, IEnumerable<Point> path)
        {
            var existingLink = _data.LocationLinks.FirstOrDefault(l => (l.Node1Id == a.Id && l.Node2Id == b.Id) || (l.Node1Id == b.Id && l.Node2Id == a.Id));
            _data.LocationLinks.Remove(existingLink);

            var link = new MapLocationLinkData();
            link.Node1Id = a.Id;
            link.Node2Id = b.Id;
            link.Points.AddRange(path);

            _data.LocationLinks.Add(link);
        }

        private double PointDistanceSquared(Point a, Point b)
        {
            var dx = a.X - b.X; 
            var dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }
        
        private MapLocationData FindClosestNode(Point point)
        {
            var minDist = double.MaxValue;
            MapLocationData result = null;
            foreach (var node in _data.LocationData)
            {
                var dist = PointDistanceSquared(node.Location, point);
                if (dist < minDist)
                {
                    minDist = dist;
                    result = node;
                }
            }
            return result;
        }

        private string _nextId = "<missing>";
        private Point _nextPoint;

        private void AddNewLocation(Point point)
        {
            _nextPoint = point;
            tbUnique.Text = _idSuggestion.ToString();
            _idSuggestion++;
            lbExisting.ItemsSource = ExistingIds;
            borderEnterId.Visibility = System.Windows.Visibility.Visible;
        }

        private void EnterUniqueClick(object sender, RoutedEventArgs e)
        {
            _nextId = tbUnique.Text;
            if (!_data.LocationData.Any(n => n.Id == _nextId))
            {
                borderEnterId.Visibility = System.Windows.Visibility.Collapsed;
                AddNewLocation(_nextPoint, _nextId);
            }
        }

        private double RoundToDecimals(double val, int decimals)
        {
            return (double)decimal.Round((decimal)val, decimals, MidpointRounding.AwayFromZero);
        }

        private void AddNewLocation(Point point, string id)
        {
            MapLocationData newNode = new MapLocationData();
            newNode.Id = id;
            var nodeX = point.X / mapControl.MapW;
            var nodeY = point.Y / mapControl.MapH;
            newNode.Location = new Point(RoundToDecimals(nodeX, 3), RoundToDecimals(nodeY, 3));
            _data.LocationData.Add(newNode);

            OnModelChanged();
        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            _data = new LocationSerialization();
            OnModelChanged();
        }

        Point GetRelativePos(Point mapPos)
        {
            var pos = mapControl.ControlToCanvas(mapPos);
            pos.X /= mapControl.MapW;
            pos.Y /= mapControl.MapH;
            return pos;
        }

        private string PointStr(Point p)
        {
            return string.Format("{0:0.000}; {1:0.000}", p.X, p.Y);
        }

        private void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            tbInfo.Text = PointStr(GetRelativePos(e.GetPosition(mapControl)));
        }
    }
}
