using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingSaga.Code
{
    public class Deck
    {
        public Deck()
        {
            Cards = new List<Card>();
            MaxDeckSize = 3;
            AllCards = new List<Card>();
        }
        
        public List<Card> Cards { get; set; }
        public int MaxDeckSize { get; set; }
        public List<Card> AllCards { get; set; }
    }
}
