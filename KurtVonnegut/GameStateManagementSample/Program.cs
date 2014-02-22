using System;
using System.Linq;

namespace GameStateManagementSample
{
    #if WINDOWS || XBOX
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (GameStateManagementGame game = new GameStateManagementGame())
            {
                game.Run();
            }
        }
    }
    #endif
}