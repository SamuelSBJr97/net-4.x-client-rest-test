using System;

namespace ApiClient46.Models.Services
{
    public class TokenResponse
    {
        public string token { get; set; }
        public DateTime? expiresIn { get; set; }
    }
}