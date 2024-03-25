using Businesslayer;
using Login_Outsource_project.Models;
using LoginDAL;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace Login_Outsource_project.Controllers
{
    public class HomeController : Controller
    {
        UserContainer ucon = new UserContainer(new UserRepository());

        public IActionResult Index() //normale index pagina
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public IActionResult Index(LoginViewModel loginViewModel) //index als die een post request krijgt...als de methode er in goed wordt uitgevoerd of te wel email/wachtwoord kloppen ga je naar de private pagina
        {
            User User = new User()
            {
                Email= loginViewModel.Email,
                Password = loginViewModel.Password,
            };

            if(ucon.AuthenticateUser(User))
            {
                return RedirectToAction("Privacy", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(loginViewModel);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}