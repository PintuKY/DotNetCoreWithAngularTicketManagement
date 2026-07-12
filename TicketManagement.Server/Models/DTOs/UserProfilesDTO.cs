using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TicketManagement.Server.Models.DTOs
{
    public class UserProfilesDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Phone { get; set; }
        public DateOnly? DOB { get; set; }
        public string? Address { get; set; }

        // Base64 data URL from client (optional) e.g. "data:image/jpeg;base64,..."
        public string? ProfileImage { get; set; }

        // Optional password change summary; used by change-password endpoint
        public PasswordUpdateDTO? PasswordSummary { get; set; }

        // Returned profile image url (when reading)
        public string? ProfileImageUrl { get; set; }
    }

    public class PasswordUpdateDTO
    {
        public string? CurrentPassword { get; set; }       
       

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(20, MinimumLength = 8,ErrorMessage = "Password must be between 8 and 20 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!_+\-=?<>.,;:()\[\]{}|\\/`~])[A-Za-z\d@#$%^&*!_+\-=?<>.,;:()\[\]{}|\\/`~]{8,20}$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string? NewPassword { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
