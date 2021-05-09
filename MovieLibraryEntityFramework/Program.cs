using System;
using NLog;


namespace MovieLibraryEntityFramework
{    
    class Program
    {        
        private static Menu menu = new Menu();                
        public Program()
        {
            menu = new Menu();            
        }
        static void Main(string[] args)
        {
            while (menu.isRunning)
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
            Console.WriteLine("Goodbye.");
        }
    }
}
