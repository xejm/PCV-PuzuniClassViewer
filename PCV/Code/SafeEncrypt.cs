using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class SafeEncryptor
{

    public static string ekcon { get; private set; }
    public static string EncryptionKey { get; set; } = "default";


    public static string EncryptPin(string pin)
    {


        try
        {

            byte[] clearBytes = Encoding.Unicode.GetBytes(pin);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }, 1000);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    pin = Convert.ToBase64String(ms.ToArray());
                }
            }
            return pin;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            return "Error 01";
        }


    }

    public static string DecryptPin(string pin)
    {


        try
        {

            byte[] cipherBytes = Convert.FromBase64String(pin);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }, 1000);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    pin = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return pin;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            return "Error 02";
        }

    }



}
