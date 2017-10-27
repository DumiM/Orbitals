using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class Game
    {
        //to keep track of time
        private readonly Timer _gameTime;

        private readonly bool paused;

        private uint _lastTicks;

        public Input input;
        public Vector2D MousePos;

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

            input = new Input();
            MousePos = null;

            //init tests
            //only for reference untill input is implemented
            var testv3 = new Vector2D(30, 20);
            testv3.x = 630;
            testv3.y = 350;
            SpaceEntities.Add(new Blackhole(testv3, Calculate.DegreesToRadians(30), 0, 30));
            var testv2 = new Vector2D(30, 20);
            testv2.x = 430;
            testv2.y = 150;
            SpaceEntities.Add(new Blackhole(testv2, Calculate.DegreesToRadians(30), 0, 30));


            _lastTicks = SwinGame.TimerTicks(_gameTime);
        }

        public bool Running { get; private set; }


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

            MousePos = input.GetInput();

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

            if (MousePos != null)
                if (!input.released)
                {
                    SwinGame.FillCircle(Color.Black, MousePos.asPoint2D(), 15);
                }
                else
                {
                    var vel = (input.vStart - MousePos) / 10;

                    var p = new Planet(MousePos, Calculate.DegreesToRadians(45 + 40), 0, 15);
                    p.vel = vel;
                    SpaceEntities.Add(p);
                }

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
    }
}