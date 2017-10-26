using System.Collections.Generic;
using SwinGameSDK;
using static System.Math;

namespace MyGame
{
    public class Game
    {
        public const double G = 1000; //the value of gravity in the game

        //to keep track of time
        private readonly Timer _gameTime;

        private uint _lastTicks;

        private readonly bool paused;

        public List<SpaceEntity> SpaceEntities;

        public Game(int width = 900, int height = 500)
        {
            Running = true;
            paused = false;

            _gameTime = SwinGame.CreateTimer();
            SwinGame.StartTimer(_gameTime);

            SpaceEntities = new List<SpaceEntity>();

            SwinGame.OpenGraphicsWindow("Orbitals", width, height);

            SwinGame.ClearScreen(Color.White);

            //init tests
            //only for reference untill input is implemented
            var testv = new Vector2D(300, 200);
            var testv2 = new Vector2D(30, 20);
            var testv3 = new Vector2D(30, 20);
            testv.x = 100;
            testv.y = 100;
            testv2.x = 230;
            testv2.y = 450;
            testv3.x = 630;
            testv3.y = 350;
            SpaceEntities.Add(new Planet(testv, DegreesToRadians(45 + 40), 0, 20));
            SpaceEntities.Add(new Planet(testv2, DegreesToRadians(10), 0, 25));
            SpaceEntities.Add(new Blackhole(testv3, DegreesToRadians(30), 0, 30));


            _lastTicks = SwinGame.TimerTicks(_gameTime);
        }


        public static double DegreesToRadians(double degrees)
        {
            var radians = PI / 180 * degrees;
            return radians;
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
                CalculateAllAccelerations();

                //update each space entity's details
                foreach (var spaceEntity in SpaceEntities)
                    spaceEntity.Update(dt);

                //if planet collides with blackhole
                //spaceEntity.alive = false;
                SpaceEntities.RemoveAll(s => !s.alive); //remove all space entities that arent alive
            }
        }

        public void CalculateAllAccelerations()
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