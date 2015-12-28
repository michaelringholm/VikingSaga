using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VikingSaga.Code
{
    [XmlInclude(typeof(Warrior))]
    [XmlInclude(typeof(AIEncounter))]
    public class VikingSagaUserProfile
    {
        public VikingSagaUserProfile()
        {
            Heroes = new Hero[4];            
        }

        public Hero SelectedHero { get; set; }
        public Hero[] Heroes { get; set; }
        public Deck Deck { get; set; }                
        public int Gold { get; set; }

        internal void Save()
        {
            DAC.StoreProfile(this);
        }

        public string Password { get; set; }

        public string Name { get; set; }
    }
}
