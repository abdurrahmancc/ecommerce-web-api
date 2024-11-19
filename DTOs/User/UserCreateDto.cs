using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce_web_api.DTOs.User
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@example\.com$", ErrorMessage = "Email must be from 'example.com' domain.")]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        [Required]
        [Compare("PasswordHash")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Role { get; set; } = "user";
        public DateTime? DateOfBirth { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string countryCode { get; set; }
        public string ProfilePictureUrl { get; set; }
    }
}