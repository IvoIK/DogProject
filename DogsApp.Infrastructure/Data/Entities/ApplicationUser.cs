using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations;

namespace DogsApp.Infrastructure.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } = null!;
    }
}