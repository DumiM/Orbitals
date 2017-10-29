using System.Collections.Generic;
using System.IO;
using System.Linq;
using SwinGameSDK;

namespace MyGame
{
    public class Panel
    {
        private readonly Color clr;
        private readonly int height;
        private readonly int width;
        private readonly float x;
        public List<string> helpText;
        private float y;

        public Panel()
        {
            //panel size relative to the window size
            width = 250;
            height = SwinGame.ScreenHeight();
            x = SwinGame.ScreenWidth() - width;
            y = 0;
            clr = SwinGame.HSBColor((float) 0.48, 1, (float) 0.7);

            //help tips text loaded from external file
            helpText = new List<string>();
            LoadText("/Docs/help_text.txt", helpText);
        }

        public void Draw(Dictionary<string, bool> status)
        {
            SwinGame.FillRectangle(clr, x, 0, width, height); // draws the panel

            //THE TEXT
            //***ALL TEXT IS DRAWN RELATIVE TO THE SCREEN SIZE***
            //**As a fraction of screen width /height***
            SwinGame.DrawText("ORBITALS", Color.White, SwinGame.LoadFont("Arial", 40), x + width / 10,
                height / 15); //heading
            SwinGame.DrawThickLine(Color.White, x, height / 5, SwinGame.ScreenWidth(), height / 5, 5); //seperator

            //the help tips drawn using the list
            for (var i = 0; i < helpText.Count; i++)
            {
                var texty = height / 4 + i * height / 35;
                SwinGame.DrawText(helpText[i], Color.White, SwinGame.LoadFont("Arial", 14), x + width / 15, texty);
            }

            //draws the details of the game variables
            SwinGame.DrawLine(Color.White, x, (float) (height / 1.5), SwinGame.ScreenWidth(), (float) (height / 1.5));
            SwinGame.DrawText("Simulator Variables", Color.White, SwinGame.LoadFont("Arial", 18), x + width / 6,
                (float) (height / 1.4));
            //darws them using status dictionary key and value
            for (var i = 1; i < status.Count; i++)
            {
                var texty = (float) (height / 1.3) + i * height / 35;
                var textx = x + width / 5;
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