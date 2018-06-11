using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IShortObjectToString
    {
        string ToShortString();
        string ToShortString(uint n);
        string PropertiesToShortSting(uint n);
    }
}

/*
        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            return sb.ToString();
        } 
*/
