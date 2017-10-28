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

        public override void Render()
        {
            //draw itself
            SwinGame.FillCircle(Color.Black, pos.asPoint2D(), (int) Mass);
        }
    }
}
