using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwinGameSDK;

namespace MyGame
{
    public class Input
    {
        public Vector2D vStart, vCurrent, vEnd;
        public bool released;
        public int count;

        public Input()
        {
            released = true;
            count = 0;
            vStart = new Vector2D();
        }
        public Vector2D GetInput()
        {

            if (SwinGame.MouseDown(MouseButton.LeftButton))
            {
                vCurrent = new Vector2D();
                vCurrent.x = SwinGame.MousePositionAsVector().X;
                vCurrent.y = SwinGame.MousePositionAsVector().Y;
                released = false;

                if (count == 0)
                {
                    vStart = vCurrent;
                }
                count += 1;
                return vCurrent;
            }

            if (!released)
            {
                if (SwinGame.MouseUp(MouseButton.LeftButton))
                {
                    vEnd = new Vector2D();
                    vEnd.x = SwinGame.MousePositionAsVector().X;
                    vEnd.y = SwinGame.MousePositionAsVector().Y;
                    released = true;
                    count = 0;
                    return vEnd;
                }
            }
            return null;
        }
    }
}
