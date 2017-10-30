using NUnit.Framework;
using MyGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Tests
{
    [TestFixture()]
    public class GameTests
    {
        [Test()]
        public void InitialiseVariablesTest()
        {
            Game g = new Game();

            //test if all game variables are intilaised as intended
            Assert.AreEqual((!g.Status["paused"]&& !g.Status["overlap"] && g.Status["running"] && !g.Status["blackhole"]), true);
        }
    }
}