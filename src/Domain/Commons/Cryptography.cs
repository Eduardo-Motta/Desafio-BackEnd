using System.Security.Cryptography;
using System.Text;

namespace Domain.Commons
{
    public static class Cryptography
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("8f12a3b5c7d8e9f01234567890abcdef");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("12ab34cd56ef78ab");

        public static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("The text to encrypt cannot be null or empty.", nameof(value));

            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var memoryStream = new MemoryStream();
            using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                var inputBytes = Encoding.UTF8.GetBytes(value);
                cryptoStream.Write(inputBytes, 0, inputBytes.Length);
            }

            return Convert.ToBase64String(memoryStream.ToArray());
        }
    }

}
