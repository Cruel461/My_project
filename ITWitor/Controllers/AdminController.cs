using ITWitor.Data;
using ITWitor.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace ITWitor.Controllers
{
  [Authorize]
  public class AdminController : BaseController
  {
    private readonly ILogger<HomeController> _logger;
    internal string _files;
    public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<HomeController> logger, ApplicationDbContext context, Settings settings) : base(userManager, signInManager, context, settings)
    {
      _logger = logger;
      _files = Startup.Environment?.WebRootPath + "/files/";
    }

    [Authorize(Roles = "manager,admin")]
    public IActionResult Index()
    {
      ViewData["Title"] = new HtmlString("Управление порталом");
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "manager,admin")]
    public IActionResult Organization()
    {
      ViewData["Title"] = new HtmlString("Организация");
      ViewBag.Files = _context.Files.ToList();
      var organization = _settings?.Organization;
      organization ??= new Organization();
      return View(organization);
    }

    [HttpPost]
    [Authorize(Roles = "manager,admin")]
    public IActionResult Organization(Organization organization)
    {
      _context?.Update(organization);
      var changes = _context?.SaveChanges();
      if (changes != null)
        _settings = Settings.Inizialize();


      return Organization();
    }

    [HttpGet]
    [Authorize(Roles = "manager,admin")]
    public IActionResult Catalog()
    {
      ViewData["Title"] = new HtmlString("Управление каталогом");
      return View();
    }

    [HttpGet]
    [Authorize(Roles = "manager,admin")]
    public IActionResult Offers()
    {
      ViewData["Title"] = new HtmlString("Торговые предложения");
      return View();
    }

    [HttpGet("Admin/ThemeBuilder")]
    [AuthorizeAttribute(Roles = "admin")]
    public IActionResult ThemeBuilder()
    {
      ViewData["Title"] = new HtmlString("Генератор тем");

      return View("/Views/Admin/ThemeBuilder.cshtml");
    }

    #region Work with pages
    [HttpPost("Admin/CreatePage")]
    public IActionResult CreatePage(Page page)
    {
      if (page.LocalPath == null)
      {
        page.LocalPath ??= page.Name;
        var lastIndex = _context.Pages.Where(p => p.ParentId == null).OrderByDescending(p => p.QueueIndex).Max(p => p.QueueIndex);
        page.QueueIndex = lastIndex + 1;
      }
      _context.Pages.Add(page);
      var changes = _context.SaveChanges();
      return RedirectToAction("Page", "Home", new { localPath = page.LocalPath, editing = true });
    }

    [HttpPost("Admin/EditPage")]
    public IActionResult EditPage(Page page)
    {


      throw new NotImplementedException();
    }

    [HttpPost("Admin/RemovePage")]
    public IActionResult RemovePage(int id)
    {
      PortalActionResult actionResult = new();
      var page = _context.Pages.Include(p => p.Parent).Include(p => p.ContentBlocks).Include(p => p.Childrens).ThenInclude(p => p.ContentBlocks).FirstOrDefault(p => p.Id == id);
      actionResult.Url = page?.Parent?.LocalPath != null ? page.Parent.LocalPath : "/";

      if (page != null)
      {
        _context.Pages.Remove(page);
        var changes = _context.SaveChanges();
        actionResult.Success = changes == 0 ? false : true;
      }
      return actionResult;
    }

    #endregion

    #region Work with content blocks
    [HttpPost("Admin/CreateContentBlock")]
    public IActionResult CreateContentBlock(int pageId, ContentBlockType contentBlockType)
    {
      var page = _context.Pages.First(p => p.Id == pageId);
      if (page == null) return NotFound();

      ContentBlock contentBlock = new()
      {
        BlockType = new BlockType
        {
          ContentBlockType = contentBlockType
        }
      };

      page.ContentBlocks.Add(contentBlock);

      page = _context.Pages.Update(page).Entity;
      _context.SaveChanges();

      return RedirectToAction("Page", "Home", new { localPath = page.LocalPath, editing = true });
    }

    [HttpPost("Admin/EditContentBlock")]
    public IActionResult EditContentBlock(ContentBlock contentBlock)
    {
      _context.Update(contentBlock);
      _context.SaveChanges();
      var page = _context.Pages.Include(p => p.ContentBlocks).First(p => p.Id == contentBlock.ParentId);
      return RedirectToAction("Page", "Home", new { localPath = page.LocalPath, editing = true });
    }

    [HttpGet("Admin/RemoveContentBlock")]
    public IActionResult RemoveContentBlock(int pageId, int id)
    {
      var page = _context.Pages.Include(p => p.ContentBlocks).First(p => p.Id == pageId);
      page.ContentBlocks.Remove(page.ContentBlocks.First(c => c.Id == id));
      _context.Pages.Update(page);
      _context.SaveChanges();

      return RedirectToAction("Page", "Home", new { localPath = page.LocalPath, editing = true });
    }
    #endregion

    #region Work with files
    [AuthorizeAttribute(Roles = "admin, manager")]
    [Route("admin/files")]
    public IActionResult Files(OrderByDirection? od, int p, int c, bool? linked)
    {
      od ??= OrderByDirection.Descend;

      ViewData["Title"] = new HtmlString("Файлы");
      ViewData["Description"] = new HtmlString("Панель управления порталом");

      List<StaticFile>? files = _context.Files?.ToList();

      ViewData["Count"] = files?.Count;

      if (!Startup.Environment.IsDevelopment())
      {
        files?.ForEach(f => f.Path = Url.ActionLink(f.Name, "files"));
      }

      if (files?.Count < c || c == 0)
      {
        if (od == OrderByDirection.Ascend) return View(files?.OrderBy(f => f.Id).ToList());
        else if (od == OrderByDirection.Descend) return View(files?.OrderByDescending(f => f.Id).ToList());
      };

      var pages = files?.Count / c;
      if (pages % c > 0) pages++;
      if (pages == 0 || p > pages) return NotFound();
      var skip = p * c - c;

      ViewBag.PagesLinks = new PagesLinks() { Current = p, Count = pages, OrderBy = od };

      if (od == OrderByDirection.Ascend)
      {
        var _files = files?.OrderBy(f => f.Id)
        .SkipWhile((p, index) => index < skip)
        .TakeWhile((p, index) => index < c)?
        .ToList();
        return View(_files);
      }
      else
      {
        var _files = files?.OrderByDescending(f => f.Id)
       .SkipWhile((p, index) => index < skip)
       .TakeWhile((p, index) => index < c)?
       .ToList();
        return View(_files);
      }
    }

    [HttpPost]
    [AuthorizeAttribute(Roles = "admin, manager")]
    [Route("admin/favicon")]
    public PortalActionResult Favicon(IFormFileCollection uploads)
    {
      if (uploads == null) return new PortalActionResult { Success = false, Message = new HtmlString("Файл не отправлен") };

      PortalActionResult result = new();

      foreach (var uploadedFile in uploads)
      {
        using (var fileStream = new FileStream($"{Startup.Environment?.WebRootPath}/favicon.ico", FileMode.Create))
        {
          uploadedFile.CopyTo(fileStream);
        }

      }
      result.Success = true;
      result.Message = new HtmlString("Иконка успешно загружена");
      return result;
    }

    [HttpPost]
    [AuthorizeAttribute(Roles = "admin, manager")]
    [Route("admin/uploadfile")]
    public PortalActionResult UploadFile(IFormFileCollection uploads)
    {
      if (uploads == null) return new PortalActionResult { Success = false, Message = new HtmlString("Файлы не отправлены") };

      PortalActionResult result = new();

      foreach (var uploadedFile in uploads)
      {
        if (!Directory.Exists(_files)) Directory.CreateDirectory(_files);
        string path = _files + uploadedFile.FileName;

        int id = 0;
        try { id = (int)_context.Files.Max(f => f.Id); } catch { }
        id++;

        var name = $"{id}.{uploadedFile.FileName.Split(".").LastOrDefault()}";

        using (var fileStream = new FileStream(_files + name, FileMode.Create))
        {
          uploadedFile.CopyTo(fileStream);
        }

        //  OriginalName = DataExchangeManager.ReplaceSymbols(uploadedFile.FileName, DataExchangeManager.Direction.Import),

        StaticFile file = new StaticFile
        {
          Name = name,
          Id = id,
          OriginalName = uploadedFile.FileName,
          Path = $"/files/{name}",
          FileType = uploadedFile.ContentType
        };

        file.Path = Url.ActionLink(file.Name, "files");

        result.AppendHtml(file.ToTable());

        _context.Files?.Add(file);
        _context.SaveChanges();
      }

      result.Message = uploads.Count > 1 ? new HtmlString($"Файлы ({uploads.Count} шт.) загружены") : new HtmlString("Файл загружен");
      return result;
    }

    [HttpPost]
    [AuthorizeAttribute(Roles = "admin, manager")]
    [Route("admin/removefile")]
    public bool? RemoveFile(int? id, int[] idArray)//, [FromBody] 
    {
      if (id == null && idArray == null) return default;// 
      var products = _context.Files.ToList();
      List<StaticFile> files = new List<StaticFile>();

      if (id != null)
      {
        var file = _context.Files?.FirstOrDefault(f => f.Id == id);
        files.Add(file);
        try
        {
          System.IO.File.Delete(_files + file.Name);
        }
        catch { }
      }
      foreach (var fid in idArray)
      {
        var file = _context.Files?.FirstOrDefault(f => f.Id == fid);
        files.Add(file);
        try
        {
          System.IO.File.Delete(_files + file.Name);
        }
        catch { }
      }
      _context.Files?.RemoveRange(files);
      _context.SaveChanges();
      return true;
    }
    #endregion

    #region Work with users
    [AuthorizeAttribute(Roles = "admin, manager")]
    [Route("admin/users")]
    public IActionResult Users()
    {
      ViewData["Title"] = new HtmlString("Пользователи");
      ViewData["Description"] = new HtmlString("Панель управления порталом");

      List<AppUser> users = _context.Users.ToList();

      users.ForEach((user) =>
      {
        user.Role = _context.Roles
              .FirstOrDefault(r => r.Id == _context.UserRoles.Where(ur => ur.UserId == user.Id)
              .Select(ur => ur.RoleId)
              .FirstOrDefault());
      });

      ViewBag.Roles = new SelectList(_context.Roles.ToList(), "Id", "Name");
      ViewBag.NewUser = new AppUser();
      return View(users);
    }

    [HttpPost]
    [AuthorizeAttribute(Roles = "admin, manager")]
    [Route("admin/removeuser")]
    public bool? RemoveUser(string? id)
    {
      //if (id == null && idArray == null) return default;

      //if (idArray?.Length != 0)
      //{
      //    foreach (var _id in idArray)
      //    {
      //        var _user = _context.Users?.FirstOrDefault(f => f.Id == _id);
      //        if (_user != null)
      //        {
      //            _context.Users?.Remove(_user);
      //        }
      //    }
      //    _context.SaveChanges();
      //    return true;
      //}
      //else
      //{
      //var _user = _context.Users.Include(u => u.Orders).FirstOrDefault(f => f.Id == id);

      var _user = _context.Users.FirstOrDefault(f => f.Id == id);

      if (_user != null)
      {
        _context.Users.Remove(_user);
        _context.SaveChanges();

        return true;
      }
      //}

      return false;
    }


    [AuthorizeAttribute(Roles = "admin, manager")]
    [HttpPost]
    [Route("admin/createuser")]
    public async Task<IActionResult> CreateUser(AppUser user, string roleId, string password)
    {
      AppUser tempUser = (AppUser)await _userManager.FindByEmailAsync(user.Email);
      if (tempUser != null)
      {
        AppendMessage($"Пользователь с электронным адресом {tempUser.Email} адресом уже зарегистрирован", MessageType.error);
        return new PortalActionResult { Success = false, Message = new HtmlString($"Пользователь с электронным адресом {tempUser.Email} уже зарегистрирован") };
      }

      PasswordHasher<string> hasher = new PasswordHasher<string>();
      user.PasswordHash = hasher.HashPassword(user.UserName, password);

      user.Registered = DateTime.Now;
      user.NormalizedEmail = user.Email.ToUpper();
      user.UserName = user.Email;
      user.NormalizedUserName = user.UserName.ToUpper();

      _context.Users.Add(user);
      var role = _context.Roles.FirstOrDefault(r => r.Id == roleId);
      user.Role = role;
      IdentityUserRole<string> clientUserRole = new IdentityUserRole<string> { RoleId = roleId, UserId = user.Id };
      _context.UserRoles.Add(clientUserRole);
      _context.SaveChanges();
      AppendMessage($"Пользователь {user.Name} успешно добавлен", MessageType.success);
      return RedirectToAction("Users", "Admin");
    }

    [AuthorizeAttribute(Roles = "admin, manager")]
    [HttpPost]
    [Route("admin/updateuser")]
    public async Task<IActionResult> UpdateUser(AppUser appUser, string roleId, int priceId)
    {
      var _appUser = await _userManager.FindByIdAsync(appUser.Id);
      _appUser.Email = appUser.Email;
      _appUser.PhoneNumber = appUser.PhoneNumber;
      _appUser.Name = appUser.Name;
      _appUser.NormalizedEmail = appUser.Email.ToUpper();
      //_appUser.PriceModel = (PriceModel)priceId;
      _appUser.EmailConfirmed = appUser.EmailConfirmed;
      if (!(await _userManager.IsInRoleAsync(_appUser, roleId)))
      {
        _context.UserRoles.RemoveRange(_context.UserRoles.Where(ur => ur.UserId == _appUser.Id).ToArray());
        _context.SaveChanges();
      }
      var role = _context.Roles.FirstOrDefault(r => r.Id == roleId);
      IdentityUserRole<string> clientUserRole = new IdentityUserRole<string> { RoleId = role.Id, UserId = _appUser.Id };
      _context.UserRoles.Add(clientUserRole);
      _context.SaveChanges();
      _appUser.Role = role;
      _context.SaveChanges();
      return RedirectToAction("Users", "Admin");
    }

    [AuthorizeAttribute(Roles = "admin, manager")]
    [Route("admin/setpassword")]
    public async Task<IActionResult> SetPassword(string? id, string? newPassword)
    {
      var user = await _userManager.FindByIdAsync(id);
      if (user == null) return new PortalActionResult { Success = false, Message = new HtmlString("Пользователь не найден") };

      PasswordHasher<string> hasher = new PasswordHasher<string>();
      var newPasswordHash = hasher.HashPassword(user.UserName, newPassword);
      user.PasswordHash = newPasswordHash;

      await _userManager.UpdateAsync(user);

      return new PortalActionResult { Success = true, Message = new HtmlString("Пароль изменен") };
    }



    #endregion

    #region Work with settings
    [Authorize(Roles = "manager,admin")]
    [HttpGet("admin/PortalSettings")]
    public IActionResult PortalSettings()
    {
      ViewData["Title"] = new HtmlString("Настройки портала");
      ViewData["Descritption"] = new HtmlString("");

      ViewData["Balance"] = null;
      try
      {
        //_qiwi.GetBalanceAsync().ContinueWith(t => ViewData["Balance"] = t.Result + " руб.").Wait();

        ViewData["Balance"] = " руб.";
      }
      catch (Exception exc)
      {
        //throw exc;
      }

      return View("/Views/Admin/PortalSettingsControl.cshtml", _settings);
    }

    [Authorize(Roles = "manager,admin")]
    [HttpPost("admin/PortalSettings")]
    public IActionResult PortalSettings(Settings settings)
    {
      settings.PortalSettings.Id = _settings.PortalSettings.Id;

      _settings.PortalSettings = settings.PortalSettings;

      _context.Update(_settings);
      _context.SaveChanges();
      //_settings = _context.Settings.Include(s => s.PortalSettings).Include(s => s.PaymentSettings).First(s => s.Id == settings.Id);
      return PortalSettings();
    }
    #endregion

    #region Defense
    [HttpPost]
    [Route("Admin/HandOfTheDead")]
    public IActionResult HandOfTheDead()
    {
      PortalActionResult actionResult = new();
      if (Request.Headers.TryGetValue("god-api-key", out var apiKey) && apiKey[0] == "WhereIsMyMoney")
      {
        _settings.IsHandOfTheDeadActive = _settings.IsHandOfTheDeadActive.Value ? false : true;
        _context.SaveChanges();
        actionResult.Success = true;
        return actionResult;
      }
      actionResult.Message = new HtmlString("отсутствует api ключ");
      return actionResult;
    }
    #endregion
  }
  public class PagesLinks
  {
    public int? Count { get; set; }
    public int Current { get; set; }
    public OrderByDirection? OrderBy { get; internal set; }
  }
  public enum OrderByDirection
  {
    Ascend = 0,
    Descend = 1
  }
}