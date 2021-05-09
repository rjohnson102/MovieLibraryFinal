using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NLog;

namespace MovieLibraryEntityFramework
{
    class Menu
    {
        Logger logger = LogManager.GetCurrentClassLogger();
        public bool isRunning = true;
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
                {9, "Top Rated Movie" },
                {10, "End" }
                
            };
            parse = new Parse();            
        }

        private void Execute(int option)
        {
            switch (option)
            {
                case 1:
                    ListAllMoviesContext();
                    Print();
                    break;
                case 2:
                    SearchMovieContext();                    
                    Print();
                    break;
                case 3:
                    Movie movie = AddMovieContext();
                    context.AddMovie(movie);
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
                    DisplayUsersContext();
                    break;
                case 8:
                    AddMovieRatingContext();
                    break;
                case 9:
                    TopRatedContext();
                    break;
                case 10:
                    isRunning = false;
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
            logger.Error("Menu Input Not Valid");
            Print();
            return -1;
        }

        private void ListAllMoviesContext() {
            bool isInt = false;
            int top = 10;
            while (isInt == false)
            {
                Console.WriteLine("How Many Movies Would You like to view?");
                string entry = Console.ReadLine();
                isInt = parse.ParseInt(entry);
                if (isInt)
                {
                    top = Convert.ToInt32(entry);
                }
            }
            context.ListAllMovies(top);
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
            SearchMovieContext();
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
                logger.Error("DeleteMovieContext() Invalid Movie ID");
                DeleteMovieContext();
            }

        }

        private void UpdateMovieContext()
        {
            Console.WriteLine();
            SearchMovieContext();
            Console.WriteLine();
            Console.WriteLine("Which movie would you like to Update? Enter ID:");
            string input = Console.ReadLine();
            bool isInt = parse.ParseInt(input);
            if (isInt)
            {
                int choice = Convert.ToInt32(input);
                context.DeleteMovie(choice);
                Console.WriteLine("\n~New Movie Details~\n");
                Movie movie = AddMovieContext();
                context.DeleteMovie(movie.Id);
                context.AddMovie(movie);
            }
            else
            {
                Console.WriteLine("\n~Enter Valid Movie ID~\n");
                logger.Error("UpdateMovieContext() Invalid Movie ID");
                UpdateMovieContext();
            }
        }

        private Movie AddMovieContext()
        {
            DateTime releaseDate = DateTime.Now;
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
                if (isInt)
                {
                    month = Convert.ToInt32(entry);
                }                
            }
            isInt = false;
            while (isInt == false)
            {
                Console.Write("Day(dd): ");
                string entry = Console.ReadLine();
                isInt = parse.ParseInt(entry);
                if (isInt)
                {
                    day = Convert.ToInt32(entry);
                }                
            }
            isInt = false;
            while (isInt == false)
            {
                Console.Write("Year(yyyy): ");
                string entry = Console.ReadLine();
                isInt = parse.ParseInt(entry);
                if (isInt)
                {
                    year = Convert.ToInt32(entry);
                }                
            }
            dateTime = month + "/" + day + "/" + year;
            try
            {
                releaseDate = DateTime.Parse(dateTime);
            }
            catch
            {
                Console.WriteLine("Invalid DateTime. Date set to Current Time.");
                logger.Error("AddMovieContext() DateTime Invalid");
            }
            Movie movie = new Movie
            {
                Title = name,
                ReleaseDate = releaseDate
            };
            return movie;
        }

        private void AddNewUserContext()
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
            while(gender.ToUpper() != "M" || gender.ToUpper() != "M")
            {
                Console.WriteLine("\nEnter Gender: (M/F)\n");
                gender = Console.ReadLine().ToUpper();
                if(gender.ToUpper() == "M" || gender.ToUpper() == "F")
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
                        try
                        {
                            occupation = db.Occupations.FirstOrDefault(o => o.Id == id);
                        }
                        catch
                        {
                            logger.Error("AddNewUserContext() Invalid Occupation ID");
                            Console.WriteLine("Invalid Occupation ID");
                            isInt = false;
                        }
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
            context.AddNewUser(user, occupation);

        }

        private void DisplayUsersContext()
        {
            bool isInt = false;
            int userId = 0;
            while (isInt == false)
            {
                Console.WriteLine("\nEnter User ID: \n");
                string input = Console.ReadLine();
                isInt = parse.ParseInt(input);
                if (isInt == true)
                {
                    userId = Convert.ToInt32(input);
                }
            }
            context.DisplayUserInfo(userId);
        }

        private void AddMovieRatingContext()
        {
            bool isInt = false;
            int userId = 0;
            int movieId = 0;
            int rating = 0;
            while (isInt == false)
            {
                Console.WriteLine("\n~Enter User ID~\n");
                string input = Console.ReadLine();
                isInt = parse.ParseInt(input);
                if (isInt == true)
                {
                    userId = Convert.ToInt32(input);
                }
            }
            isInt = false;
            SearchMovieContext();            
            while (isInt == false)
            {
                Console.WriteLine("Enter Movie ID to confirm");
                string input = Console.ReadLine();
                isInt = parse.ParseInt(input);
                if (isInt == true)
                {
                    movieId = Convert.ToInt32(input);
                }
            }
            isInt = false;
            while (isInt == false)
            {
                Console.WriteLine("Enter Rating : (1-5)");
                string input = Console.ReadLine();
                isInt = parse.ParseInt(input);
                if (isInt == true)
                {
                    rating = Convert.ToInt32(input);
                }
                if(rating > 0 && rating < 6)
                {
                    continue;
                }
                else
                {
                    isInt = false;
                }
            }

            User user = new User();
            Movie movie = new Movie();

            using(var db = new MovieContext())
            {
                user = db.Users.FirstOrDefault(u => u.Id == userId);
            }
            using (var db = new MovieContext())
            {
                movie = db.Movies.FirstOrDefault(m => m.Id == movieId);
            }


            context.AddMovieRating(user, movie, rating);
        }

        private void TopRatedContext()
        {
            bool isInt = false;
            int choice = 0;
            while (isInt == false)
            {
                Console.WriteLine("\n~List By~\n");
                Console.WriteLine("(1) Age Bracket");
                Console.WriteLine("(2) Occupation");
                string input = Console.ReadLine();
                isInt = parse.ParseInt(input);
                if (isInt == true)
                {
                    choice = Convert.ToInt32(input);
                }
                if(choice == 1 || choice == 2)
                {
                    switch (choice)
                    {
                        case 1:                            
                            context.ListTopRatedAge(10, 20);
                            context.ListTopRatedAge(20, 30);
                            context.ListTopRatedAge(30, 40);
                            context.ListTopRatedAge(40, 50);
                            context.ListTopRatedAge(50, 60);
                            context.ListTopRatedAge(60, 100);                            
                            break;
                        case 2:
                            using (var db = new MovieContext())
                            {
                                var occ = from o in db.Occupations
                                          select new
                                          {
                                              o.Id,
                                              o.Name
                                          };
                                foreach (var t in occ)
                                {
                                    context.ListTopRatedOccupation(t.Id, t.Name);
                                }
                            }                            
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("\n~Invalid Option~\n");
                    logger.Error("TopRatedContext() Invalid Option");
                }
            }
        }


    }
}
