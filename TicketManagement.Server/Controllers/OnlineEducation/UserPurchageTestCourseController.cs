using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TicketManagement.Server.DBContexts;
using TicketManagement.Server.Models.DTOs;
using TicketManagement.Server.Models.OnlineEducation;
using TicketManagement.Server.Services.OnlineEducation;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserPurchageTestCourseController : Controller
    {
        private readonly IUserTestCourse _iuserTestCourse;
        private readonly IUserService _iuserService;
        private readonly ITestService _testService;
        private readonly AppDatabaseContext _db;
        private readonly ILogger<UserPurchageTestCourseController> _logger;

        public UserPurchageTestCourseController(
            IUserTestCourse iuserTestCourse,
            IUserService iuserservice,
            ITestService testService,
            AppDatabaseContext db,
            ILogger<UserPurchageTestCourseController> logger)
        {
            _iuserTestCourse = iuserTestCourse;
            _iuserService = iuserservice;
            _testService = testService;
            _db = db;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("usercoursedata")]
        public async Task<IActionResult> MyCourses()
        {
            var userId = _iuserService.GetCurrentUserId(User);
            if (!userId.HasValue)
                return Unauthorized(new { message = "User not authenticated" });

            var UserCourseData = await _iuserTestCourse.GetUserTestCourseAsync(User);
            return Ok(UserCourseData);
        }

        // POST: api/UserPurchageTestCourse/TestPayment
        [HttpPost("TestPayment")]
        public async Task<IActionResult> TestPayment([FromBody] UPaymentDTO payload)
        {
            if (payload == null)
                return BadRequest(new { message = "Payload is required." });

            if (payload.TestGuid == Guid.Empty)
                return BadRequest(new { message = "Invalid test GUID." });

            if (payload.PaymentPrice <= 0)
                return BadRequest(new { message = "Invalid payment amount." });

            var user = await _iuserService.GetCurrentUserAsync(User);
            if (user == null)
                return Unauthorized(new { message = "User must be logged in to make a purchase." });

            var test = await _db.Tests.AsNoTracking().FirstOrDefaultAsync(t => t.TestGuid == payload.TestGuid);
            if (test == null)
                return BadRequest(new { message = "Test not found for supplied TestGuid." });

            if (test.Price != payload.PaymentPrice)
            {
                _logger.LogInformation("Client price {ClientPrice} differs from server price {ServerPrice} for TestId {TestId}", payload.PaymentPrice, test.Price, test.Id);
            }

            await using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var payment = new Payment
                {
                    PaymentGuid = Guid.NewGuid(),
                    UserId = user.Id,
                    TestId = test.Id,
                    Amount = payload.PaymentPrice,
                    PaymentStatus = "Success",
                    PaymentMethod = payload.PaymentMethod ?? "card",
                    GatewayOrderId = string.Empty,
                    GatewayPaymentId = string.Empty,
                    PaymentDate = DateTime.UtcNow,
                    TransactionId = 0,
                    Active = true
                };

                // Use the exact DbSet property as declared in AppDatabaseContext
                _db.payments.Add(payment);
                await _db.SaveChangesAsync();

                var userCourse = new UserTestCourse
                {
                    UserCoursGuid = Guid.NewGuid(),
                    UserId = user.Id,
                    TestId = test.Id,
                    PaymentId = payment.Id,
                    PurchaseDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddYears(1),
                    Status = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    Progress = 0m,
                    LastAccessed = null,
                    Completed = false,
                    CertificateIssued = false,
                    CertificateExpired = false
                };

                _db.UserCourses.Add(userCourse);
                await _db.SaveChangesAsync();

                await tx.CommitAsync();

                return Ok(new
                {
                    success = true,
                    message = "Payment recorded and course granted.",
                    paymentId = payment.Id,
                    userCourseId = userCourse.ID
                });
            }
            catch (Exception ex)
            {
                await tx.RollbackAsync();
                _logger.LogError(ex, "Error while processing TestPayment for user {UserId}", user.Id);
                return Problem(detail: ex.Message, statusCode: 500);
            }
        }
    }
}
