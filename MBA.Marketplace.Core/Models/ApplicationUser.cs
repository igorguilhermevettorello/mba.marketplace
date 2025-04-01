using MBA.Marketplace.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace MBA.Marketplace.Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Vendedor Vendedor { get; set; }
    }
}
