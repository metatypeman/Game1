using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class RelationStorageOfSemanticAnalyzer : IObjectToString
    {
        private Dictionary<ATNExtendedToken, Dictionary<ATNExtendedToken, List<string>>> mInfoDict = new Dictionary<ATNExtendedToken, Dictionary<ATNExtendedToken, List<string>>>();

        /// <summary>
        /// inputExtendedToken -> relationName -> outputExtendedToken
        /// </summary>
        /// <param name="inputExtendedToken"></param>
        /// <param name="outputExtendedToken"></param>
        /// <param name="relationName"></param>
        public void AddRelation(ATNExtendedToken inputExtendedToken, ATNExtendedToken outputExtendedToken, string relationName)
        {
            if(mInfoDict.ContainsKey(inputExtendedToken))
            {
                var targetDict = mInfoDict[inputExtendedToken];

                if(targetDict.ContainsKey(outputExtendedToken))
                {
                    var targetList = targetDict[outputExtendedToken];

                    if(!targetList.Contains(relationName))
                    {
                        targetList.Add(relationName);
                    }

                    return;
                }

                {
                    var targetList = new List<string>() { relationName };
                    targetDict[outputExtendedToken] = targetList;
                }
                return;
            }

            {
                var targetList = new List<string>() { relationName };
                var targetDict = new Dictionary<ATNExtendedToken, List<string>>();
                targetDict[outputExtendedToken] = targetList;
                mInfoDict[inputExtendedToken] = targetDict;
            }
        }

        /// <summary>
        /// inputExtendedToken -> relationName -> outputExtendedToken
        /// </summary>
        /// <param name="inputExtendedToken"></param>
        /// <param name="outputExtendedToken"></param>
        /// <param name="relationName"></param>
        /// <returns></returns>
        public bool ContainsRelation(ATNExtendedToken inputExtendedToken, ATNExtendedToken outputExtendedToken, string relationName)
        {
            if (mInfoDict.ContainsKey(inputExtendedToken))
            {
                var targetDict = mInfoDict[inputExtendedToken];

                if(targetDict.ContainsKey(outputExtendedToken))
                {
                    var targetList = targetDict[outputExtendedToken];
                    return targetList.Contains(relationName);
                }
            }

            return false;
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
            var sb = new StringBuilder();
            return sb.ToString();
        }
    }
}
