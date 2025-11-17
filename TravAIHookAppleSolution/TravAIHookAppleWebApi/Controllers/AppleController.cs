using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravAIHookAppleWebApi.Data;
using TravAIHookAppleWebApi.Middlewares;

namespace TravAIHookAppleWebApi.Controllers
{
    [ApiController]
    [Route("api/apple")]
    public class AppleController : ControllerBase
    {
        private readonly AppLogger _logger;
        private readonly AppDbContext _db;

        public AppleController(AppLogger logger, AppDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpPost("notifications")]
        public async Task<IActionResult> Notifications([FromBody] JsonElement payload)
        {
            try
            {
                if (!payload.TryGetProperty("signedPayload", out var spElement))
                {
                    await _logger.LogAsync("/api/apple/notifications", "POST",
                        "signedPayload bulunamadı", null, 0, "Error");

                    return Ok();
                }

                var signedPayload = spElement.GetString();

                // RAW LOG
                await _logger.LogAsync("/api/apple/notifications/raw", "POST",
                    "RAW Notification Payload", null, 0, "Info", null, payload.ToString());

                // JWT parse
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(signedPayload);

                var headerJson = JsonSerializer.Serialize(jwt.Header);
                var payloadJson = JsonSerializer.Serialize(jwt.Payload);

                await _logger.LogAsync("/api/apple/notifications/jwt_header", "POST", headerJson, null, 0, "Info");
                await _logger.LogAsync("/api/apple/notifications/jwt_payload", "POST", payloadJson, null, 0, "Info");

                var userIdStr = jwt.Claims.FirstOrDefault(c => c.Type == "appAccountToken")?.Value;
                var expiryStr = jwt.Claims.FirstOrDefault(c => c.Type == "expiresDate")?.Value;

                // appAccountToken = senin UserId'in
                if (!Guid.TryParse(userIdStr, out var userId))
                {
                    await _logger.LogAsync("/api/apple/notifications", "POST",
                        $"Geçersiz appAccountToken: {userIdStr}", null, 0, "Error");

                    return Ok();
                }

                var user = await _db.Users.FirstOrDefaultAsync(x => x.UserId == userId);
                if (user == null)
                {
                    await _logger.LogAsync("/api/apple/notifications", "POST",
                        $"Kullanıcı bulunamadı: {userId}", null, 0, "Warning");

                    return Ok();
                }

                if (long.TryParse(expiryStr, out var expiryMs))
                {
                    var expiryDate = DateTimeOffset.FromUnixTimeMilliseconds(expiryMs).UtcDateTime;
                    user.SubscriptionExpireDate = expiryDate;
                    await _db.SaveChangesAsync();

                    await _logger.LogAsync("/api/apple/notifications", "POST",
                        $"Subscription güncellendi | expiry: {expiryDate}", null, 0, "Success", userId);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                await _logger.LogAsync("/api/apple/notifications/error", "POST",
                    "EXCEPTION: " + ex.Message, null, 0, "Error");

                return Ok();
            }
        }
    }
}
