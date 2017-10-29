using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class Planet : SpaceEntity
    {
        public List<Vector2D> lines;

        public Planet(Vector2D aPos, double aMass, Color colour) : base(aPos, aMass)
        {
            lines = new List<Vector2D>();
            clr = colour;
        }

        public override void Render()
        {
            //draw itself
            SwinGame.FillCircle(clr, pos.asPoint2D(), (int) Mass);

            //add it's current position to the line array
            lines.Add(pos);
            //Render the line
            RenderLines();
        }

        public void RenderLines()
        {
            //length of the line
            //increase/decrease the value to control length
            var count = lines.Count - 1000;
            if (count > 0)
                lines.RemoveRange(0, count);

            //draws the line using positions sotred in the array
            foreach (var l in lines)
                SwinGame.FillCircle(clr, l.asPoint2D(), 1);
        }
    }
}