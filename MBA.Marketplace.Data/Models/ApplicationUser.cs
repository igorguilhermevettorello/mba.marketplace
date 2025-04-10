using MBA.Marketplace.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace MBA.Marketplace.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Vendedor Vendedor { get; set; }
    }
}
