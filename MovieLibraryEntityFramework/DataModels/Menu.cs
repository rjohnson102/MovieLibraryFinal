using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MovieLibraryEntityFramework
{
    class Menu
    {
        private static Dictionary<int, string> menuOptions;
        private static Parse parse;
        private static DatabaseRepository context;

        public Menu()
        {
            context = new DatabaseRepository();
            menuOptions = new Dictionary<int, string>()
            {
                {1, "List All Movies"},
                {2, "Search Movie"},
                {3, "Add Movie"},
                {4, "Update Movie"},
                {5, "Delete Movie"},
                {6, "Add New User"},
                {7, "Display User Info" },
                {8, "Add Movie Rating" },
                
            };
            parse = new Parse();            
        }

        private void Execute(int option)
        {
            switch (option)
            {
                case 1:
                    context.ListAllMovies();
                    Print();
                    break;
                case 2:
                    SearchMovieContext();                    
                    Print();
                    break;
                case 3:
                    AddMovieContext();
                    break;
                case 4:
                    UpdateMovieContext();
                    Print();
                    break;
                case 5:
                    DeleteMovieContext();
                    Print();
                    break;
                case 6:
                    AddNewUserContext();
                    break;
                case 7:
                    //DisplayUsersContext();
                    break;
                case 8:
                    //AddMovieRatingContext();
                    break;
            }
        }

        public void Print()
        {
            foreach(KeyValuePair<int,string> menuOption in menuOptions)
            {
                Console.WriteLine(menuOption.Key + ". " + menuOption.Value);
            }         
            int option = ReadMenuOption();
            Execute(option);
        }

        private int ReadMenuOption()
        {
            bool isInt = false;
            string input = Console.ReadLine();
            isInt = parse.ParseInt(input);
            while(isInt == false)
            {
                input = Console.ReadLine();
                isInt = parse.ParseInt(input);
            }
            int i = Convert.ToInt32(input);
            foreach(KeyValuePair<int, string> option in menuOptions)
            {
                if(i == option.Key)
                {
                    return i;
                }                
            }
            Console.WriteLine("\n~Enter Valid Option~\n");
            Print();
            return -1;
        }

        private void SearchMovieContext()
        {
            Console.WriteLine("What movie would you like to search?");
            string name = Console.ReadLine();
            context.SearchMovie(name);
        }

        private void DeleteMovieContext()
        {
            Console.WriteLine();
            context.ListAllMovies();
            Console.WriteLine();
            Console.WriteLine("Which movie would you like to delete? Enter ID:");
            string input = Console.ReadLine();
            bool isInt = parse.ParseInt(input);
            if (isInt)
            {
                int choice = Convert.ToInt32(input);
                context.DeleteMovie(choice);
            }
            else
            {
                Console.WriteLine("\n~Enter Valid Movie ID~\n");
                DeleteMovieContext();
            }

        }

        private void UpdateMovieContext()
        {
            Console.WriteLine();
            context.ListAllMovies();
            Console.WriteLine();
            Console.WriteLine("Which movie would you like to Update? Enter ID:");
            string input = Console.ReadLine();
            bool isInt = parse.ParseInt(input);
            if (isInt)
            {
                int choice = Convert.ToInt32(input);
                context.DeleteMovie(choice);
                Console.WriteLine("\n~New Movie Details~\n");
                AddMovieContext();
            }
            else
            {
                Console.WriteLine("\n~Enter Valid Movie ID~\n");
                UpdateMovieContext();
            }
        }

        private void AddMovieContext()
        {
            bool isInt = false;
            string dateTime;
            int month=0;
            int day=0;
            int year=0;
            Console.WriteLine("\n~Movie Name~\n");
            string name = Console.ReadLine();
            Console.WriteLine("\n~Release Date~\n");
            while(isInt == false)
            {
                Console.Write("Month(mm): ");
                string entry = Console.ReadLine();
                isInt = parse.ParseInt(entry);
                month = Convert.ToInt32(entry);
            }
            isInt = false;
            while (isInt == false)
            {
                Console.Write("Day(dd): ");
                string entry = Console.ReadLine();
                isInt = parse.ParseInt(entry);
                day = Convert.ToInt32(entry);
            }
            isInt = false;
            while (isInt == false)
            {
                Console.Write("Year(yyyy): ");
                string entry = Console.ReadLine();
                isInt = parse.ParseInt(entry);
                year = Convert.ToInt32(entry);
            }
            dateTime = month + "/" + day + "/" + year;
            DateTime releaseDate = DateTime.Parse(dateTime);
            Movie movie = new Movie
            {
                Title = name,
                ReleaseDate = releaseDate
            };
            context.AddMovie(movie);
        }

        public void AddNewUserContext()
        {
            bool isInt = false;
            long age = 0;
            string gender = "";
            string zipCode = "";
            Occupation occupation = new Occupation();
            while (isInt == false)
            {
                Console.WriteLine("\nEnter Age: \n");
                string input = Console.ReadLine();
                isInt = parse.ParseInt(input);
                if(isInt == true)
                {
                    age = Convert.ToInt32(input);
                }
            }
            isInt = false;
            while(gender.ToLower() != "m" || gender.ToLower() != "f")
            {
                Console.WriteLine("\nEnter Gender: (M/F)\n");
                gender = Console.ReadLine();
                if(gender.ToLower() == "m" || gender.ToLower() == "f")
                {
                    break;
                }
            }
            Console.WriteLine("\nEnter Zip Code\n");
            zipCode = Console.ReadLine();
            

            using(var db = new MovieContext())
            {
                var occupations = db.Occupations.ToList();
                foreach(var occupation1 in occupations)
                {
                    Console.WriteLine("(" +occupation1.Id + ") " + occupation1.Name);
                }
            }

            while (isInt == false)
            {
                Console.WriteLine("\n~Enter Occupation ID~\n");
                string input = Console.ReadLine();
                isInt = parse.ParseInt(input);
                if (isInt == true)
                {
                    int id = Convert.ToInt32(input); 
                    using(var db = new MovieContext())
                    {
                        occupation = db.Occupations.FirstOrDefault(o => o.Id == id);                        
                    }
                }
            }

            User user = new User()
            {                  
                Age = age,
                Gender = gender,
                ZipCode = zipCode,   
                Occupation = occupation
            };            
            context.AddNewUser(user);

        }


    }
}
