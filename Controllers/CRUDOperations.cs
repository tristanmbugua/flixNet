using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace _301141338_Mbugua__LabThree.Controllers
{
    public class DownloadController : Controller
    {
        static AWS_Driver driver = new AWS_Driver();
        public IActionResult Index(string movieName)
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }
            driver.pullMovie(movieName);
            return View();
        }
    }

    public class UploadController : Controller
    {
        static AWS_Driver driver = new AWS_Driver();
        public IActionResult Index(string filename, string movieDirector, string genre)
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }
            driver.putMovie(filename, filename.Substring(0, filename.IndexOf(".")), movieDirector, genre);
            return View();
        }
    }

    public class DeleteController : Controller
    {
        static AWS_Driver driver = new AWS_Driver();
        public IActionResult Index(string movieName)
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }
            driver.deleteMovie(movieName);
            return View();
        }
    }

    public class UpdateController : Controller
    {
        static AWS_Driver driver = new AWS_Driver();
        public IActionResult Index(string movieTitle, string movieDirector, string genre, string releaseTime)
        {
            if (!UserAccountDriver.validUser)
            {
                return View("AuthenticationError");
            }
            driver.updateMovie(Movie.findMovie(movieTitle), movieDirector, genre, releaseTime);
            return View();

        }
    }
}