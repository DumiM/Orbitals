using System.Collections.Generic;
using static System.Math;

namespace MyGame
{
    public class Calculate
    {
        public const double G = 1000; //the value of gravity in the game

        public static void Acceleration(List<SpaceEntity> SpaceEntities)
        {
            //for each space entity
            for (var i = 0; i < SpaceEntities.Count; i++)
            {
                //second loop to calculate relative accel against each other space entity
                SpaceEntities[i].Accel = new Vector2D();
                for (var j = 0; j < i; j++)
                {
                    // Find relative position, which is needed for distance and direction
                    var step = new Vector2D();
                    step = SpaceEntities[i].pos - SpaceEntities[j].pos;

                    // distance is |x^2+y^2+..|
                    var distance = step.Length();

                    // Law of gravity
                    // gravitatonal force = (G1*G2)/(radius^2)
                    var force = G * SpaceEntities[i].Mass * SpaceEntities[j].Mass / (distance * distance);

                    // direction vector from x position to y
                    var direction = step.normal();

                    // Add equal and opposite accelerations
                    // since the space entities attract towards each other
                    SpaceEntities[i].Accel -= direction * (force / SpaceEntities[i].Mass);
                    SpaceEntities[j].Accel += direction * (force / SpaceEntities[j].Mass);
                }
            }
        }

        public static double DegreesToRadians(double degrees)
        {
            var radians = PI / 180 * degrees;
            return radians;
        }
    }
}