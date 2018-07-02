using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class RolesStorageOfSemanticAnalyzer : IObjectToString
    {
        private Dictionary<string, List<ATNExtendedToken>> mRolesDict = new Dictionary<string, List<ATNExtendedToken>>();

        public void Add(string role, ATNExtendedToken extendedToken)
        {
            if(mRolesDict.ContainsKey(role))
            {
                var itemsList = mRolesDict[role];
                if(itemsList.Contains(extendedToken))
                {
                    return;
                }
                itemsList.Add(extendedToken);
                return;
            }

            var newItemsList = new List<ATNExtendedToken>();
            newItemsList.Add(extendedToken);
            mRolesDict[role] = newItemsList;
        }

        public int Count
        {
            get
            {
                return mRolesDict.Count;
            }             
        }

        public void Assing(RolesStorageOfSemanticAnalyzer source)
        {
            if(source.Count == 0)
            {
                return;
            }

            foreach(var rolesDictKVPItem in source.mRolesDict)
            {
                var role = rolesDictKVPItem.Key;

                foreach (var extendedToken in rolesDictKVPItem.Value)
                {
                    Add(role, extendedToken);
                }
            }
        }

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
            var nextNSpaces = StringHelper.Spaces(nextN);
            var nextNextN = nextN + 4;
            var sb = new StringBuilder();
            if(mRolesDict == null)
            {
                sb.AppendLine($"{spaces}{nameof(mRolesDict)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(mRolesDict)}");
                foreach (var role in mRolesDict)
                {
                    sb.AppendLine($"{nextNSpaces}{nameof(role)} = {role.Key}");
                    var itemsOfRole = role.Value;
                    sb.AppendLine($"{spaces}Begin {nameof(itemsOfRole)}");
                    foreach (var itemOfRole in itemsOfRole)
                    {
                        sb.Append(itemOfRole.ToString(nextNextN));
                    }
                    sb.AppendLine($"{spaces}End {nameof(itemsOfRole)}");
                }
                sb.AppendLine($"{spaces}End {nameof(mRolesDict)}");
            }
            return sb.ToString();
        }
    }
}
