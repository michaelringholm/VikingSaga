using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingSagaWpfApp.Code;

namespace VikingSaga.Code
{
    public abstract class Hero
    {
        public enum ConditionEnum { Perfect, Suffer, Immune, Defeated };
        public string Name { get; set; }
        public ConditionEnum Condition { get; set; } // TODO check condition based on HP after battle
        public int HP { get; set; }
        public int RemainingHP { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
        public int Mana { get; set; }
        public int RemainingMana { get; set; }
        public String CardImageURL { get; set; }
        public bool HasGainedNewLevel { get; set; }
        public Campaign.Campaign Campaign { get; set; }

        internal void PrepareForBattle()
        {
            RemainingHP = HP;
            RemainingMana = Mana;
            Condition = ConditionEnum.Perfect;
        }

        public int Gold { get; set; }

        internal void Victory(Treasure treasure, VikingSagaUserProfile profile)
        {
            Gold += treasure.Gold;
            XP += treasure.XP;

            if (XP > GetLevel(Level).EndXP)
            {
                XP = XP - GetLevel(Level).EndXP;
                Level++;
                GameEngine.Current.OnLevelGained();
            }
        }

        internal void Defeat(Treasure treasure, VikingSagaUserProfile profile)
        {
            Gold -= treasure.Gold * 5;
            XP -= treasure.XP * 5;

            if (Gold < 0)
                Gold = 0;

            if (XP < 0)
                XP = 0;
        }

        public Map Map { get; set; }

        public abstract List<Level> GetLevels();

        public Level GetLevel(int levelNo)
        {
            var level = GetLevels().Where(l => l.LevelNo == levelNo).SingleOrDefault();

            if (level == null)
                throw new Exception("Invalid levelNo = " + levelNo);
            else
                return level;
        }

        public Level GetLevel()
        {
            return GetLevel(this.Level);
        }
    }
}
