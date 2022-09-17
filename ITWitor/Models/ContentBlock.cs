using ITW;

using ITWitor.Models;

using Microsoft.AspNetCore.Html;

using Newtonsoft.Json;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace ITWitor.Models
{
  public class ContentBlock : BaseModel, IContentBlock
  {
    public int? ParentId { get; set; }

    [ForeignKey("ParentId")]
    public Page? Parent { get; set; }

    public BlockType BlockType { get; set; }

    public int QueueIndex { get; set; }

    public string? Html { get; set; } = string.Empty;

    [NotMapped]
    [JsonIgnore]
    public HtmlString? HtmlString
    {
      get
      {
        string result = string.Empty;
        result = Html != null ? Regex.Replace(Html.Replace("</pre>", "").Replace("&gt;", ">").Replace("&lt;", "<").Replace("&quot;", "\""), @"\<pre.+\>", string.Empty) : result;
        return new HtmlString(result);
      }
    }
  }


  public class BlockType : BaseModel
  {
    [ForeignKey("Parent")]
    public int? ParentId { get; set; }

    public ContentBlock? Parent { get; set; }

    private ContentBlockType contentBlockType;

    public ContentBlockType ContentBlockType
    {
      get
      {
        FriendlyName = contentBlockType.DisplayName();
        return contentBlockType;
      }
      set => contentBlockType = value;
    }
    public string? FriendlyName { get; set; }
  }


  public enum ContentBlockType
  {
    [Display(Name = "Смешанный")]
    HtmlEditor = 1,

    [Display(Name = "Галерея")]
    Gallery = 2,

    [Display(Name = "Unknown")]
    Unknown = 0
  }
}