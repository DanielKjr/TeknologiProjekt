using System;
using System.Threading;

namespace TeknologiProjekt
{
    public static class Program
    {
        private static Mutex m;
        const string gameName = "Game";
        [STAThread]
        static void Main()
        {
            m = new Mutex(true, gameName, out bool createdNew);
            if (!createdNew)
            {
                return;
            }
            using (var game = new GameWorld())
                game.Run();
        }
    }
}
