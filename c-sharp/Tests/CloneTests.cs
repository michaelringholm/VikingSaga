using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VikingSagaWpfApp.Code.BattleNs;
using VikingSagaWpfApp.Code.BattleNs.Interfaces;
using VikingSagaWpfApp.Code.BattleNs.Cards;
using VikingSagaWpfApp.Code.BattleNs.Players.AI;

namespace Tests
{
    [TestClass]
    public class CloneTests
    {
        class TestCard : CardBasicMob { }

        [TestMethod]
        public void TestMethod1()
        {
            var ply1 = new TestPlayer();
            var ply2 = new TestPlayer();

            var testCard1 = new TestCard();
            var testCard1Prop = SpellProperty.DoubleAttack();
            testCard1.AddSpellProperty(testCard1Prop);

            var testCard2 = new TestCard();

            var testCard3 = new TestCard();
            var testCard4 = new TestCard();

            ply1.Deck.Cards.Add(testCard1);
            ply1.Deck.Cards.Add(testCard2);

            ply2.Deck.Cards.Add(testCard3);
            ply2.Deck.Cards.Add(testCard4);

            var battle = new Battle(ply1, ply2, new EmptyBattleObserver());
            ply1.DrawFromDeck(1);
            ply2.DrawFromDeck(1);

            TestPlayer clonePly1;
            TestPlayer clonePly2;
            Battle cloneBattle;
            AiHelper.CloneBattle(battle, out clonePly1, out clonePly2, out cloneBattle);

            var testCard1Cpy = (CardBasicMob)clonePly1.Hand.AllCards().First();
            var testCard2Cpy = (CardBasicMob)clonePly1.Deck.Cards.First();

            var testCard1PropCpy = testCard1Cpy.GetSpellProperty(SpellPropertyType.DoubleAttack);

            Assert.AreNotSame(ply1, clonePly1);
            Assert.AreNotSame(testCard1, testCard1Cpy);
            Assert.AreNotSame(testCard1Prop, testCard1PropCpy);
        }
    }
}
