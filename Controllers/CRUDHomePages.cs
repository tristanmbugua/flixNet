using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _301141338_Mbugua__LabThree.Controllers
{
    public class DownloadMovieController : Controller
    {
        
        public IActionResult Index()
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.initializeMovies();

            List<SelectListItem> movieList = new List<SelectListItem>();

            for (int i = 0; i < Movie.movies.Count; i++)
            {
                movieList.Add(new SelectListItem { Text = Movie.movies.ElementAt(i).ToString(), Value = i.ToString() });
            }
            
            ViewBag.movieList = Movie.movies;
            ViewBag.movieName = Movie.movieTitles;

            return View();
        }
    }
    
    public class UploadMovieController : Controller
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

    public class DeleteMovieController : Controller
    {
        public IActionResult Index()
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.initializeMovies();
            ViewBag.movieName = Movie.movieTitles;
            return View();
        }
    }

    public class UpdateMovieController : Controller
    {
        public IActionResult Index()
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.initializeMovies();
            ViewBag.movieList = Movie.movies;
            ViewBag.movieName = Movie.movieTitles;
            return View();
        }
    }
    
    public class UpdateFormController : Controller
    {
        public IActionResult Index(string movieName)
        {
            Movie.initializeMovies();
            Movie changeableMovie = Movie.findMovie(movieName);
            IEnumerable<SelectListItem> genres = Movie.genres;

            ViewBag.movie = changeableMovie; 
            ViewBag.genre = genres;
            return View();
        }
    }

}