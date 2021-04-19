using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLibraryEntityFramework
{
    public class MovieGenre
    {
        public int Id { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
