using NUnit.Framework;

namespace MyGame.Tests
{
    [TestFixture]
    public class PlanetTests
    {
        [Test]
        public void PlanetUpdateTest()
        {
            var v = new Vector2D();
            var p = new Planet(v, 0, 0, 0);

            //an acceleration vector
            var acc = new Vector2D();
            acc.x += 10; //x direction = 10
            p.Accel = acc;

            p.Update(10); //dt = 10
            var actual = p.pos;
            var expected = acc * 10 * 10;
            Assert.AreEqual(actual.x, expected.x, "Shuld both be 1000"); //compare the x values
            //this confirms that btoh velocity and position is calculated accurately
        }
    }
}