using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibraryEntityFramework
{
    class Parse
    {
        public bool ParseInt(string input)
        {
            try
            {
                Convert.ToInt32(input);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine("\n~Invalid Option~\n");
                return false;
            }            
        }

    }
}
