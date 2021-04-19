using System;

namespace MovieLibraryEntityFramework
{
    class Program
    {
        private static Menu menu = new Menu();        
        private static bool isRunning = true;
        public Program()
        {
            menu = new Menu();            
        }
        static void Main(string[] args)
        {
            while (isRunning)
            {
                Start();
            }
            End();
        }

        public static void Start()
        {
            menu.Print();
        }

        public static void End()
        {

        }
    }
}
