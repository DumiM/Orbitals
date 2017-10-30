using SwinGameSDK;

namespace MyGame
{
    public class Line : SpaceEntity
    {
        public Line(Vector2D aPos, double aMass, Color colour) : base(aPos, aMass)
        {
            clr = colour;
        }

        public override void Render()
        {
            //draw itself
            SwinGame.FillCircle(clr, pos.asPoint2D(), 1);
        }
    }
}