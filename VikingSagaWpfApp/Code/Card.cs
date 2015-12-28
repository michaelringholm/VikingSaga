using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using VikingSagaWpfApp.Code.Battle.Cards;

namespace VikingSaga.Code
{
    // What is really required to be serialized? Level, statistics..
    public class Card
    {
        public Guid ID { get; set; }
        
        [Serializable]
        public enum CardConditionEnum { Perfect, Defeated };
        
        public CardConditionEnum Condition { get; set; } // TODO set condition after battle

        private BattleCard _battleCard = null;
        [XmlIgnore]
        public BattleCard BattleCard
        {
            get
            {
                if(_battleCard == null)
                    _battleCard = VikingSagaWpfApp.Code.Battle.Cards.CardFactory.Create(BattleCardType);

                return _battleCard;
            }
        }

        [XmlIgnore]
        public Type BattleCardType 
        { 
            get
            {
                return Type.GetType(BattleCardTypeString);
            }

            set
            {
                BattleCardTypeString = value.ToString();
            }
        }
        public String BattleCardTypeString { get; set; }

        public int BattlesWon { get; set; }
        public int BattlesLost { get; set; }
        public int WinningStreak { get; set; }
        public int TotalBattles { get; set; }

        public int Kills { get; set; }
    }
}
