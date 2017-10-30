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
        private Color _clr; //colour of the current planet

        private int _count; //count on the clicks

        private readonly Input _input;
        public Vector2D MousePos;

        private readonly Panel _panel;

        private readonly List<SpaceEntity> _spaceEntities;

        private int _size; //space entity size
        private Dictionary<int, int> _blackholeSizes;
        private Dictionary<int, int> _planetSizes;

        //contains the statuses of the game
        //pause, overlap, lackhole status

        public Dictionary<string, bool> Status { get; private set; }

        public Game()
        {
            //initialise all game variables
            //including statuses, blackhole and planet sizes
            InitialiseVariables();

            //start the timer
            _gameTime = SwinGame.CreateTimer();
            SwinGame.StartTimer(_gameTime);


            _spaceEntities = new List<SpaceEntity>();

            SwinGame.OpenGraphicsWindow("Orbitals", 1300, 700);
            _panel = new Panel();

            SwinGame.ClearScreen(Color.White);

            _input = new Input();
            MousePos = null;

            _count = 0;

            _lastTicks = SwinGame.TimerTicks(_gameTime);
        }

        public void Update()
        {
            //records all user inputs
            SwinGame.ProcessEvents();
            if (SwinGame.WindowCloseRequested())
                Status["running"] = false;


            // how much time has passed
            var currentTicks = SwinGame.TimerTicks(_gameTime);
            double dt = currentTicks - _lastTicks;
            _lastTicks = currentTicks;


            _input.GetKey(Status);

            if (!Status["paused"])
            {
                MousePos = _input.GetInput(); //get mouse status and position

                // the length of time between each calculation
                //decrease denominator for faster running
                dt = dt / 100.0;

                //calculates relative accelerations of each space entity
                Calculate.Acceleration(_spaceEntities);

                //update each space entity's details
                foreach (var spaceEntity in _spaceEntities)
                    spaceEntity.Update(dt);

                //enable/disable overlapping
                if (Status["overlap"])
                    ToggleOverlap();

                //if planet collides with blackhole
                //spaceEntity.alive = false;
                _spaceEntities.RemoveAll(s => !s.alive); //remove all space entities that arent alive
            }
            //pause game
            TogglePause();
        }

        public void Render()
        {
            SwinGame.ClearScreen(Color.White);
            _size = _input.GetSize(_size);
            //if mouse is down
            //and not released yet
            if (MousePos != null)
                if (!_input.Released)
                {
                    if (Status["blackhole"])
                    {
                        SwinGame.FillCircle(SwinGame.HSBColor((float)0.63, (float)0.21, (float)0.30), MousePos.asPoint2D(), _blackholeSizes[_size]);
                    }
                    else
                    {
                        //if it is the first iteration
                        //decide the color of the planet and it's lines
                        //using hsb with a random hue
                        if (_count == 0)
                        {
                            var rnd = new Random();
                            _clr = SwinGame.HSBColor((float) rnd.Next(0, 100) / 100, (float) 0.5, (float) 0.7);
                        }
                        _count += 1;

                        SwinGame.FillCircle(_clr, MousePos.asPoint2D(), _planetSizes[_size]); //draws the planet

                        //velocity of the planet once it's launched
                        var vel = (_input.VStart - MousePos) / 10;

                        //line added with the properties of the planet
                        var l = new Line(MousePos, 15, _clr);
                        l.vel = vel;
                        var lines = new List<SpaceEntity>();
                        lines.Add(l);
                        //the other spaceentities are added to the same list
                        //since the path is predicted dynamically,
                        //depending on other objects in the game
                        lines.AddRange(_spaceEntities);

                        //simulate the planet's release from the current position in to the future
                        for (var i = 0; i < 100; i++)
                        {
                            Calculate.Acceleration(lines);
                            lines[0].Update(0.5); //length between each dot
                            lines[0].Render();
                        }

                        //draws line from intial click position to current position
                        SwinGame.DrawLine(Color.Grey, _input.VStart.asPoint2D(), _input.VCurrent.asPoint2D());
                    }
                }
                //when mouse is released, i.e when planet is launched
                else
                {
                    if (Status["blackhole"])
                    {
                        _spaceEntities.Add(new Blackhole(MousePos, _blackholeSizes[_size]));
                    }
                    else
                    {
                        _count = 0; //when released reset the counter

                        //the velocity of the planet relative to the intial click position
                        var vel = (_input.VStart - MousePos) / 10;

                        var p = new Planet(MousePos, _planetSizes[_size], _clr);
                        p.vel = vel;
                        _spaceEntities.Add(p);
                    }
                }

            //check if any deletes were requested
            if (Status["blackhole"])
                DeletBlackhole();

            foreach (var spaceEntity in _spaceEntities)
                spaceEntity.Render(); //tells each space entity to render itself


            _panel.Draw(Status, _size);//draws panel

            SwinGame.RefreshScreen();
        }

        //when pressed key p
        public void TogglePause()
        {
            //pause all space entities
            foreach (var spaceEntity in _spaceEntities.OfType<Planet>())
                spaceEntity.paused = Status["paused"];
        }


        public void ToggleOverlap()
        {
            //checks if any of the planets
            //is overlapping with any of the blacholes
            foreach (var p in _spaceEntities.OfType<Planet>())
            foreach (var b in _spaceEntities.OfType<Blackhole>())
                if (SwinGame.PointInCircle(p.pos.asPoint2D(), (float) b.pos.x, (float) b.pos.y, (float) b.Mass))
                    p.alive = false;
        }

        public void DeletBlackhole()
        {
            if (_input.GetDelete() == null) return;
            foreach (var b in _spaceEntities.OfType<Blackhole>())
                //checks whether the click position was inside a blackhole
                if (SwinGame.PointInCircle(_input.GetDelete().asPoint2D(),
                    (float) b.pos.x, (float) b.pos.y, (float) b.Mass))
                    b.alive = false;
        }

        public void InitialiseVariables()
        {
            //initialising the game statuses
            Status = new Dictionary<string, bool>
            {
                {"running", true},
                {"paused", false},
                {"overlap", false},
                {"blackhole", false}
            };

            _size = 2; //default size

            //blackhole masses
            _blackholeSizes = new Dictionary<int, int>();
            for (int i = 1; i< 4; i++)
            {
                //30, 40, 50
                _blackholeSizes.Add(i, 20 + i * 10);
            }

            //planet masses
            _planetSizes = new Dictionary<int, int>();
            for (int i = 1; i < 4; i++)
            {
                //15,20,25
                _planetSizes.Add(i, 10 + i * 5);
            }
        }
    }
}