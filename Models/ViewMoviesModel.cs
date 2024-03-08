using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace _301141338_Mbugua__LabThree.Models
{
    public class ViewMoviesModel : PageModel
    {
        public List<Movie> movies = Movie.movies;
    }
}
