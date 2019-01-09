using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public static class PathsHelper
    {
        public static string DisplayPath(IList<IPlane> path)
        {
            var sb = new StringBuilder();

            foreach (var item in path)
            {
                sb.Append($"{item.Name},");
            }
            sb.Remove(sb.Length - 1, 1);

            var str = sb.ToString().Replace(",", " -> ");
            return str;
        }
    }
}
