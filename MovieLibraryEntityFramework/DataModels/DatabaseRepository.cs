using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace MovieLibraryEntityFramework
{
    class DatabaseRepository : IRepository
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        public void ListAllMovies(int top)
        {
            using (var db = new MovieContext())
            {
                try
                {
                    var movies = db.Movies.ToList().Take(top);
                    Console.WriteLine();
                    foreach (var movie in movies)
                    {
                        Console.WriteLine(movie.Id + ". " + movie.Title + "\tRelease Date: " + movie.ReleaseDate);
                    }
                    Console.WriteLine();
                }
                catch
                {
                    Console.WriteLine("Could Not Return Movie List; Check Log Files For More Info");
                    logger.Error("ListAllMovies Returned 0");
                }
            }
        }
        public void SearchMovie(string name)
        {
            using (var db = new MovieContext())
            {
                try
                {
                    var movies = db.Movies.Where(m => m.Title.ToLower().Contains(name.ToLower())).ToList();
                    foreach (var movie in movies)
                    {
                        Console.WriteLine();
                        Console.WriteLine(movie.Id + ". " + movie.Title);
                        Console.WriteLine();
                    }
                }
                catch
                {
                    Console.WriteLine("~Enter Valid Movie Name~");
                    logger.Debug("SearchMovie() not given valid parameter");
                }
            }
        }
        public void AddMovie(Movie movie)
        {
            using(var db =new MovieContext())
            {
                try
                {
                    db.Add<Movie>(movie);
                    db.SaveChanges();
                }
                catch
                {
                    Console.WriteLine("Unable to Add Movie");
                    logger.Error("AddMovie() Could not Add Movie");
                }
            }
            Console.WriteLine("\nMovie Added!: " + movie.Id + ". " + movie.Title+"\n");
        }

        public void DeleteMovie(long Id)
        {
            using (var db = new MovieContext())
            {
                try
                {
                    var movies = db.Movies.Where(m => m.Id.Equals(Id)).ToList();
                    Console.WriteLine();
                    foreach (var movie in movies)
                    {
                        db.Movies.Remove(movie);
                        Console.WriteLine("Movie Deleted: " + movie.Id + ". " + movie.Title + "\n");
                    }
                    db.SaveChanges();

                }
                catch
                {
                    Console.WriteLine("\n~Movie Does Not Exist~\n");
                    logger.Error("DeleteMovie() could not Delete Movie");
                }
            }
        }

        public void UpdateMovie(int Id)
        {
            
        }

        public void AddNewUser(User user, Occupation occupation)
        {
            using(var db = new MovieContext())
            {
                try
                {
                    db.Add<User>(user);
                    db.Attach(occupation);
                    db.SaveChanges();
                    Console.WriteLine("\n~User Created!~\n");
                    Console.WriteLine("\nYour User ID is: " + user.Id + "\n\n");
                }
                catch
                {
                    logger.Error("AddNewUser() could Not Add New User");
                    Console.WriteLine("One or more Parameters are not Correct. \nTry again.");
                }
            }
        }

        public void DisplayUserInfo(int userId)
        {
            using(var db = new MovieContext())
            {
                try
                {
                    var userRatings = from a in db.UserMovies
                                      join b in db.Movies
                                      on a.Movie.Id equals b.Id
                                      where a.User.Id == userId
                                      select new { a.Rating, a.Movie.Title, a.RatedAt };
                    var checkNull = userRatings.ToList();
                    if(checkNull.Count() != 0)
                    {
                        Console.WriteLine("\n~User " + userId + "'s Rated Movies~\n");
                        foreach (var movieTitle in userRatings)
                        {
                            Console.WriteLine("Title: " + movieTitle.Title + " Rating: (" + movieTitle.Rating + ") Rated At: " + movieTitle.RatedAt);  
                        }
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Could Not Display User Info; Please Enter Valid User ID");
                        logger.Error("DisplayUserInfo() incorrect User ID");
                    }
                }
                catch
                {
                    Console.WriteLine("Could Not Display User Info");
                    logger.Error("DisplayUserInfo() error");
                }
            }
        }

        public void AddMovieRating(User user, Movie movie, int rating)
        {
            DateTime date = DateTime.Now;
            UserMovie userMovie = new UserMovie()
            {
                User = user,
                Movie = movie,
                Rating = rating,
                RatedAt = date
            };
            
            using(var db = new MovieContext())
            {
                try
                {
                    db.Add<UserMovie>(userMovie);
                    db.Attach(user);
                    db.Attach(movie);
                    db.SaveChanges();
                    Console.WriteLine("Rating Added!");
                    Console.WriteLine("User ID: " + user.Id);
                    Console.WriteLine("Movie Title: " + movie.Title);
                    Console.WriteLine("Rating: (" + rating + "/5)" + " Rated At: " + date);
                    Console.WriteLine();
                }
                catch
                {
                    Console.WriteLine("Could Not Add Rating; Please Make sure all values are correct");
                    logger.Error("AddMovieRating() not fulfilled.");
                }
            }
        }

        public void ListTopRatedAge(int i, int j)
        {
            using(var db = new MovieContext())
            {
                try
                {
                    var top = (from m in db.Movies
                               join um in db.UserMovies
                               on m.Id equals um.Movie.Id
                               join us in db.Users
                               on um.User.Id equals us.Id
                               where us.Age > i && us.Age < j
                               group new { m, um, us } by new { m.Id, m.Title } into newGroup
                               orderby newGroup.Average(x => x.um.Rating) descending
                               select new
                               {
                                   Key = newGroup.Key.Id,
                                   Title = newGroup.Key.Title,
                                   Rating = newGroup.Average(x => x.um.Rating)
                               }).Take(1);

                    foreach (var thing in top)
                    {
                        Console.WriteLine("\n~Age Group (" + i + "-" + j + ")~");
                        Console.WriteLine("Movie ID: " + thing.Key + " Movie Title: " + thing.Title + "Average Rating: " + String.Format("{0:0.00}", thing.Rating));
                    }
                }
                catch
                {
                    Console.WriteLine("ListTopRatedAge() Error.");
                    logger.Error("ListTopRatedAge() Error.");
                }
                
            }
        }

        public void ListTopRatedOccupation(long occupationId, string occupation)
        {
            
            using (var db = new MovieContext())
            {
                try
                {
                    var top = (from m in db.Movies
                               join um in db.UserMovies
                               on m.Id equals um.Movie.Id
                               join us in db.Users
                               on um.User.Id equals us.Id
                               join o in db.Occupations on us.Occupation.Id equals o.Id
                               where o.Id == occupationId
                               group new { m, um, us } by new { m.Id, m.Title } into newGroup
                               orderby newGroup.Average(x => x.um.Rating) descending
                               select new
                               {
                                   Key = newGroup.Key.Id,
                                   Title = newGroup.Key.Title,
                                   Rating = newGroup.Average(x => x.um.Rating)
                               }).Take(1);
                    foreach (var thing in top)
                    {
                        Console.WriteLine("\n~Occupation : " + occupation + "~");
                        Console.WriteLine("Movie ID: " + thing.Key + " Movie Title: " + thing.Title + "Average Rating: " + String.Format("{0:0.00}", thing.Rating));
                    }
                }
                catch
                {
                    Console.WriteLine("ListTopRatedOccupation() Error.");
                    logger.Error("ListTopRatedOccupation() Error.");
                }
            }
        }
    }
}

