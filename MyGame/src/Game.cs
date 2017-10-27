using System.Collections.Generic;
using SwinGameSDK;
using static System.Math;

namespace MyGame
{
    public class Game
    {

        //to keep track of time
        private readonly Timer _gameTime;

        private uint _lastTicks;

        private readonly bool paused;

        public List<SpaceEntity> SpaceEntities;

        public Game()
        {
            Running = true;
            paused = false;

            _gameTime = SwinGame.CreateTimer();
            SwinGame.StartTimer(_gameTime);

            SpaceEntities = new List<SpaceEntity>();

            SwinGame.OpenGraphicsWindow("Orbitals", 900, 500);

            SwinGame.ClearScreen(Color.White);


            _lastTicks = SwinGame.TimerTicks(_gameTime);
        }
        

        public void Update()
        {
            //records all user inputs
            SwinGame.ProcessEvents();
            if (SwinGame.WindowCloseRequested())
                Running = false;

            //code for getting new objects from input goes here
            /*
            if (SwinGame.MouseDown(MouseButton.LeftButton))
            {
                var inital_pos = SwinGame.MousePosition();
                do
                {
                    SpaceEntities.Add(new Planet());
                } while (SwinGame.MouseDown(MouseButton.LeftButton));
                var final_pos = SwinGame.MousePosition();
            }*/

            // if key p is pressed to pause
            if (SwinGame.KeyTyped(KeyCode.vk_p))
                TogglePause();

            // how much time has passed
            var currentTicks = SwinGame.TimerTicks(_gameTime);
            double dt = currentTicks - _lastTicks;
            _lastTicks = currentTicks;


            if (!paused)
            {
                // the length of time between each calculation
                //increase denominator for more smoother running
                dt = dt / 100.0;

                //calculates relative accelerations of each space entity
                Calculate.Acceleration(SpaceEntities);

                //update each space entity's details
                foreach (var spaceEntity in SpaceEntities)
                    spaceEntity.Update(dt);

                //if planet collides with blackhole
                //spaceEntity.alive = false;
                SpaceEntities.RemoveAll(s => !s.alive); //remove all space entities that arent alive
            }
        }
        

        public void Render()
        {
            SwinGame.ClearScreen(Color.White);
            foreach (var spaceEntity in SpaceEntities)
                spaceEntity.Render(); //tells each space entity to render itself

            SwinGame.RefreshScreen();
        }

        //when pressed key p
        public void TogglePause()
        {
            //    // pause ourselves
            //    paused = !paused;
            //    SwinGame.PauseTimer(_gameTime);
            //    // stop any balls flying also ... needed?
            //    foreach (var spaceEntity in SpaceEntities)
            //        spaceEntity.paused = paused;
            //    Console.WriteLine("Paused: " + paused);
        }

        public bool Running { get; private set; }
    }
}