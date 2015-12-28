using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows;
using GameLib.Utility;
using GameLib.World.Maps;
using Vik.Code.Utility;
using System.Windows.Threading;
using GameLib.Encounters;
using GameLib.World.Maps.Geo;
using Vik.Code.Controls.Utility;
using Vik.Code.Controls.Battle;

namespace Vik.Code.Controls.Maps
{
    public partial class MapControl : UserControl
    {
        private List<MapLocationLinkData> _links = new List<MapLocationLinkData>();
        private List<MapLocationData> _nodes = new List<MapLocationData>();
        private List<MapNodeControl> _nodeControls = new List<MapNodeControl>();

        private MapLocationData _playerLocation;
        private PathHelper _playerTravelPathHelper;
        private TranslateTransform _playerIconTransform = new TranslateTransform();
        private DoubleAnimationUsingPath _playerPathAnimationX;
        private DoubleAnimationUsingPath _playerPathAnimationY;

        private bool _travelInProgress;
        private Encounter _travelEncounter;
        private double _travelEncounterT;
        private MapLocationData _travelDestinationData;

        public Map CurrentMap { get { return _map; } }
        private Map _map;

        private double _travelSpeedUnitsPerSec = 0.3; // Units are [0..1]

        public bool InMapEditor { get; set; }

        public double MapW { get { return imgMap.ActualWidth; } }
        public double MapH { get { return imgMap.ActualHeight; } }

        public Point ControlToCanvas(Point p)
        {
            double x = p.X - ((this.ActualWidth - imgMap.ActualWidth) / 2);
            double y = p.Y - ((this.ActualHeight - imgMap.ActualHeight) / 2);
            return new Point(x, y);
        }

        public MapControl()
        {
            InitializeComponent();

            Player.RenderTransform = _playerIconTransform;
            SizeChanged += MapControl_SizeChanged;
        }

        void MapControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateControls();
            PlacePlayerIconAt(_playerLocation);
        }

        private void SetPlayerLocationInternal(MapLocationData mapLocation)
        {
            _playerLocation = mapLocation;
            PlacePlayerIconAt(mapLocation);
            Player.Visibility = System.Windows.Visibility.Visible;
        }

        public void SetPlayerLocation(MapLocationData mapLocation)
        {
            Dispatcher.CurrentDispatcher.BeginInvoke((Action<MapLocation>)SetPlayerLocationInternal, DispatcherPriority.ContextIdle, mapLocation);
        }

        private void PlacePlayerIconAt(MapLocationData mapLocation)
        {
            if (mapLocation == null)
                return;

            var pos = Denormalize(mapLocation.Location);
            PlacePlayerIconAtDenormalized(pos.X, pos.Y);
        }

        public void SetMap(Map map, ImageSource imageSource)
        {
            Player.Visibility = System.Windows.Visibility.Collapsed;

            imgMap.Source = imageSource;
            SetNodes(map.Locations, map.AllLinks());
            _map = map;
        }

        public void SetNodes(IEnumerable<MapLocationData> nodes, IEnumerable<MapLocationLinkData> links)
        {
            _nodes.Clear();
            _nodes.AddRange(nodes);

            _links.Clear();
            _links.AddRange(links);

            CreateControlsAsync();
        }

        private void UpdateLocations()
        {
            Debug.Assert(_nodeControls.Count == _nodes.Count);

            for (int i = 0; i < _nodeControls.Count; ++i)
            {
                var node = _nodes[i];
                var nodeControl = _nodeControls[i];
                var nodeActualLocation = Denormalize(node.Location);
                double nodeX = nodeActualLocation.X - nodeControl.Width / 2;
                double nodeY = nodeActualLocation.Y - nodeControl.Height / 2;
                nodeControl.RenderTransform = new TranslateTransform(nodeX, nodeY);
            }
        }

        private Point Denormalize(Point p)
        {
            double w = imgMap.ActualWidth;
            double h = imgMap.ActualHeight;
            double l = (this.ActualWidth - w) / 2;
            double t = (this.ActualHeight - h) / 2;
            return new Point((p.X * w) + l, (p.Y * h) + t);
        }

        private PathFigure CreatePath(MapLocationLinkData link)
        {
            var pathFigure = new PathFigure();
            pathFigure.IsClosed = false;
            pathFigure.IsFilled = false;
            pathFigure.StartPoint = Denormalize(link.Points[0]);

            var segments = new PathSegmentCollection();
            for (int i = 1; i < link.Points.Count; ++i)
            {
                var p = Denormalize(link.Points[i]);
                segments.Add(new LineSegment(p, true));
            }

            pathFigure.Segments = segments;
            return pathFigure;
        }

        private void CreateControlsAsync()
        {
            Dispatcher.CurrentDispatcher.BeginInvoke((Action)CreateControls, DispatcherPriority.ContextIdle);
        }

