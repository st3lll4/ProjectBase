using System.Text.Json.Serialization;

namespace WebApp.DTOs.Identity;
public class JWTResponse
{
    [JsonPropertyName("jwt")]
    public string Jwt { get; set; } = default!;

    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = default!;
}