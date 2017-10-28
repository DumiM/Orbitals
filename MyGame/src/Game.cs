﻿using System.Collections.Generic;
using SwinGameSDK;
using System;

namespace MyGame
{
    public class Game
    {
        //to keep track of time
        private readonly Timer _gameTime;
        private uint _lastTicks;

        private readonly bool paused;

        public Input input;
        public Vector2D MousePos;

        public double dt;

        public List<SpaceEntity> SpaceEntities;

        public int count; //count on the clicks
        public Color clr; //colour of the current planet

        public Game()
        {
            Running = true;
            paused = false;

            //start the timer
            _gameTime = SwinGame.CreateTimer();
            SwinGame.StartTimer(_gameTime);
            

            SpaceEntities = new List<SpaceEntity>();

            SwinGame.OpenGraphicsWindow("Orbitals", 900, 500);

            SwinGame.ClearScreen(Color.White);

            input = new Input();
            MousePos = null;

            count = 0;
            

            //init tests
            //only for reference untill input is implemented
            var testv3 = new Vector2D(30, 20);
            testv3.x = 630;
            testv3.y = 350;
            SpaceEntities.Add(new Blackhole(testv3, 30));
            var testv2 = new Vector2D(30, 20);
            testv2.x = 430;
            testv2.y = 150;
            SpaceEntities.Add(new Blackhole(testv2, 30));


            _lastTicks = SwinGame.TimerTicks(_gameTime);
        }

        public bool Running { get; private set; }


        public void Update()
        {
            //records all user inputs
            SwinGame.ProcessEvents();
            if (SwinGame.WindowCloseRequested())
                Running = false;

            // if key p is pressed to pause
            if (SwinGame.KeyTyped(KeyCode.vk_p))
                TogglePause();

            // how much time has passed
            var currentTicks = SwinGame.TimerTicks(_gameTime);
            double dt = currentTicks - _lastTicks;
            _lastTicks = currentTicks;

            MousePos = input.GetInput(); //get mouse status and position
            
            if (!paused)
            {
                // the length of time between each calculation
                //decrease denominator for faster running
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

            //if mouse is down
            //and not released yet
            if (MousePos != null)
                if (!input.released)
                {
                    //if it is the first iteration
                    //decide the color of the planet and it's lines
                    //using hsb with a random hue
                    if (count == 0)
                    {
                        Random rnd = new Random();
                        clr = SwinGame.HSBColor((float)rnd.Next(0, 100) / 100, (float)0.5, (float)0.7);
                    }
                    count += 1;

                    SwinGame.FillCircle(clr, MousePos.asPoint2D(), 15);//draws the planet

                    //velocity of the planet once it's launched
                    var vel = (input.vStart - MousePos) / 10;

                    //line added with the properties of the planet
                    var l = new Line(MousePos, 15, clr);
                    l.vel = vel;
                    List<SpaceEntity> lines = new List<SpaceEntity>();
                    lines.Add(l);
                    //the other spaceentities are added to the same list
                    //since the path is predicted dynamically,
                    //depending on other objects in the game
                    lines.AddRange(SpaceEntities); 

                    //simulate the planet's release from the current position in to the future
                    for (int i = 0; i < 100; i++)
                    {
                        Calculate.Acceleration(lines);
                        lines[0].Update(0.5);
                        lines[0].Render();

                    }

                    //draws line from intial click position to current position
                    SwinGame.DrawLine(Color.Grey, input.vStart.asPoint2D(), input.vCurrent.asPoint2D());
                }
                //when mouse is released, i.e when planet is launched
                else
                {
                    count = 0; //when released reset the counter

                    //the velocity of the planet relative to the intial click position
                    var vel = (input.vStart - MousePos) / 10;

                    var p = new Planet(MousePos, 15, clr);
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