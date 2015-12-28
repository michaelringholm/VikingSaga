using System;
using System.Collections.Generic;
using System.Linq;
using GameLib.DataStore.DTOs;
using GameLib.Quests;
using Vik.Code.Game.Main.Profiles;

namespace GameLib.DataStore.DTOs
{
    public class ProfileDTO : DTO
    {
        public string Name { get; set; }

        public int Gold { get; set; }

        public WorldLocation PlayerLocation { get; set; }

        public string[] PartyCards = new string[5];
        public List<string> DeckCards = new List<string>();
        public List<string> RemainingCards = new List<string>();

        public List<string> BackpackCards = new List<string>();

        public List<Quest> ActiveQuests = new List<Quest>();
        public List<string> CompletedQuestTypes = new List<string>();

        public List<int> GlobalFlagsAsIntegers = new List<int>();
    }
}
