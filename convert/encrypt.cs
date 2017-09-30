using System;
using System.IO;
using System.Security.Cryptography;

namespace convert
{
internal static class CryptographyHelpers
{ 
     private static string _key = "2cff5601e52f4747bfb9e271fe45042a";
        private static string _salt = "d31beaac47b44b45b1c6066712d49ff6";
   
    public static string StudyEncrypt(string identity)
    {
        return CryptographyHelpers.Encrypt(_key, _salt, identity);

    }
           public static string StudyDecrypt(string cryptograph)
    {
        return CryptographyHelpers.Decrypt(_key, _salt, cryptograph);
    }
    public static byte[] StudyFileDecrypt(string cypherfile)
    {
        var mm = System.IO.File.ReadAllText(cypherfile);
        var originfile = StudyDecrypt(mm);
        var bytesss = Convert.FromBase64String(originfile);
        return bytesss;

    }
    internal static string Decrypt(string password, string salt, string encrypted_value)
    {
        string decrypted;

        using (var aes = Aes.Create())
        {
            var keys = GetAesKeyAndIV(password, salt, aes);
            aes.Key = keys.Item1;
            aes.IV = keys.Item2;

            // create a decryptor to perform the stream transform.
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            // create the streams used for encryption.
            var encrypted_bytes = ToByteArray(encrypted_value);
            using (var memory_stream = new MemoryStream(encrypted_bytes))
            {
                using (var crypto_stream = new CryptoStream(memory_stream, decryptor, CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(crypto_stream))
                    {
                        decrypted = reader.ReadToEnd();
                    }
                }
            }
        }

        return decrypted;
    }

    internal static string Encrypt(string password, string salt, string plain_text)
    {
        string encrypted;

        using (var aes = Aes.Create())
        {
            var keys = GetAesKeyAndIV(password, salt, aes);
            aes.Key = keys.Item1;
            aes.IV = keys.Item2;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var memory_stream = new MemoryStream())
            {
                using (var crypto_stream = new CryptoStream(memory_stream, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(crypto_stream))
                    {
                        writer.Write(plain_text);
                    }

                    var encrypted_bytes = memory_stream.ToArray();
                    encrypted = ToString(encrypted_bytes);
                }
            }
        }

        return encrypted;
    }

   private static byte[] ToByteArray(string input)
{
    return Convert.FromBase64String(input);
}

private static string ToString(byte[] input)
{
    return Convert.ToBase64String(input);
}

    private static Tuple<byte[], byte[]> GetAesKeyAndIV(string password, string salt, SymmetricAlgorithm symmetricAlgorithm)
    {
        const int bits = 8;
        var key = new byte[16];
        var iv = new byte[16];

        var derive_bytes = new Rfc2898DeriveBytes(password, ToByteArray(salt));
        key = derive_bytes.GetBytes(symmetricAlgorithm.KeySize / bits);
        iv = derive_bytes.GetBytes(symmetricAlgorithm.BlockSize / bits);

        return new Tuple<byte[], byte[]>(key, iv);
    }

}
}