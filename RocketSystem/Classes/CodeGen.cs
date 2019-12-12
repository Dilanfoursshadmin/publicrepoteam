using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RocketSystem.Classes
{
    public class CodeGen
    {
        public static int sendCode()
        {
            Random rnd = new Random();
            int number = rnd.Next(9999, 100000);
            return number;
        }
    }
}