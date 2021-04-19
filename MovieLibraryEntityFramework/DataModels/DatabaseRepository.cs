using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieLibraryEntityFramework
{
    class DatabaseRepository : IRepository
    {
        public void ListAllMovies()
        {
            using (var db = new MovieContext())
            {
                var movies = db.Movies.ToList();
                Console.WriteLine();
                foreach (var movie in movies)
                {
                    Console.WriteLine(movie.Id + ". " + movie.Title + "\tRelease Date: " + movie.release_date);
                }
                Console.WriteLine();
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
                }
            }
        }
        public void AddMovie(Movie movie)
        {
            using(var db =new MovieContext())
            {
                db.Add<Movie>(movie);
                db.SaveChanges();
            }
            Console.WriteLine("\nMovie Added!: " + movie.Id + ". " + movie.Title+"\n");
        }

        public void DeleteMovie(int Id)
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
                }
            }
        }

        public void UpdateMovie(int Id)
        {
            
        }
    }
}

