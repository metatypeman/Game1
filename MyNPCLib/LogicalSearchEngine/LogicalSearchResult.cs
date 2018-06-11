﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class LogicalSearchResult: IObjectToString
    {
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
    }
}
