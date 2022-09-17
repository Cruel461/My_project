using ITWitor.Data;
using ITWitor.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ITWitor.Controllers
{
  public class HomeController : BaseController
  {
    private readonly ILogger<HomeController> _logger;

    public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<HomeController> logger, ApplicationDbContext context, Settings settings) : base(userManager, signInManager, context, settings)
    {
      _logger = logger;
    }

    [HttpGet]
    public IActionResult Index(bool? editing)
    {
      return View((Page)ViewBag.Page);
    }

    [HttpPost]
    [Authorize(Roles = "manager,admin")]
    public IActionResult Index(Page page, bool? editing)
    {
      page = _context.Pages.Update(page).Entity;
      _context.SaveChanges();
      return Index(editing);
    }

    [HttpGet("{localPath?}")]
    public IActionResult Page(string localPath, bool? editing, bool godMode = false)
    {
      localPath ??= "/";

      var page = _context.Pages
        .Include(p => p.ContentBlocks)
        .ThenInclude(cb => cb.BlockType)
        .FirstOrDefault(page => page.LocalPath == localPath);

      if (localPath == "/" && page == null)
      {
        page = new Page() { LocalPath = localPath, Name = "/", QueueIndex = 1, Title = "Главная" };
        _context.Pages.Add(page);
        _context.SaveChanges();
      }

      if (_context.Pages.Count() == 0 && !User.IsInRole("admin"))
        return View(@"Views\Shared\OnService.cshtml");
      else if (_context.Pages.Count() == 0 && User.IsInRole("admin"))
      {
        page = new Page() { LocalPath = localPath };
        _context.Pages.Add(page);
        _context.SaveChanges();
      }

      if (page == null && User.IsInRole("admin") && godMode)
      {
        page = new()
        {
          LocalPath = localPath,
        };
        _context.Pages.Add(page);
        _context.SaveChanges();
        editing = true;
      }

      if (editing.HasValue && editing.Value)
      {
        ViewBag.BlockTypes = Enum.GetNames(typeof(ContentBlockType)).Where(e => e.ToLower() != "unknown").ToList();
      }

      if (page == null) return NotFound();

      ViewBag.Page = page;
      ViewData["Title"] = new HtmlString(page.Title);
      ViewData["Description"] = new HtmlString(page.Description);



      return View("/Views/Shared/Generic.cshtml", (Page)ViewBag.Page);
    }

    [HttpPost("{localPath}")]
    [Authorize(Roles = "manager,admin")]
    public IActionResult Page(Page page, bool? editing, bool godMode = false)
    {
      page = _context.Pages.Update(page).Entity;
      _context.SaveChanges();
      return Page(page.LocalPath!, editing, godMode);
    }

    [HttpPost]
    [Route("Login/returnUrl")]
    public IActionResult LogIn(string email, string password, string? returnUrl)
    {
      Task.Run(async () =>
      {
        var user = await _userManager.FindByEmailAsync(email.ToLower());
        if (user == null)
          AppendMessage(new Message("Неверный логин", MessageType.error));
        else
                  if ((await _signInManager.PasswordSignInAsync(user, password, true, true)).Succeeded)
          AppendMessage(new Message("Успешная авторизация", MessageType.success));
        else
          AppendMessage(new Message("Неверный пароль", MessageType.error));


      }).ContinueWith(t => t).Wait();

      if (returnUrl != null)
        returnUrl = new Regex(@"(?<=https:\/\/.+?)\/.+").Match(returnUrl).Value;

      if (!String.IsNullOrEmpty(returnUrl)) return LocalRedirect(returnUrl);
      else return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("Logout/returnUrl")]
    public async Task<IActionResult> LogOut(string returnUrl)
    {
      await _signInManager.SignOutAsync();
      return Redirect(returnUrl);
    }
  }
}