using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Movies.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<Movie> Movies;

        [BindProperty]
        public string search { get; set; }

        [BindProperty]
        public List<string> mpaa { get; set; } = new List<string>();

        [BindProperty]
        public float? minIMDB { get; set; }

        [BindProperty]
        public float? maxIMDB { get; set; }

        [BindProperty]
        public string sort { get; set; }



        public void OnGet()
        {
            Movies = MovieDatabase.All.OrderBy(movie => movie.Title);
        }

        public void OnPost()
        {
            Movies = MovieDatabase.All;

            if (search != null)
            {
                Movies = Movies.Where(movie => movie.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                || movie.Director != null && movie.Director.Contains(search, StringComparison.OrdinalIgnoreCase));
                //Movies = MovieDatabase.Search(Movies, search);
            }

            if(mpaa.Count != 0)
            {
                Movies = Movies.Where(movie => mpaa.Contains(movie.MPAA_Rating));
                //Movies = MovieDatabase.FilterByMPAA(Movies, mpaa);
            }

            if(minIMDB != null)
            {
                Movies = Movies.Where(movie => movie.IMDB_Rating != null && movie.IMDB_Rating >= minIMDB);
                //Movies = MovieDatabase.FilterByMinIMDB(Movies, (float)minIMDB);
            }

            if (maxIMDB != null)
            {
                Movies = Movies
                    .Where(movie => movie.IMDB_Rating != null)
                    .Where(movie => movie.IMDB_Rating <= maxIMDB);
            }

            if (sort != null)
            {
                Movies = Movies.OrderBy<Movie, string>(movie =>
                {
                    switch (sort)
                    {
                        case "title":
                            return movie.Title;
                        case "director":
                            if (movie.Director == null) return "";
                            string[] parts = movie.Director.Split(" ");
                            return parts[parts.Length -1];
                        case "year":
                            return movie.Release_Year;
                        case "imdb":
                            return movie.IMDB_Rating.ToString();
                        case "rt":
                            return movie.Rotten_Tomatoes_Rating.ToString();
                        default:
                            return movie.Title;
                    }
                });
            }
        }
    }
}
