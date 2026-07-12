using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Services.OnlineEducation
{
    public class UserProfilesService : IUserProfilesService
    {
        private readonly AppDatabaseContext _db;
        private readonly ILogger<UserProfilesService> _logger;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;
        private readonly IPasswordHasher<Users> _passwordHasher;

        // Max upload size 5 MB
        private const int MaxImageBytes = 5 * 1024 * 1024;
        private static readonly string[] AllowedMimes = new[] { "image/png", "image/jpeg", "image/jpg", "image/webp", "image/gif" };

        public UserProfilesService(AppDatabaseContext db, ILogger<UserProfilesService> logger, IUserService userService, IWebHostEnvironment env, IPasswordHasher<Users> passwordHasher)
        {
            _db = db;
            _logger = logger;
            _userService = userService;
            _env = env;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> SaveUserprofileAsync(UserProfilesDTO data, ClaimsPrincipal userClaims)
        {
            try
            {
                var currentUserId = _userService.GetCurrentUserId(userClaims);
                if (!currentUserId.HasValue)
                {
                    _logger.LogWarning("SaveUserprofileAsync: no current user id in claims");
                    return false;
                }

                var userId = currentUserId.Value;

                // Update Users table (FullName, Email, Mobile) if provided
                var user = await _db.users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user == null)
                {
                    _logger.LogWarning("SaveUserprofileAsync: user record not found for id {UserId}", userId);
                    return false;
                }

                if (!string.IsNullOrWhiteSpace(data.FirstName) || !string.IsNullOrWhiteSpace(data.LastName))
                {
                    var first = data.FirstName?.Trim() ?? string.Empty;
                    var last = data.LastName?.Trim() ?? string.Empty;
                    var full = string.IsNullOrEmpty(last) ? first : $"{first} {last}".Trim();
                    if (!string.IsNullOrWhiteSpace(full))
                        user.FullName = full;
                }

                if (!string.IsNullOrWhiteSpace(data.Email))
                    user.Email = data.Email.Trim();

                if (!string.IsNullOrWhiteSpace(data.Phone))
                    user.Mobile = data.Phone.Trim();

                // Save or update profile entity
                var existing = await _db.userProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
                UserProfiles entity = existing ?? new UserProfiles { UserId = userId };

                if (!string.IsNullOrWhiteSpace(data.Address))
                    entity.Address = data.Address;

                if (!string.IsNullOrWhiteSpace(data.City))
                    entity.City = data.City;

                if (!string.IsNullOrWhiteSpace(data.State))
                    entity.State = data.State;

                if (data.DOB.HasValue)
                    entity.DOB = data.DOB.Value;

                // Handle profile image data URL with server-side validation
                if (!string.IsNullOrWhiteSpace(data.ProfileImage) && data.ProfileImage.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
                {
                    var savedUrl = await SaveDataUrlImageAsync(data.ProfileImage);
                    if (!string.IsNullOrEmpty(savedUrl))
                        entity.ProfileImageUrl = savedUrl;
                    else
                        _logger.LogWarning("Profile image upload failed or validation failed for user {UserId}", userId);
                }

                if (existing == null)
                    _db.userProfiles.Add(entity);
                else
                    _db.userProfiles.Update(entity);

                // Update user object and save both changes in one SaveChanges call
                _db.users.Update(user);

                await _db.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error saving user profile");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving user profile");
                return false;
            }
        }

        public async Task<UserProfilesDTO?> GetUserProfileAsync(ClaimsPrincipal userClaims)
        {
            try
            {
                var currentUserId = _userService.GetCurrentUserId(userClaims);
                if (!currentUserId.HasValue)
                    return null;

                var userId = currentUserId.Value;

                var user = await _db.users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
                var profile = await _db.userProfiles.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId);

                var result = new UserProfilesDTO
                {
                    FirstName = user?.FullName, // if you prefer splitting, implement logic
                    Email = user?.Email,
                    Phone = user?.Mobile,
                    Address = profile?.Address,
                    City = profile?.City,
                    State = profile?.State,
                    DOB = profile?.DOB,
                    ProfileImageUrl = profile?.ProfileImageUrl
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user profile");
                return null;
            }
        }

        public async Task<bool> ChangePasswordAsync(PasswordUpdateDTO model, ClaimsPrincipal userClaims)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.CurrentPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
                    return false;

                if (model.NewPassword != model.ConfirmPassword)
                    return false;

                var user = await _userService.GetCurrentUserAsync(userClaims);
                if (user == null)
                    return false;

                // Verify current password
                var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? string.Empty, model.CurrentPassword);
                if (verify == PasswordVerificationResult.Failed)
                    return false;

                // Optionally: add password strength checks here

                // Hash new password and update
                user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                _db.users.Update(user);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error changing password");
                return false;
            }
        }

        private async Task<string?> SaveDataUrlImageAsync(string dataUrl)
        {
            try
            {
                var commaIndex = dataUrl.IndexOf(',');
                if (commaIndex < 0)
                    return null;

                var meta = dataUrl.Substring(5, commaIndex - 5); // skip "data:"
                var base64 = dataUrl.Substring(commaIndex + 1);

                // meta example: "image/jpeg;base64"
                var parts = meta.Split(';', StringSplitOptions.RemoveEmptyEntries);
                var mime = parts.Length > 0 ? parts[0] : "image/jpeg";

                // Validate mime
                if (Array.IndexOf(AllowedMimes, mime.ToLowerInvariant()) < 0)
                {
                    _logger.LogWarning("Rejected image mime {Mime}", mime);
                    return null;
                }

                var bytes = Convert.FromBase64String(base64);
                if (bytes.Length > MaxImageBytes)
                {
                    _logger.LogWarning("Rejected image - size {Size} exceeds {Max}", bytes.Length, MaxImageBytes);
                    return null;
                }

                var ext = mime switch
                {
                    "image/png" => ".png",
                    "image/webp" => ".webp",
                    "image/gif" => ".gif",
                    _ => ".jpg"
                };

                var uploadsFolder = Path.Combine(_env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"), "uploads", "profileimages");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var fileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await File.WriteAllBytesAsync(filePath, bytes);

                var relativeUrl = $"/uploads/profileimages/{fileName}";
                return relativeUrl;
            }
            catch (FormatException fex)
            {
                _logger.LogWarning(fex, "Invalid base64 profile image supplied");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed saving profile image from data URL");
                return null;
            }
        }
    }
}
