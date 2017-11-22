using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace InstagramClone.Controllers
{
    public class common
    {

        //Reference: https://stackoverflow.com/questions/3984138/hash-string-in-c-sharp
        public static string HashPassword(string password)
        {
            var sha1 = SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(password);
            var hash = sha1.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static bool CheckPass(string secrect, string password)
        {
            string passwordHash = HashPassword(password);
            if (secrect == passwordHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
