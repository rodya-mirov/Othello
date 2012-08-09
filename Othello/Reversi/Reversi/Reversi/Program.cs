using System;

namespace Reversi
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (OthelloGame game = new OthelloGame())
            {
                game.Run();
            }
        }
    }
#endif
}

