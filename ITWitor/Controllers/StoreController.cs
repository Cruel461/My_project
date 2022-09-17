using ITWitor.Data;
using ITWitor.Models;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace ITWitor.Controllers
{
    public class StoreController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public StoreController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<HomeController> logger, ApplicationDbContext context, Settings settings) : base(userManager, signInManager, context, settings)
        {
            _logger = logger; 
        }

        public IActionResult Catalog()
        {
            ViewData["Title"] = new HtmlString("Каталог");
            ViewData["Description"] = new HtmlString("Каталог спецодежды: широкий ассортимент рабочей одежды, спецобуви, СИЗ и пр.");
            return View();
        }
    }
}