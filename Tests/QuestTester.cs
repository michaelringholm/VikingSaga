using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VikingSaga.Code.Campaign.NPC;
using VikingSaga.Code;

namespace Tests
{
    [TestClass]
    public class QuestTester
    {
        [TestMethod]
        public void TheWildBoar()
        {
            var midheimVillage = new Midheim();
            var midheimBarmaid = new MidheimBarmaid();
            var hero = new Warrior { Name = "Test Hero" };

            midheimVillage.EnterVillage(hero);

            Console.WriteLine("Done");
        }
    }
}
