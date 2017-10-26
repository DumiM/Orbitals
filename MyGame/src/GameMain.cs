namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
            var game = new Game();

            while (game.Running)
            {
                game.Update();
                game.Render();
            }
        }
    }
}