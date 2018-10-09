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
            //LogInstance.Log($"begin = {begin} end = {end} delta = {delta}");
#endif

            var result = new List<float>();

            if(delta == 0f)
            {
                result.Add(begin);
                return result;
            }

            if(begin == end)
            {
                result.Add(begin);
                return result;
            }

            delta = Math.Abs(delta);
            var absDelta = delta;

            if (begin > end)
            {
                delta = delta * -1f;
            }

#if DEBUG
            //LogInstance.Log($"NEXT delta = {delta}");
#endif

            var currentValue = begin;

            result.Add(begin);

            while (true)
            {
#if DEBUG
                //LogInstance.Log($"currentValue = {currentValue}");
#endif

                var newValue = currentValue + delta;

#if DEBUG
                //LogInstance.Log($"newValue = {newValue}");
#endif

                var currentAbsDelta = Math.Abs(end - newValue);

#if DEBUG
                //LogInstance.Log($"currentAbsDelta = {currentAbsDelta} absDelta = {absDelta}");
#endif

                if(currentAbsDelta >= absDelta)
                {
                    currentValue = newValue;
                    result.Add(newValue);
                }
                else
                {
                    result.Add(end);
                    return result;
                }              
            }
        }
    }
}
