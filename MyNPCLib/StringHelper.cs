using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public static class StringHelper
    {
        public static string Spaces(uint n)
        {
            if(n == 0)
            {
                return string.Empty;
            }

            var sb = new StringBuilder();

            for (var i = 0; i < n; i++)
            {
                sb.Append(" ");
            }

            return sb.ToString();
        }
    }
}
