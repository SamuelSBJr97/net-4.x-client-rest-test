using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ApiClient46.Services
{
    public static class SanitizeHelper
    {
        public static string SanitizeKey(string key)
        {
            // Permitir apenas caracteres [a-z], [0-9] e [-]
            string sanitizedKey = Regex.Replace(key, @"[^a-z0-9-]", "", RegexOptions.IgnoreCase);

            // Substituir múltiplos '-' consecutivos por um único '-'
            sanitizedKey = Regex.Replace(sanitizedKey, @"-+", "-");

            // Remover '-' no início ou no final da string
            sanitizedKey = sanitizedKey.Trim('-');

            return sanitizedKey;
        }
    }
}