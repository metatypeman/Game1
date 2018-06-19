using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IObjectToBriefString
    {
        string ToBriefString();
        string ToBriefString(uint n);
        string PropertiesToBriefSting(uint n);
    }
}

/*
        public string ToBriefString()
        {
            return ToBriefString(0u);
        }

        public string ToBriefString(uint n)
        {
            return this.GetDefaultToBriefStringInformation(n);
        }

        public string PropertiesToBriefSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            return sb.ToString();
        } 
*/
