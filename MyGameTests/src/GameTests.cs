using System;
using NUnit.Framework;

namespace MyGame.Tests
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void DegreesToRadiansTest()
        {
            var actual = Game.DegreesToRadians(90);
            Assert.AreEqual(1.5708, Math.Round(actual, 4));
        }
    }
}