using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(64)]
        public string? Username { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string? Email { get; set; }
        [Required]
        [MaxLength(128)]
        public string? Password { get; set; }
        [Required]
        [MaxLength(64)]
        public required string FirstName { get; set; }
        [Required]
        [MaxLength(64)]
        public required string LastName { get; set; }
    }
}