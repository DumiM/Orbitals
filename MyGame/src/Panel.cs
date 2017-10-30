using System.Collections.Generic;
using System.IO;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    public class Panel
    {
        private readonly Color _clr;
        private readonly int _height;
        private readonly int _width;
        private readonly float _x;
        public List<string> HelpText;

        public Panel()
        {
            //panel size relative to the window size
            _width = 250;
            _height = SwinGame.ScreenHeight();
            _x = SwinGame.ScreenWidth() - _width;
            _clr = SwinGame.HSBColor((float) 0.48, 1, (float) 0.7);

            //help tips text loaded from external file
            HelpText = new List<string>();
            LoadText("/Docs/help_text.txt", HelpText);
        }

        public void Draw(Dictionary<string, bool> status, int size)
        {
            SwinGame.FillRectangle(_clr, _x, 0, _width, _height); // draws the panel

            //THE TEXT
            //***ALL TEXT IS DRAWN RELATIVE TO THE SCREEN SIZE***
            //**As a fraction of screen width /height***
            SwinGame.DrawText("ORBITALS", Color.White, SwinGame.LoadFont("Arial", 40), _x + _width / 10,
                _height / 15); //heading
            SwinGame.DrawThickLine(Color.White, _x, _height / 5, SwinGame.ScreenWidth(), _height / 5, 5); //seperator

            //the help tips drawn using the list
            for (var i = 0; i < HelpText.Count; i++)
            {
                var texty = _height / 4 + i * _height / 35;
                SwinGame.DrawText(HelpText[i], Color.White, SwinGame.LoadFont("Arial", 14), _x + _width / 15, texty);
            }

            //draws the details of the game variables
            SwinGame.DrawLine(Color.White, _x, (float) (_height / 1.5), SwinGame.ScreenWidth(), (float) (_height / 1.5));
            SwinGame.DrawText("Simulator Variables", Color.White, SwinGame.LoadFont("Arial", 18), _x + _width / 6,
                (float) (_height / 1.4));
            //darws them using status dictionary key and value
            for (var i = 1; i < status.Count + 1; i++)
            {
                var texty = (float) (_height / 1.3) + i * _height / 35;
                var textx = _x + _width / 5;

                //if last iteration draw size
                if (i == status.Count)
                {
                    SwinGame.DrawText("size", Color.White, SwinGame.LoadFont("Arial", 14), textx,
                        texty);
                    SwinGame.DrawText(size.ToString(), Color.White, SwinGame.LoadFont("Arial", 14),
                        textx + 100, texty);
                    return;
                }
                //else draw rest of the variables
                SwinGame.DrawText(status.ElementAt(i).Key, Color.White, SwinGame.LoadFont("Arial", 14), textx,
                    texty);
                SwinGame.DrawText(status.ElementAt(i).Value.ToString(), Color.White, SwinGame.LoadFont("Arial", 14),
                    textx + 100, texty);
            }
        }

        //help tips text read from file
        //and stored in list
        public void LoadText(string filename, List<string> textList)
        {
            var reader = new StreamReader(filename);

            while (!reader.EndOfStream)
                textList.Add(reader.ReadLine());

            reader.Close();
        }
    }
}