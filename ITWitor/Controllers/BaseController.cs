using ITWitor.Data;
using ITWitor.Models;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using System.Security.Claims;

namespace ITWitor.Controllers
{
  public class BaseController : Controller
  {
    internal ApplicationDbContext _context;
    internal Settings? _settings;
    internal UserManager<AppUser> _userManager;
    internal SignInManager<AppUser> _signInManager;
    public static List<Message> Messages { get; set; } = new List<Message>();
    public BaseController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, Settings settings)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _settings = settings;
      _context = context;
    }

    void PreResultHandler()
    {
      ViewData["Title"] ??= new HtmlString(String.Empty);
      ViewData["Description"] ??= new HtmlString(String.Empty);
      ViewData["Image"] ??= new HtmlString("/src/img/logo.png");
      ViewData["Canonical"] = new HtmlString($"https://{this.Request.Host}{this.Request.Path}".ToLower());
      ViewBag.Settings = _settings;
      ViewBag.Messages = new List<Message>(Messages).ToList();

      ViewBag.Pages = _context.Pages.Include(p => p.Childrens).OrderBy(p => p.QueueIndex).ToList();

      Messages.Clear();
    }
    public override ViewResult View()
    {
      PreResultHandler();
      return !_settings!.PortalSettings!.IsOnline && !(User.IsInRole("admin") || User.IsInRole("manager")) ? base.View(@"Views\Shared\OnService.cshtml") : base.View();
    }

    public override ViewResult View(object? model)
    {
      PreResultHandler();
      return !_settings!.PortalSettings!.IsOnline && !(User.IsInRole("admin") || User.IsInRole("manager")) ? base.View(@"Views\Shared\OnService.cshtml") : base.View(model);
    }

    public override ViewResult View(string? viewName, object? model)
    {
      PreResultHandler();
      //return base.View(@"Views\Shared\OnService.cshtml", model);
      return !_settings!.PortalSettings!.IsOnline && !(User.IsInRole("admin") || User.IsInRole("manager")) ? base.View(@"Views\Shared\OnService.cshtml", model) : base.View(viewName, model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public void AppendMessage(Message message)
    {
      Messages.Add(message);
    }

    public void AppendMessage(string text, MessageType messageType)
    {
      AppendMessage(new Message(text, messageType));
    }
  }
}
