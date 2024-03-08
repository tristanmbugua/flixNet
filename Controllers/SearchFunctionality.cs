using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _301141338_Mbugua__LabThree.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            IEnumerable<SelectListItem> genres = Movie.genres;
            ViewBag.genre = genres;
            return View();
        }
    }
    public class SearchGenreController : Controller
    {
        public IActionResult Index(string genre)
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }
            
            ViewBag.movieList = Movie.searchByGenre(genre);
            return View();
        }
    }
    public class SearchRatingController : Controller
    {
        public IActionResult Index(string min, string max)
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }
            
            ViewBag.movieList = Movie.searchByRating(Convert.ToInt32(min), Convert.ToInt32(max));
            return View();
        }
    }
}
