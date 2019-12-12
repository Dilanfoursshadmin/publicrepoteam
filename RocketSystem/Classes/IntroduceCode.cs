using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace RocketSystem.Classes
{
    public class IntroduceCode
    {
        static char[] ValidChars = {'2','3','4','5','6','7','8','9',
                   'A','B','C','D','E','F','G','H',
                   'J','K','L','M','N','P','Q',
                   'R','S','T','U','V','W','X','Y','Z'}; // len=32

        //key for HMAC function -- change!
        const int codelength = 7; // lenth of passcode

        public static string sendInCode(string address)
        {

            string hashkey = "password";
            byte[] hash;
            using (HMACSHA1 sha1 = new HMACSHA1(ASCIIEncoding.ASCII.GetBytes(hashkey)))
                hash = sha1.ComputeHash(UTF8Encoding.UTF8.GetBytes(address));
            int startpos = hash[hash.Length - 1] % (hash.Length - codelength);
            StringBuilder passbuilder = new StringBuilder();
            for (int i = startpos; i < startpos + codelength; i++)
                passbuilder.Append(ValidChars[hash[i] % ValidChars.Length]);
            return passbuilder.ToString();
        }
    }
}