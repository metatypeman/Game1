using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public static class NamesHelper
    {
        public static string CreateEntityName()
        {
            return $"#{Guid.NewGuid().ToString("D")}";
        }
    }
}
