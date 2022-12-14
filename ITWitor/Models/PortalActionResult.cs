using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;

namespace ITWitor.Models
{
  public class PortalActionResult : IActionResult
  {
    public HtmlString? Message { get; set; }
    public bool Success { get; set; }
    public string? Html { get; set; }
    public string? Json { get; set; }
    public int? Code { get; set; }
    public string? Url { get; set; }
    public string? Data { get; set; }

    public PortalActionResult() { }

    public PortalActionResult(bool result, string message, string html = "")
    {
      Success = result;
      Message = new HtmlString(message);
      Html = html;
    }

    public PortalActionResult(bool result, string message, HtmlString html = null)
    {
      Success = result;
      Message = new HtmlString(message);
      if (html != null) Html = html.ToString();
    }

    public void AppendHtml(HtmlString htmlString)
    {
      Html += htmlString.ToString();
    }

    public async Task ExecuteResultAsync(ActionContext context)
    {
      var result = Newtonsoft.Json.JsonConvert.SerializeObject(this);
      await context.HttpContext.Response.WriteAsync(result);
    }
  }
}
