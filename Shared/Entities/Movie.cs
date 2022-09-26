using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorMovies.Shared.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime? ReleaseDate { get; set; }
        public string Poster { get; set; }
        public string TitleBrief 
        {
            get
            {
                if (string.IsNullOrEmpty(Title))
                {
                    return null;
                }
                if(Title.Length > 60)
                {
                    return Title.Substring(0,60) + "...";
                }
                else
                {
                    return Title;
                }
            }
            
        }
        public string Summary { get; set; }
        public bool InTheathers { get; set; }
        public string Trailer { get; set; }
        public List<MoviesGenre> MoviesGenre { get; set; } = new();
    }
}
