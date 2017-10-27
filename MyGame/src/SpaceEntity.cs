using SwinGameSDK;

namespace MyGame
{
    public abstract class SpaceEntity
    {
        public bool alive = true;
        private double angle, power;
        public bool paused;

        public Vector2D pos;
        public Vector2D vel;

        public SpaceEntity(Vector2D aPos, double aAngle, double aPower, double aMass)
        {
            // Keep initial values
            pos = aPos;
            angle = aAngle; // radians!
            power = aPower;
            Mass = aMass;
            vel = new Vector2D();

            // Set inital alive/pause state flags
            alive = true;
            paused = false;
        }

        public abstract void Update(double DT);

        public void Render()
        {
            //SwinGame.HSBColor((float)rand.Next(0, 360), (float)0.5, (float)0.8)

            //draw itself
            SwinGame.FillCircle(Color.Black, pos.asPoint2D(), (int) Mass);
        }


        public double Mass { get; }

        public Vector2D Accel { get; set; }
    }
}