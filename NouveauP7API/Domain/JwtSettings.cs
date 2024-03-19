﻿namespace NouveauP7API.Domain
{
    public class JwtSettings
    {
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? SecretKey { get; set; }
        public int ExpiryInMinutes { get; set; } // Ajoutez cette propriété
    }
}
