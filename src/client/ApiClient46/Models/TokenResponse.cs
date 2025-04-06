using System;

namespace ApiClient46.Models.Services
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}