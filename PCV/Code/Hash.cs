



using System;
using System.IO;
using System.Security.Cryptography;

public static class Hash
{

    public static string CalculateFileHash(string filePath)
    {
        using (var algorithm = HashAlgorithm.Create("SHA-256"))
        {
            if (algorithm == null)
            {
                throw new InvalidOperationException("SHA-256 hash algorithm is not supported.");
            }

            using (var fileStream = File.OpenRead(filePath))
            {
                byte[] hashBytes = algorithm.ComputeHash(fileStream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }


}