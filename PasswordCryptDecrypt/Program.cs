using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace PasswordCryptDecrypt
{
    class Program
    {
        public static List<string> saltStringInDatabase = new List<string>() { "YU4MCiDhA3gmi43J4Lub0Sb892CUQ+Tqy0EpeG4GBqQ=", "rTHENMwXsiCvfS+W5SvmOnzGT5V7S9D0MhzXwgZLBkY=", "Ysbn7QKLhjzASJ6N/4hCePCOwr0bRJ37613vIT74528=" };

        static void Main(string[] args)
        {
            var lstPass = new List<string>(){"12312312", "12312312", "12312312", "12312312", "12312312", "12312312", "12312312", "12312312"};

            foreach (var pass in lstPass)
            {
                GenerateCryptedPassword(pass);
            }
        }

        public static string GenerateCryptedPassword(string password)
        {
            var salt = GenerateSaltString();

            Console.WriteLine($"Password: {password},Salt: {salt},Concat: {string.Concat(password,salt)}");
            var finalHash1 = string.Empty;
            using (var crypt = new SHA256Managed())
            {
                var passBytes = crypt.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(password, salt)));
                finalHash1 = passBytes.Cast<byte>().Aggregate(finalHash1, (current, theByte) => current + theByte.ToString("x2"));
            }

            Console.WriteLine($"Hashed password: {finalHash1}");
            return finalHash1;
        }
        public static string GenerateSaltString()
        {
            const int saltLength = 32;
            var salt = new byte[saltLength];
            using var random = new RNGCryptoServiceProvider();
            random.GetNonZeroBytes(salt);
            var result = Convert.ToBase64String(salt);

            if (CheckSaltInDatabase(result))
                GenerateSaltString();
            return result;
        }
        public static bool CheckSaltInDatabase(string salt)
        {
            return saltStringInDatabase.Contains(salt);
        }
    }
}
