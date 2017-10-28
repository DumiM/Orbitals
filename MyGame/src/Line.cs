using SwinGameSDK;
using System.Collections.Generic;
using System;


namespace MyGame
{
    public class Line : SpaceEntity
    {
        public List<Vector2D> lines;
        public Line(Vector2D aPos, double aMass) : base(aPos, aMass)
        {
        }

        public override void Render(Color clr)
        {
            //draw itself
            SwinGame.FillCircle(clr, pos.asPoint2D(), 1);
        }
    }
}