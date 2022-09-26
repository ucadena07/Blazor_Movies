using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Required field")]
        public string Name { get; set; }
        public List<MoviesGenre> MoviesGenre { get; set; } = new();
    }
}
