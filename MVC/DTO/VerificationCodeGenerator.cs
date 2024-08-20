using System;
using System.Linq;

namespace DTO
{
    public static class VerificationCodeGenerator
    {
        public static string GenerateVerificationCode(int length = 6)
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
