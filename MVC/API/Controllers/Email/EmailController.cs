using Microsoft.AspNetCore.Mvc;
using BL;
using Microsoft.AspNetCore.Cors;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;

        public EmailController(ILogger<EmailController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> SendGrid([FromBody] EmailRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.EmailRecipient))
            {
                _logger.LogError("Email recipient is null or empty.");
                return BadRequest("Email recipient is required.");
            }

            EmailManager em = new EmailManager();
            var response = await em.Execute(request.EmailRecipient);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Ok("Email sent successfully");
            }
            _logger.LogError($"Failed to send email to {request.EmailRecipient}. Status Code: {response.StatusCode}");
            return StatusCode((int)response.StatusCode, response.Body);
        }
    }

    public class EmailRequest
    {
        public string EmailRecipient { get; set; }
    }
}
