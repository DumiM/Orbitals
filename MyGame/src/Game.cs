using System;
using System.Collections.Generic;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    public class Game
    {
        //to keep track of time
        private readonly Timer _gameTime;

        private uint _lastTicks;
        public Color clr; //colour of the current planet

        public int count; //count on the clicks

        public double dt;

        public Input input;
        public Vector2D MousePos;

        public List<SpaceEntity> SpaceEntities;

        //contains the statuses of the game
        //pause, overlap, lackhole status
        public Dictionary<string, bool> status;

        public Game()
        {
            //initialising the game statuses
            status = new Dictionary<string, bool>();
            status.Add("running", true);
            status.Add("paused", false);
            status.Add("overlap", false);
            status.Add("blackhole", false);

            //start the timer
            _gameTime = SwinGame.CreateTimer();
            SwinGame.StartTimer(_gameTime);


            SpaceEntities = new List<SpaceEntity>();

            SwinGame.OpenGraphicsWindow("Orbitals", 900, 500);

            SwinGame.ClearScreen(Color.White);

            input = new Input();
            MousePos = null;

            count = 0;

            _lastTicks = SwinGame.TimerTicks(_gameTime);
        }

        public void Update()
        {
            //records all user inputs
            SwinGame.ProcessEvents();
            if (SwinGame.WindowCloseRequested())
                status["running"] = false;


            // how much time has passed
            var currentTicks = SwinGame.TimerTicks(_gameTime);
            double dt = currentTicks - _lastTicks;
            _lastTicks = currentTicks;


            input.GetKey(status);

            if (!status["paused"])
            {
                MousePos = input.GetInput(); //get mouse status and position

                // the length of time between each calculation
                //decrease denominator for faster running
                dt = dt / 100.0;

                //calculates relative accelerations of each space entity
                Calculate.Acceleration(SpaceEntities);

                //update each space entity's details
                foreach (var spaceEntity in SpaceEntities)
                    spaceEntity.Update(dt);

                //enable/disable overlapping
                if (status["overlap"])
                    ToggleOverlap();

                //if planet collides with blackhole
                //spaceEntity.alive = false;
                SpaceEntities.RemoveAll(s => !s.alive); //remove all space entities that arent alive
            }
            //pause game
            TogglePause();
        }

        public void Render()
        {
            SwinGame.ClearScreen(Color.White);

            //if mouse is down
            //and not released yet
            if (MousePos != null)
                if (!input.released)
                {
                    if (status["blackhole"])
                    {
                        SwinGame.FillCircle(Color.Black, MousePos.asPoint2D(), 30);
                    }
                    else
                    {
                        //if it is the first iteration
                        //decide the color of the planet and it's lines
                        //using hsb with a random hue
                        if (count == 0)
                        {
                            var rnd = new Random();
                            clr = SwinGame.HSBColor((float) rnd.Next(0, 100) / 100, (float) 0.5, (float) 0.7);
                        }
                        count += 1;

                        SwinGame.FillCircle(clr, MousePos.asPoint2D(), 15); //draws the planet

                        //velocity of the planet once it's launched
                        var vel = (input.vStart - MousePos) / 10;

                        //line added with the properties of the planet
                        var l = new Line(MousePos, 15, clr);
                        l.vel = vel;
                        var lines = new List<SpaceEntity>();
                        lines.Add(l);
                        //the other spaceentities are added to the same list
                        //since the path is predicted dynamically,
                        //depending on other objects in the game
                        lines.AddRange(SpaceEntities);

                        //simulate the planet's release from the current position in to the future
                        for (var i = 0; i < 100; i++)
                        {
                            Calculate.Acceleration(lines);
                            lines[0].Update(0.5); //length between each dot
                            lines[0].Render();
                        }

                        //draws line from intial click position to current position
                        SwinGame.DrawLine(Color.Grey, input.vStart.asPoint2D(), input.vCurrent.asPoint2D());
                    }
                }
                //when mouse is released, i.e when planet is launched
                else
                {
                    if (status["blackhole"])
                    {
                        SpaceEntities.Add(new Blackhole(MousePos, 30));
                    }
                    else
                    {
                        count = 0; //when released reset the counter

                        //the velocity of the planet relative to the intial click position
                        var vel = (input.vStart - MousePos) / 10;

                        var p = new Planet(MousePos, 15, clr);
                        p.vel = vel;
                        SpaceEntities.Add(p);
                    }
                }

            //check if any deletes were requested
            if (status["blackhole"])
                DeletBlackhole();

            foreach (var spaceEntity in SpaceEntities)
                spaceEntity.Render(); //tells each space entity to render itself

            SwinGame.RefreshScreen();
        }

        //when pressed key p
        public void TogglePause()
        {
            //pause all space entities
            foreach (var spaceEntity in SpaceEntities.OfType<Planet>())
                spaceEntity.paused = status["paused"];
        }


        public void ToggleOverlap()
        {
            //checks if any of the planets
            //is overlapping with any of the blacholes
            foreach (var p in SpaceEntities.OfType<Planet>())
            foreach (var b in SpaceEntities.OfType<Blackhole>())
                if (SwinGame.PointInCircle(p.pos.asPoint2D(), (float) b.pos.x, (float) b.pos.y, (float) b.Mass))
                    p.alive = false;
        }

        public void DeletBlackhole()
        {
            if (input.GetDelete() != null) //checks if a delete was requested
                foreach (var b in SpaceEntities.OfType<Blackhole>())
                    //checks whether the click position was inside a blackhole
                    if (SwinGame.PointInCircle(input.GetDelete().asPoint2D(),
                        (float) b.pos.x, (float) b.pos.y, (float) b.Mass))
                        b.alive = false;
        }
    }
}