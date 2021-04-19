using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MovieLibraryEntityFramework
{
    public class Movie
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime release_date { get; set; }
        
        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
    }
}
