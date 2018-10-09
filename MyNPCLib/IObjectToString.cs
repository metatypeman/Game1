using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface IObjectToString
    {
        string ToString(uint n);
        string PropertiesToSting(uint n);
    }
}

/*
 public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            return sb.ToString();
        }
*/
