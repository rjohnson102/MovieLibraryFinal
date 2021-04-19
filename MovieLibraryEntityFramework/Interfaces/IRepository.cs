using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibraryEntityFramework
{
    public interface IRepository
    {
            public void ListAllMovies();
            public void SearchMovie(string name);

            public void AddMovie(Movie movie);

            public void UpdateMovie(int Id);

            public void DeleteMovie(int Id);
    }
}
