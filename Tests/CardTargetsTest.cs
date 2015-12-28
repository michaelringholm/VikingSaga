using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VikingSagaWpfApp.Code.BattleNs.Players.AI;
using VikingSagaWpfApp.Code.BattleNs.Cards;

namespace Tests
{
    [TestClass]
    public class CardTargetsTest
    {
        int TestCount(int count)
        {
            var gen = new CardTargetGenerator();
            gen.Reset(count);

            int counter = 1;
            while (gen.Advance())
                counter++;

            return counter;
        }

        [TestMethod]
        public void TestMethod1()
        {
            int TargetCount = BattleCard.AllTargets.Count;

            int oneCard = TestCount(1);
            Assert.IsTrue(oneCard == TargetCount);

            int twoCards = TestCount(2);
            Assert.IsTrue(twoCards == TargetCount * TargetCount);

            int threeCards = TestCount(3);
            Assert.IsTrue(threeCards == TargetCount * TargetCount * TargetCount);
        }
    }
}
