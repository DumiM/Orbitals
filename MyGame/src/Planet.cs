namespace MyGame
{
    public class Planet : SpaceEntity
    {
        public Planet(Vector2D aPos, double aAngle, double aPower, double aMass) : base(aPos, aAngle, aPower, aMass)
        {
        }

        public override void Update(double DT)
        {
            //Doesn't update if paused
            if (paused)
                return;
            //accel = vel/time, therfore:
            vel += DT * Accel;
            pos += DT * vel;
        }
    }
}