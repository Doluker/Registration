using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace HashPasswords
{
    public class Hash
    {
        public static string PasswordHash(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return string.Empty;
            }
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hasBytes = sha256.ComputeHash(bytes);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hasBytes.Length; i++)
                    {
                        sb.Append(hasBytes[i].ToString("x2").ToUpper());
                    }
                    return sb.ToString();
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine($"Ошибка:{e.Message}");
                return string.Empty;
            }
        }
    }
}
