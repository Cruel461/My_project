
using ITWitor.Models;

using Newtonsoft.Json;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Encodings.Web;

namespace ITWitor.Models
{
  public class Page : BaseModel
  {
    public string? Title { get; set; } = "Заголовок";
    public string? Description { get; set; } = "Описание";
    public string? KeyWords { get; set; } = "Ключевые слова";
    public string? LocalPath { get; set; }
    public string? Image { get; set; } = string.Empty;

    [ForeignKey("Parent")]
    public int? ParentId { get; set; }
    public Page? Parent { get; set; }

    public int QueueIndex { get; set; }

    public List<Page> Childrens { get; set; } = new List<Page>();

    public List<ContentBlock> ContentBlocks { get; set; } = new List<ContentBlock>();
  }
}