        private void CreateControls()
        {
            if (_nodes == null)
                return;

            foreach(var nodeControl in _nodeControls)
            {
                this.mapCanvas.Children.Remove(nodeControl);
            }
            _nodeControls.Clear();

            var pathCollection = new PathFigureCollection();

            foreach (var node in _nodes)
            {
                var nodeControl = new MapNodeControl();
                nodeControl.Width = 16; // TODO: Where to place size?
                nodeControl.Height = 16;
                nodeControl.Visibility = System.Windows.Visibility.Visible;
                nodeControl.Tag = node;
                nodeControl.MouseDown += nodeControl_MouseDown;
                nodeControl.MouseMove += nodeControl_MouseMove;

                this.mapCanvas.Children.Add(nodeControl);
                _nodeControls.Add(nodeControl);

                foreach (var link in _links.Where(l => l.Node1Id == node.Id))
                {
                    var pathFigure = CreatePath(link);
                    pathCollection.Add(pathFigure);
                }
            }

            var geo = new PathGeometry();
            geo.Figures = pathCollection;
            linePath.Data = geo;

            UpdateLocations();
        }

        void nodeControl_MouseMove(object sender, MouseEventArgs e)
        {
            ShowDebugInfo((MapNodeControl)sender);
        }

        private void ShowDebugInfo(MapNodeControl control)
        {
            var data = (MapLocationData)control.Tag;
            tbDebugInfo.Text = data.GetDebugInfo();
        }

        void nodeControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InMapEditor || e.ChangedButton != MouseButton.Left)
                return;

            var control = (MapNodeControl)sender;
            var locationData = (MapLocationData)control.Tag;

            StartPlayerTravel(locationData);
        }

        private MapLocationLinkData GetLink(MapLocationData from, MapLocationData to)
        {
            return _links.Where(link => (link.Node1Id == from.Id && link.Node2Id == to.Id) || (link.Node1Id == to.Id && link.Node2Id == from.Id)).FirstOrDefault();
        }

        private void StartPlayerTravel(MapLocationData to, double tBegin = 0.0)
        {
            if (_travelInProgress)
                return;

            var travelLink = GetLink(_playerLocation, to);
            if (travelLink == null)
                return; // For now just ignore click on non-connected nodes
            //    throw new InvalidOperationException("Current location not connected to destination location");

            double tEnd = 1.0;

            bool isResumedTravel = tBegin != 0.0; // Only 1 encounter per travel is currently possible
            if (!isResumedTravel)
            {
                var encounter = _map.GetEncounter(_playerLocation.Id, to.Id, out _travelEncounterT);
                if (encounter != null)
                {
                    _travelEncounter = encounter;
                    _travelDestinationData = to;
                    tEnd = _travelEncounterT;
                }
            }
            else
            {
                _travelEncounter = null;
                _travelEncounterT = -1;
                _travelDestinationData = null;
            }

            bool reverse = travelLink.Node1Id != _playerLocation.Id;

            _playerTravelPathHelper = new PathHelper(_playerLocation.Id, to.Id, travelLink, reverse);

            var points = _playerTravelPathHelper.CalcFixPoints(tBegin, tEnd).ToList();
            int travelMs = (int)((PathHelper.CalcTotalDistance(points) / _travelSpeedUnitsPerSec) * 1000);

            var denormalizedPoints = points.Select(p => Denormalize(p)).ToList();
            var path = AnimHelper.PathFromPoints(denormalizedPoints);

            _playerPathAnimationX = AnimHelper.GetPathAnim(path, travelMs, PathAnimationSource.X);
            _playerPathAnimationY = AnimHelper.GetPathAnim(path, travelMs, PathAnimationSource.Y);
            _playerPathAnimationX.Completed += PlayerTravelArrive;

            _playerIconTransform.BeginAnimation(TranslateTransform.XProperty, _playerPathAnimationX);
            _playerIconTransform.BeginAnimation(TranslateTransform.YProperty, _playerPathAnimationY);

            _travelInProgress = true;
        }

        private void PlacePlayerIconAtDenormalized(double x, double y)
        {
            _playerIconTransform.X = x;
            _playerIconTransform.Y = y;
        }

        private void StartEncounter(Encounter encounter)
        {
            VikGame.EncounterController.RunEncounter(encounter);
            StartPlayerTravel(_travelDestinationData, _travelEncounterT);
        }

        private void PlayerTravelArrive(object sender, EventArgs e)
        {
            _travelInProgress = false;

            if (_travelEncounter != null)
            {
                UiUtil.Post(() => { StartEncounter(_travelEncounter); });
            }
            else
            {
                var destination = _nodes.Where(n => n.Id == _playerTravelPathHelper.LocationToId).FirstOrDefault();
                VikGame.World.ChangePlayerLocation(_map.Id, destination.Id, GameLib.World.PlayerChangeLocationMethod.Travel);

                _playerIconTransform.BeginAnimation(TranslateTransform.XProperty, null);
                _playerIconTransform.BeginAnimation(TranslateTransform.YProperty, null);
            }
        }
    }
}
