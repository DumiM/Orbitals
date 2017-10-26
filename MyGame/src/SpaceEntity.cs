using SwinGameSDK;

namespace MyGame
{
    public abstract class SpaceEntity
    {
        public bool alive = true;
        private double angle, power;
        public bool paused;

        public Vector2D pos;
        private Vector2D vel;

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

        public void Update(double DT)
        {
            //Doesn't update if paused
            if (paused)
                return;
            //accel = vel/time, therfore:
            vel += DT * Accel;
            pos += DT * vel;
        }

        public void Render()
        {
            //draw itself
            SwinGame.FillCircle(Color.Black, pos.asPoint2D(), (int) Mass);
        }


        public double Mass { get; }

        public Vector2D Accel { get; set; }
    }
}