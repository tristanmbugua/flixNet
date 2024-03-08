using Microsoft.AspNetCore.Mvc;

namespace _301141338_Mbugua__LabThree.Controllers
{
    public class SignInController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
    public class LogInController : Controller
    {
        public IActionResult Index(String username, String password)
        {
            UserAccountDriver.username = username;
            UserAccountDriver.password = password;

            SQL_ServerDriver driver = new SQL_ServerDriver();
            UserAccountDriver.validUser = driver.authenticateUser();

            if (UserAccountDriver.validUser)
            {
                return View("LogInSuccess");
            }
            return View("LogInFailure");
        }
    }
    public class RegisterController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
    public class RegisterUserController : Controller
    {
        public IActionResult Index(String username, String password)
        {
            SQL_ServerDriver driver = new SQL_ServerDriver();
            driver.addUser(username, password);
            return View();
        }
    }
}