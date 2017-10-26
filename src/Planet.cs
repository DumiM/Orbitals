using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;
using SwinGameSDK;

namespace MyGame
{
    public class Planet: SpaceEntity
    {
        public Planet(Vector2D aPos, double aAngle, double aPower, double aMass) : base (aPos, aAngle, aPower, aMass)
        {
            
        }
    }
}
