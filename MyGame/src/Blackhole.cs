using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwinGameSDK;

namespace MyGame
{
    public class Blackhole : SpaceEntity
    {
        public Blackhole(Vector2D aPos, double aMass) : base(aPos, aMass)
        {
        }

        public override void Update(double DT)
        {
        }

        public override void Render(Color clr)
        {
            //draw itself
            clr = Color.Black;
            SwinGame.FillCircle(clr, pos.asPoint2D(), (int) Mass);
        }
    }
}
