using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public static class ListHelper
    {
        public static bool IsEmpty<T>(this IList<T> list)
        {
            if(list == null)
            {
                return true;
            }

            if(list.Count == 0)
            {
                return true;
            }

            return false;
        }
    }
}
