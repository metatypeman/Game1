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

        public static string GetDefaultToStringInformation(this IObjectToString targetObject, uint n)
        {
            var spaces = Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            var nameOfType = targetObject.GetType().FullName;
            sb.AppendLine($"{spaces}Begin {nameOfType}");
            sb.Append(targetObject.PropertiesToSting(nextN));
            sb.AppendLine($"{spaces}End {nameOfType}");
            return sb.ToString();
        }

        public static string GetDefaultToShortStringInformation(this IShortObjectToString targetObject, uint n)
        {
            var spaces = Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            var nameOfType = targetObject.GetType().FullName;
            sb.AppendLine($"{spaces}Begin {nameOfType}");
            sb.Append(targetObject.PropertiesToShortSting(nextN));
            sb.AppendLine($"{spaces}End {nameOfType}");
            return sb.ToString();
        }

        public static string GetDefaultToBriefStringInformation(this IObjectToBriefString targetObject, uint n)
        {
            var spaces = Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            var nameOfType = targetObject.GetType().FullName;
            sb.AppendLine($"{spaces}Begin {nameOfType}");
            sb.Append(targetObject.PropertiesToBriefSting(nextN));
            sb.AppendLine($"{spaces}End {nameOfType}");
            return sb.ToString();
        }
    }
}
