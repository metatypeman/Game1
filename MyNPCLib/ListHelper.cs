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

        public static IList<float> GetRange(float begin, float end, float delta)
        {
#if DEBUG
            LogInstance.Log($"BaseNPCContext GetRange begin = {begin} end = {end} delta = {delta}");
#endif

            var result = new List<float>();


        }
    }
}
