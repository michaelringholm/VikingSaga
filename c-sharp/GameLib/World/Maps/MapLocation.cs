using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Windows;

namespace GameLib.World.Maps
{
    public class ChangePlayerLocationAction : MapLocationAction
    {
        public string MapId { get; set; }
        public string MapLocationId { get; set; }
        public PlayerChangeLocationMethod Method { get; set; }
    }

    public class EnterSpecialLocationAction : MapLocationAction
    {
        public bool Enter { get; set; }
        public WorldData.SpecialLocationEnum SpecialLocationId { get; set; }
    }

    public abstract class MapLocationAction
    {
        public string DisplayName { get; set; }
    }

    public class MapLocationData
    {
        public string Id { get; set; }
        public Point Location { get; set; }

        public virtual string GetDebugInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Id: {0}, Pos: {1:0.000}; {2:0.000}", Id, this.Location.X, this.Location.Y));
            return sb.ToString();
        }
    }

    public class MapLocation : MapLocationData
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public readonly List<MapLocationLinkData> Links = new List<MapLocationLinkData>();

        public Func<bool> BeforeEnter = () => { return true; };
        public Action AfterEnter = () => { };
        public Func<bool> BeforeLeave = () => { return true; };
        public Action AfterLeave = () => { };

        public List<MapLocationAction> Actions = new List<MapLocationAction>();

        public MapLocation(MapLocationData data)
        {
            this.Id = data.Id;
            this.Location = data.Location;
        }

        public override string GetDebugInfo()
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Pos: {0:0.000}; {1:0.000}", this.Location.X, this.Location.Y));
            sb.AppendLine("ID: " + this.Id + ", Title: " + this.Title);
            sb.Append("Actions: " + string.Join(",", this.Actions.Select(a => a.DisplayName)));
            return sb.ToString();
        }
    }
}
