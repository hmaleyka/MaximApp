using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;

namespace MaximApp.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsRemained { get; set; }
    }
}
