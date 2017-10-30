using System.Collections.Generic;
using SwinGameSDK;

namespace MyGame
{
    public class Input
    {
        private int _count;
        public bool Released;
        public Vector2D VStart, VCurrent, VEnd, VDelete;

        public Input()
        {
            Released = true;
            _count = 0;
            VStart = new Vector2D();
        }

        public Vector2D GetInput()
        {
            if (SwinGame.MouseDown(MouseButton.LeftButton))
            {
                //store current mouse position as a vector when mouse is down
                VCurrent = new Vector2D
                {
                    x = SwinGame.MousePositionAsVector().X,
                    y = SwinGame.MousePositionAsVector().Y
                };
                Released = false;

                //if its the first time of this loop
                //make this position the starting position of the planet
                if (_count == 0)
                    VStart = VCurrent;
                _count += 1;
                return VCurrent;
            }

            if (!Released)
                //when released, record the mouse position as a vector
                if (SwinGame.MouseUp(MouseButton.LeftButton))
                {
                    VEnd = new Vector2D
                    {
                        x = SwinGame.MousePositionAsVector().X,
                        y = SwinGame.MousePositionAsVector().Y
                    };
                    Released = true;
                    _count = 0;
                    return VEnd;
                }
            return null;
        }

        //checks if a delete was requested
        //if it was, returns the position as a vector
        public Vector2D GetDelete()
        {
            if (SwinGame.MouseClicked(MouseButton.RightButton))
            {
                VDelete = new Vector2D
                {
                    x = SwinGame.MousePositionAsVector().X,
                    y = SwinGame.MousePositionAsVector().Y
                };
                return VDelete;
            }
            return null;
        }

        //changes the game's status varibles
        //with key presses
        public void GetKey(Dictionary<string, bool> status)
        {
            if (SwinGame.KeyTyped(KeyCode.vk_p))
                status["paused"] = !status["paused"];
            if (SwinGame.KeyTyped(KeyCode.vk_b))
                status["blackhole"] = !status["blackhole"];
            if (SwinGame.KeyTyped(KeyCode.vk_o))
                status["overlap"] = !status["overlap"];
        }
        public int GetSize(int size)
        {
            if (SwinGame.KeyTyped(KeyCode.vk_1))
                return 1;
            if (SwinGame.KeyTyped(KeyCode.vk_2))
                return 2;
            if (SwinGame.KeyTyped(KeyCode.vk_3))
                return 3;
            return size;
        }
    }
}