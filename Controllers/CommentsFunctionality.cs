using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace _301141338_Mbugua__LabThree.Controllers
{
    public class ReadCommentsController : Controller
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
    
    public class EditCommentsController : Controller
    {
        public IActionResult Index(string movieName)
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.initializeMovies();
            Movie movie = Movie.findMovie(movieName);
            ViewBag.movieName = movieName;
            ViewBag.comments = movie.unpackedComments;
            ViewBag.username = UserAccountDriver.username;
            return View();
        }
    }
    
    public class UpdateCommentsController : Controller
    {
        public IActionResult Index(
                string movieName, string comment0, string author0, string time0,
                string comment1, string author1, string time1,
                string comment2, string author2, string time2,
                string comment3, string author3, string time3,
                string comment4, string author4, string time4
            )
        {

            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.findMovie(movieName).editComments(
                comment0, author0, time0,
                comment1, author1, time1,
                comment2, author2, time2,
                comment3, author3, time3,
                comment4, author4, time4
                );
            return View();
        }
    }
    
    public class AddCommentsController : Controller
    {
        public IActionResult Index()
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.initializeMovies();
            ViewBag.movieTitle = Movie.movieTitles;
            ViewBag.username = UserAccountDriver.username;
            ViewBag.time = DateTime.Now.ToString();
            return View();
        }
    }

    public class AddCommentController : Controller
    {
        public IActionResult Index(
            string movieTitle, string comment, 
            string author, string time)
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }

            Movie.findMovie(movieTitle).addComment(comment, author, time);
            return View();
        }
    }
}