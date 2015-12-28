using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using VikingSaga.Code.Campaign.PEE.Maps;
using Global;
using VikingSagaWpfApp.Animations;

namespace VikingSaga.Controls
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

        private double _travelSpeedUnitsPerSec = 0.1; // Units are [0..1]

        public MapControl()
        {
            InitializeComponent();

            NameScope.SetNameScope(this, new NameScope()); // required for story boards
            
            string input = @"<C orange><B><F+>Header<F-></B><BR><C white>This is some <C orange><B>cool</B> <C white><I>text</I>!! This is some This is some This is some";
            var inlines = FormattedTextParser.Parse(input, this.FontFamily, 14).ToList();
            MouseOverHintLabel.Inlines.Clear();
            MouseOverHintLabel.Inlines.AddRange(inlines);

            Player.RenderTransform = _playerIconTransform;

            SizeChanged += MapControl_SizeChanged;
        }

        void MapControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateControls();
            PlacePlayerIconAt(_playerLocation);
        }

        private void SetPlayerLocation(MapLocationData mapLocation)
        {
            _playerLocation = mapLocation;
            PlacePlayerIconAt(_playerLocation.Location.X, _playerLocation.Location.Y);
        }

        private void PlacePlayerIconAt(double x, double y)
        {
            _playerIconTransform.X = x;
            _playerIconTransform.Y = y;
        }

        private void PlacePlayerIconAt(MapLocationData mapLocation)
        {
            var pos = Denormalize(mapLocation.Location);
            PlacePlayerIconAt(pos.X, pos.Y);
        }

        private void PlacePlayerIconAt(PathHelper pathHelper, double t)
        {
            var pos = pathHelper.GetPoint(t);
            pos = Denormalize(pos);
            PlacePlayerIconAt(pos.X, pos.Y);
        }

        public void SetNodes(IEnumerable<MapLocationData> nodes, IEnumerable<MapLocationLinkData> links)
        {
            _nodes.Clear();
            _nodes.AddRange(nodes);

            _links.Clear();
            _links.AddRange(links);

            SetPlayerLocation(_nodes[0]); // test
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
                nodeControl.Margin = new Thickness(nodeX, nodeY, 0, 0);
            }
        }

        Point Denormalize(Point p)
        {
            double w = Image.ActualWidth;
            double h = Image.ActualHeight;
            double l = (this.ActualWidth - w) / 2;
            double t = (this.ActualHeight - h) / 2;
            return new Point((p.X * w) + l, (p.Y * h) + t);
        }

        PathFigure CreatePath(MapLocationLinkData link)
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

        private void CreateControls()
        {
            foreach(var nodeControl in _nodeControls)
            {
                this.mainGrid.Children.Remove(nodeControl);
            }
            _nodeControls.Clear();

            var pathCollection = new PathFigureCollection();

            foreach (var node in _nodes)
            {
                var nodeControl = new MapNodeControl();
                nodeControl.Width = 16;
                nodeControl.Height = 16;
                nodeControl.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                nodeControl.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                nodeControl.Visibility = System.Windows.Visibility.Visible;
                nodeControl.Tag = node;
                nodeControl.MouseDown += nodeControl_MouseDown;

                this.mainGrid.Children.Add(nodeControl);
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

        void nodeControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            var control = (MapNodeControl)sender;
            var locationData = (MapLocationData)control.Tag;

            StartPlayerTravel(locationData);
        }

        private MapLocationLinkData GetLink(MapLocationData from, MapLocationData to)
        {
            return _links.Where(link => (link.Node1Id == from.Id && link.Node2Id == to.Id) || (link.Node1Id == to.Id && link.Node2Id == from.Id)).FirstOrDefault();
        }

        private void StartPlayerTravel(MapLocationData to)
        {
            var travelLink = GetLink(_playerLocation, to);
            if (travelLink == null)
                throw new InvalidOperationException("Current location not connected to destination location");

            bool reverse = travelLink.Node1Id != _playerLocation.Id;
            _playerTravelPathHelper = new PathHelper(_playerLocation.Id, to.Id, travelLink, reverse);

            int travelMs = (int)((_playerTravelPathHelper.TotalDistance / _travelSpeedUnitsPerSec) * 1000);

            var points = _playerTravelPathHelper.FixPoints.Select(p => Denormalize(p)).ToList();
            var path = AnimHelper.PathFromPoints(points);

            _playerPathAnimationX = AnimHelper.GetPathAnim(path, travelMs, PathAnimationSource.X);
            _playerPathAnimationY = AnimHelper.GetPathAnim(path, travelMs, PathAnimationSource.Y);
            _playerPathAnimationX.Completed += PlayerTravelArrive;

            _playerIconTransform.BeginAnimation(TranslateTransform.XProperty, _playerPathAnimationX);
            _playerIconTransform.BeginAnimation(TranslateTransform.YProperty, _playerPathAnimationY);
        }

        void PlayerTravelArrive(object sender, EventArgs e)
        {
            var destination = _nodes.Where(n => n.Id == _playerTravelPathHelper.LocationToId).FirstOrDefault();
            SetPlayerLocation(destination);
        }
    }
}
