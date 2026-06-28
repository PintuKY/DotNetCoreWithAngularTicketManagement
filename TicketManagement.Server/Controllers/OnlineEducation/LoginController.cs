using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Helper;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private static readonly ConcurrentDictionary<string, EmailOtpState> EmailOtps = new();
        private readonly AppDatabaseContext _db;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Users> _passwordHasher;

        public LoginController(AppDatabaseContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            _passwordHasher = new PasswordHasher<Users>();
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDto data)
        {
            var email = data.Email?.Trim().ToLower();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(data.Password))
                return BadRequest(new { message = "Email and password are required." });

            var user = await _db.users.FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == email);
            if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash))
                return Unauthorized(new { message = "Invalid email or password." });

            if (!user.IsActive)
                return Unauthorized(new { message = "Your account is inactive." });

            var passwordResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, data.Password);
            if (passwordResult == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Invalid email or password." });

            user.LastLoginDate = DateTime.UtcNow;
            user.UpdatedOn = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Login successful.",
                user = new
                {
                    user.Id,
                    user.UserGuid,
                    user.FullName,
                    user.Email,
                    user.Role
                }
            });
        }

        [HttpPost("registration")]
        public async Task<IActionResult> UserRegistration([FromBody] RegistrationDto data)
        {
            var firstName = data.FirstName?.Trim();
            var lastName = data.LastName?.Trim();
            var email = data.Email?.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(data.Password) ||
                string.IsNullOrWhiteSpace(data.ConfirmPassword))
            {
                return BadRequest(new { message = "First name, last name, email, password and confirm password are required." });
            }

            if (!IsValidPassword(data.Password))
                return BadRequest(new { message = "Password must be maximum 10 characters and include one capital letter, one small letter, one number and one special symbol." });

            if (data.Password != data.ConfirmPassword)
                return BadRequest(new { message = "Password and confirm password do not match." });

            var existingUser = await GetUserByEmailAsync(email);
            if (existingUser != null && !string.IsNullOrWhiteSpace(existingUser.PasswordHash))
                return Conflict(new { message = "Email is already registered." });

            if (existingUser == null || !existingUser.EmailVerified)
                return BadRequest(new { message = "Please verify your email with OTP before registration." });

            var user = existingUser;
            user.UserGuid ??= Guid.NewGuid();
            user.FullName = $"{firstName} {lastName}".Trim();
            user.Mobile = data.Mobile?.Trim();
            user.Role = "Student";
            user.IsActive = true;
            user.EmailVerified = true;
            user.UpdatedOn = DateTime.UtcNow;
            user.CreatedOn ??= DateTime.UtcNow;

            user.PasswordHash = _passwordHasher.HashPassword(user, data.Password);
            await _db.SaveChangesAsync();
            EmailOtps.TryRemove(email, out _);

            return Ok(new
            {
                message = "Registration successful.",
                user = new
                {
                    user.Id,
                    user.UserGuid,
                    user.FullName,
                    user.Email,
                    user.Role
                }
            });
        }

        //[HttpPost("emailvarifcation")] //EmailVerificationDto
        //public async Task<IActionResult> EmailVarifaction([FromBody] EmailOTPDto data)
        //{
        //    var email = data.Email?.Trim().ToLower();
        //    if (string.IsNullOrWhiteSpace(email))
        //        return BadRequest(new { message = "Email is required." });

        //    var existingUser = await GetUserByEmailAsync(email);
        //    if (existingUser != null && !string.IsNullOrWhiteSpace(existingUser.PasswordHash))
        //        return Conflict(new { message = "Email is already registered." });

        //    if (string.IsNullOrWhiteSpace(data.Otp))
        //    {
        //        var otp = Random.Shared.Next(100000, 999999).ToString();

        //        EmailOtps[email] = new EmailOtpState
        //        {
        //            Otp = otp,
        //            ExpiresOn = DateTime.UtcNow.AddMinutes(10),
        //            IsVerified = false
        //        };

        //        if (existingUser == null)
        //        {
        //            _db.emailOtp.Add(new EmailOTPs
        //            {
        //                Email = email,
        //                OTP = otp,
        //                IsVerified = false,
        //                // Expire after 10 minutes
        //                ExpiresOn = DateTimeHelper.GetOtpExpiry(),                       
        //                CreatedOn = DateTimeHelper.UtcNow()
        //            });
        //        }
        //        //else
        //        //{
        //        //    existingUser.EmailVerified = false;
        //        //    existingUser.UpdatedOn = DateTime.UtcNow;
        //        //}

        //        await _db.SaveChangesAsync();

        //        var emailSent = await SendOtpEmailAsync(email, otp);

        //        return Ok(new
        //        {
        //            message = emailSent
        //                ? "OTP sent to your email."
        //                : "OTP generated. Configure SMTP settings to send it by email.",
        //            emailSent,
        //            devOtp = emailSent ? null : otp
        //        });
        //    }

        //    if (!EmailOtps.TryGetValue(email, out var savedOtp))
        //        return BadRequest(new { message = "OTP not found. Please request a new OTP." });

        //    if (savedOtp.ExpiresOn < DateTime.UtcNow)
        //    {
        //        EmailOtps.TryRemove(email, out _);
        //        return BadRequest(new { message = "OTP expired. Please request a new OTP." });
        //    }

        //    if (savedOtp.Otp != data.Otp.Trim())
        //        return BadRequest(new { message = "Invalid OTP." });

        //    savedOtp.IsVerified = true;
        //    var user = await GetUserByEmailAsync(email);
        //    if (user == null)
        //    {
        //        user = new Users
        //        {
        //            UserGuid = Guid.NewGuid(),
        //            Email = email,
        //            Role = "Student",
        //            IsActive = false,
        //            CreatedOn = DateTime.UtcNow
        //        };
        //        _db.users.Add(user);
        //    }

        //    user.EmailVerified = true;
        //    user.UpdatedOn = DateTime.UtcNow;
        //    await _db.SaveChangesAsync();

        //    return Ok(new { message = "Email verified successfully." });
        //}

        [HttpPost("emailvarifcation")]
        public async Task<IActionResult> EmailVarifaction([FromBody] EmailOTPDto data)
        {
            try
            {
                if (data == null)
                    return BadRequest(new { message = "Request data is required." });

                var email = data.Email?.Trim().ToLower();

                // Email validation
                if (string.IsNullOrWhiteSpace(email))
                    return BadRequest(new { message = "Email is required." });

                if (!Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    return BadRequest(new { message = "Please enter a valid email address." });
                }

                // Check existing registered user
                var existingUser = await GetUserByEmailAsync(email);

                if (existingUser != null &&  !string.IsNullOrWhiteSpace(existingUser.PasswordHash))
                {
                    return Conflict(new
                    {
                        message = "Email is already registered."
                    });
                }

                // SEND OTP
                if (string.IsNullOrWhiteSpace(data.Otp)) 
                {
                    var otp = Random.Shared.Next(100000, 999999).ToString();

                    // Memory cache
                    EmailOtps[email] = new EmailOtpState
                    {
                        Otp = otp,
                        ExpiresOn = DateTimeHelper.GetOtpExpiry(),
                        IsVerified = false
                    };

                    // Remove old OTPs
                    var oldOtps = _db.emailOtp
                        .Where(x => x.Email == email && !x.IsVerified);

                    _db.emailOtp.RemoveRange(oldOtps);

                    // Save new OTP
                    _db.emailOtp.Add(new EmailOTPs
                    {
                        Email = email,
                        OTP = otp,
                        IsVerified = false,
                        ExpiresOn = DateTimeHelper.GetOtpExpiry(),
                        CreatedOn = DateTimeHelper.UtcNow()
                    });

                    await _db.SaveChangesAsync();

                    // Send Email
                    var emailSent = await SendOtpEmailAsync(email, otp);

                    return Ok(new
                    {
                        message = emailSent
                            ? "OTP sent successfully to your email."
                            : "OTP generated but email could not be sent. Check SMTP settings.",

                        emailSent,
                        devOtp = emailSent ? null : otp
                    });
                }

                // VERIFY OTP

                var savedOtp = await _db.emailOtp
                    .Where(x =>
                        x.Email == email &&
                        !x.IsVerified)
                    .OrderByDescending(x => x.CreatedOn)
                    .FirstOrDefaultAsync();

                if (savedOtp == null)
                {
                    return BadRequest(new
                    {
                        message = "OTP not found. Please request a new OTP."
                    });
                }

                if (savedOtp.ExpiresOn < DateTimeHelper.UtcNow())
                {
                    return BadRequest(new
                    {
                        message = "OTP expired. Please request a new OTP."
                    });
                }

                if (savedOtp.OTP != data.Otp.Trim())
                {
                    return BadRequest(new
                    {
                        message = "Invalid OTP."
                    });
                }

                savedOtp.IsVerified = true;

                if (existingUser == null)
                {
                    existingUser = new Users
                    {
                        UserGuid = Guid.NewGuid(),
                        Email = email,
                        Role = "Student",
                        IsActive = false,
                        EmailVerified = true,
                        CreatedOn = DateTimeHelper.UtcNow(),
                        UpdatedOn = DateTimeHelper.UtcNow()
                    };

                    _db.users.Add(existingUser);
                }
                else
                {
                    existingUser.EmailVerified = true;
                    existingUser.UpdatedOn = DateTimeHelper.UtcNow();
                }

                await _db.SaveChangesAsync();

                return Ok(new
                {
                    message = "Email verified successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while processing request.",
                    error = ex.Message
                });
            }
        }

        private Task<Users?> GetUserByEmailAsync(string email)
        {
            return _db.users.FirstOrDefaultAsync(u => u.Email != null && u.Email.ToLower() == email);
        }

        private static bool IsValidPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^A-Za-z0-9\s])\S{1,10}$");
        }

        private async Task<bool> SendOtpEmailAsync(string email, string otp)
        {
            var host = _configuration["EmailSettings:Host"];
            var portValue = _configuration["EmailSettings:Port"];
            var userName = _configuration["EmailSettings:UserName"];
            var password = _configuration["EmailSettings:Password"];
            var fromEmail = _configuration["EmailSettings:FromEmail"];
            var fromName = _configuration["EmailSettings:FromName"] ?? "Online Education";
            var enableSsl = bool.TryParse(_configuration["EmailSettings:EnableSsl"], out var ssl) && ssl;

            if (string.IsNullOrWhiteSpace(host) ||
                string.IsNullOrWhiteSpace(portValue) ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fromEmail) ||
                !int.TryParse(portValue, out var port))
            {
                return false;
            }

            try
            {
                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = "Your email verification OTP",
                    Body = $"Your OTP is {otp}. It will expire in 10 minutes.",
                    IsBodyHtml = false
                };
                message.To.Add(email);

                using var smtpClient = new SmtpClient(host, port)
                {
                    EnableSsl = enableSsl,
                    Credentials = new NetworkCredential(userName, password)
                };

                await smtpClient.SendMailAsync(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private class EmailOtpState
        {
            public string Otp { get; set; } = string.Empty;
            public DateTime ExpiresOn { get; set; }
            public bool IsVerified { get; set; }
        }

        // For larger projects, you might separate them like:
        //AuthController
        //    - Login
        //    - Logout
        //    - RefreshToken

        //RegistrationController
        //    - Register
        //    - EmailVerification

        //UserController
        //    - Profile
        //    - UpdateProfile
        //    - ChangePassword
    }
}
