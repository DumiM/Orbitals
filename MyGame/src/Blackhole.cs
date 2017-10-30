using SwinGameSDK;

namespace MyGame
{
    public class Blackhole : SpaceEntity
    {
        public Blackhole(Vector2D aPos, double aMass) : base(aPos, aMass)
        {
            clr = SwinGame.HSBColor((float) 0.63, (float) 0.21, (float) 0.30);
        }

        public override void Update(double dt)
        {
        }

        public override void Render()
        {
            //draw itself
            SwinGame.FillCircle(clr, pos.asPoint2D(), (int) Mass);
        }
    }
}