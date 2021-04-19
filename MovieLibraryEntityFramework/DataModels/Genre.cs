using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibraryEntityFramework
{
    public class Genre
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
