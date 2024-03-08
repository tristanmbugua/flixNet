using Microsoft.AspNetCore.Mvc;

namespace _301141338_Mbugua__LabThree.Controllers
{
    public class RateMovieController : Controller
    {
        public IActionResult Index()
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.initializeMovies();
            ViewBag.movieTitle = Movie.movieTitles;
            return View();
        }
    }
    public class RateController : Controller
    {
        public IActionResult Index(
                string movieTitle, int rating
            )
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.findMovie(movieTitle).addRating(rating);
            return View();
        }
    }
}
