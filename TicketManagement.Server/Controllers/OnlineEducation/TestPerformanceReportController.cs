using Microsoft.AspNetCore.Mvc;

namespace TicketManagement.Server.Controllers.OnlineEducation
{
    [ApiController]
    [Route("api/performance-report")]
    public sealed class TestPerformanceReportController : ControllerBase
    {

        [HttpGet]
        public IActionResult Index()
        {


            return Ok(new
            {
                examTitle = "Bihar Higher Secondary Computer Science Mock Test - 10",
                stats = new[]
              {
                new { label = "Score", value = "1.00/ 150", accent = "#486df5", icon = "▣" },
                new { label = "Accuracy", value = "50.00%", accent = "#21b6cf", icon = "◎" },
                new { label = "Air", value = "11 /14", accent = "#19c997", icon = "≡" },
                new { label = "Percentage", value = "0.67%", accent = "#ff4d43", icon = "%" },
                new { label = "Percentile", value = "21.43%", accent = "#ffb72b", icon = "●●" },
                new { label = "Avg Time/Ques", value = "0 Min 12 Sec", accent = "#6a45b8", icon = "◷" }
            },
                leaderboard = new[]
              {
                new { rank = 1, name = "Jitendra Kumar Sharma", marks = "70/150" },
                new { rank = 2, name = "Arman Ali", marks = "67/150" },
                new { rank = 3, name = "Aashutosh Kumar", marks = "65/150" },
                new { rank = 4, name = "kurpench", marks = "65/150" },
                new { rank = 5, name = "abhishek anand", marks = "64/150" },
                new { rank = 6, name = "KAMAKHYA MISHRA", marks = "63/150" }
            },
                comparison = new[]
              {
                new { label = "Score", icon = "▣", you = "1.00", topper = "70.00" },
                new { label = "Accuracy", icon = "◎", you = "50.00", topper = "46.67" },
                new { label = "Correct", icon = "✓", you = "1", topper = "70" },
                new { label = "Incorrect", icon = "×", you = "1", topper = "80" },
                new { label = "Total Time", icon = "◷", you = "0 Min 24 Sec", topper = "197 Min 52 Sec" }
            },
                highlights = new[]
              {
                new { title = "Strongest", subject = "Language", icon = "★", tone = "green" },
                new { title = "Weakest", subject = "Computer Science", icon = "⌘", tone = "red" },
                new { title = "Fastest", subject = "Computer Science", icon = "ϟ", tone = "yellow" },
                new { title = "Slowest", subject = "Language", icon = "⏱", tone = "blue" }
            },
                sections = new[]
              {
                new { section = "Language", attempted = "2 / 30", correct = "1 / 30", accuracy = "50.00%", time = "00:24" },
                new { section = "General Studies", attempted = "0 / 40", correct = "0 / 40", accuracy = "0%", time = "00:00" },
                new { section = "Computer Science", attempted = "0 / 80", correct = "0 / 80", accuracy = "0%", time = "00:00" }
            },
                efficiency = new[]
              {
                new { title = "Attempted", value = "2 of 150", icon = "✎", tone = "cyan" },
                new { title = "Correct", value = "1 of 150", icon = "✓", tone = "green" },
                new { title = "Incorrect", value = "1 of 150", icon = "×", tone = "red" },
                new { title = "Time/Ques", value = "0 Min 12 Sec", icon = "⌛", tone = "yellow" }
            }
            });
        }
    }
}
