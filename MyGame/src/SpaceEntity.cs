using SwinGameSDK;

namespace MyGame
{
    public abstract class SpaceEntity
    {
        public bool alive = true;

        public Color clr;
        public bool paused;

        public Vector2D pos;
        public Vector2D vel;

        protected SpaceEntity(Vector2D aPos, double aMass)
        {
            // Keep initial values
            pos = aPos;
            Mass = aMass;
            vel = new Vector2D();

            // Set inital alive/pause state flags
            alive = true;
            paused = false;
        }


        public double Mass { get; }

        public Vector2D Accel { get; set; }

        public virtual void Update(double DT)
        {
            //Doesn't update if paused
            if (paused)
                return;
            //accel = vel/time, therfore:
            vel += DT * Accel;
            pos += DT * vel;
        }

        public abstract void Render();
    }
}