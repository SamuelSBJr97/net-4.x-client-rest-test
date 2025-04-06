using System;

namespace ApiClient.Services
{
    internal class TokenResponse
    {
        public string AccessToken { get; internal set; }
        public DateTime ExpiresIn { get; internal set; }
    }
}