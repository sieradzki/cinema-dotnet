using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Settings
{
    public sealed class JwtOptions
    {
        public const string SectionName = "JWT";
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public string SigningKey { get; init; } = string.Empty;
        public int ExpirationMinutes { get; init; } = 60;
    }
}