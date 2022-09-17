using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Identity;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace ITWitor.Models
{
    public class AppUser : IdentityUser
    {
        [DisplayName("Имя")]
        public string? Name { get; set; }

        [DisplayName("Роль")]
        public IdentityRole? Role { get; set; }

        [DisplayName("Зарегистрирован")]
        public DateTime Registered { get; set; }

        [DisplayName("Последний визит")]
        public DateTime LastVisit { get; set; }
    }
}
