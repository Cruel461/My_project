using ITWitor.Models;

namespace ITWitor.Models
{
  public class Visit : BaseModel
    {
        public DateTime VisitStart { get; set; }
        public DateTime VisitEnd { get; set; }
        public string? UserAgent { get; internal set; }

        public Visit(string userAgent, DateTime visitStart = new DateTime())
        {
            VisitStart = visitStart == DateTime.MinValue ? DateTime.Now : visitStart;
            UserAgent = userAgent;
        }
    }
}
