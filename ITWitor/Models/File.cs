using ITWitor.Models;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace ITWitor.Models
{
  public class StaticFile : BaseModel
    {

        //[ForeignKey("Product")]
        //public int? ParentId { get; set; }
        //[NotMapped]
        //public Product? Product { get; set; }

        private string? path;
        private string? url;
        private string? originalName;
        private string? fileType;

        public string? Path { get => path; set => path = value; }
        public string? Url { get => url; set => url = value; }
        public string? FileType { get => fileType; set => fileType = value; }
        public string? OriginalName { get => originalName; set => originalName = value; }
        //  public virtual ICollection<ProductFile> ProductFiles { get; set; }


    }


    public static class FilesHelper
    {
        public static HtmlString ToTable(this IHtmlHelper html, IList<StaticFile> files)
        {
            string result = $"<table id=\"{nameof(files)}\" class=\"table table-striped table-bordered w-100\"><thead><tr>";

            result += $"<th><i class=\"fa fa-image\"></th><th>Имя</th><th>Тип</th><th>URL</th><th><i class=\"fa fa-remove\"></i></th><th><input type=\"checkbox\"/></th>";
            result += "</tr></thead></tbody>";

            if (files == null)
            {
                result += "</tbody></table>";
                return new HtmlString(result);
            }

            foreach (var item in files)
            {
                var fancybox = "";
                var player = "";
                if (item.FileType.Contains("image"))
                {
                    fancybox += $"<a class=\"fancybox nobg\" href=\"{item.Url}\"><img src=\"{item.Url}\" width=\"40\"/></a>";
                }
                else if (item.FileType.Contains("audio"))
                {
                    player += $"<audio controls> <source src='{item.Url}' type='audio/ogg; codecs=vorbis'> <source src='{item.Url}' type='audio/mpeg'> Тег audio не поддерживается вашим браузером. <a href='{item.Url}'>Скачать</a>. </audio>";
                }
                else if (item.FileType.Contains("video"))
                {

                }

                if (player == "")
                {
                    player = $"<a style=\"word-break: break-word;\" target=\"_blank\" href=\"{item.Url}\">{item.Url}</a>";
                }

                result += $"<tr data-id=\"{item.Id}\"><th>{fancybox}</th><th style=\"word-break: break-word;\" name=\"{nameof(item.OriginalName)}\">{item.OriginalName}</th><th style=\"word-break: break-word;\" name=\"{nameof(item.FileType)}\">{item.FileType}</th><th><i class=\"fa fa-copy\"></i>{player}<input name=\"{nameof(item.Url)}\" type=\"text\" readonly value=\"{item.Url}\" style=\"position: absolute; opacity: 0;\"/></th><th><i class=\"fa fa-remove\"></i></th><th><input type=\"checkbox\"/></th></tr>";
            }
            result += "</tbody></table>";

            return new HtmlString(result);
        }


        public static HtmlString ToTable(this StaticFile file)
        {
            if (file == null) return default;
            var result = "";
            var fancybox = "";
            var player = "";
            if (file.FileType.Contains("image"))
            {
                fancybox += $"<a class=\"fancybox nobg\" href=\"{file.Url}\"><img src=\"{file.Url}\" width=\"40\"/></a>";
            }
            else if (file.FileType.Contains("audio"))
            {
                player += $"<audio controls class=\"mr-4\"> <source src='{file.Url}' type='audio/ogg; codecs=vorbis'> <source src='{file.Url}' type='audio/mpeg'> Тег audio не поддерживается вашим браузером. <a href='{file.Url}'>Скачайть</a>. </audio>";
            }
            else if (file.FileType.Contains("video"))
            {

            }

            if (player == "")
            {
                player = $"<a target=\"_blank\" href=\"{file.Url}\">{file.Url}</a>";
            }

            result += $"<tr data-id=\"{file.Id}\"><th>{fancybox}</th><th name=\"{nameof(file.OriginalName)}\">{file.OriginalName}</th><th name=\"{nameof(file.FileType)}\">{file.FileType}</th><th><i class=\"fa fa-copy\"></i>{player}<input name=\"{nameof(file.Url)}\" type=\"text\" readonly value=\"{file.Url}\" style=\"position: absolute; opacity: 0;\"/></th><th><i class=\"fa fa-remove\"></i></th><th><input type=\"checkbox\"/></th></tr>";

            return new HtmlString(result);
        }
    }

}
