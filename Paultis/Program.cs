using System;
using System.IO;

namespace TemptureBlockGame.src
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
		{
            string fuck = Directory.GetCurrentDirectory();
            using (var game = new TemptureGame())
                game.Run();
        }
    }
}
