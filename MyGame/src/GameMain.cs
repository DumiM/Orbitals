namespace MyGame
{
    public class GameMain
    {
        public static void Main()
        {
            var game = new Game();

            while (game.Status["running"])
            {
                game.Update();
                game.Render();
            }
        }
    }
}