using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class RelationStorageOfSemanticAnalyzer : IObjectToString
    {
        private Dictionary<string, Dictionary<string, List<string>>> mInfoDict = new Dictionary<string, Dictionary<string, List<string>>>();
        private Dictionary<string, List<string>> mInfoForSingleRelationDict = new Dictionary<string, List<string>>();

        /// <summary>
        /// inputConcept -> relationName -> outputConcept
        /// </summary>
        /// <param name="inputConcept"></param>
        /// <param name="outputConcept"></param>
        /// <param name="relationName"></param>
        public void AddRelation(string inputConcept, string outputConcept, string relationName)
        {
            if(mInfoDict.ContainsKey(inputConcept))
            {
                var targetDict = mInfoDict[inputConcept];

                if(targetDict.ContainsKey(outputConcept))
                {
                    var targetList = targetDict[outputConcept];

                    if(!targetList.Contains(relationName))
                    {
                        targetList.Add(relationName);
                    }

                    return;
                }

                {
                    var targetList = new List<string>() { relationName };
                    targetDict[outputConcept] = targetList;
                }
                return;
            }

            {
                var targetList = new List<string>() { relationName };
                var targetDict = new Dictionary<string, List<string>>();
                targetDict[outputConcept] = targetList;
                mInfoDict[inputConcept] = targetDict;
            }
        }

        /// <summary>
        /// relationName -> outputConcept
        /// </summary>
        /// <param name="outputConcept"></param>
        /// <param name="relationName"></param>
        public void AddRelation(string outputConcept, string relationName)
        {
            if(mInfoForSingleRelationDict.ContainsKey(outputConcept))
            {
                var targetList = mInfoForSingleRelationDict[outputConcept];

                if (!targetList.Contains(relationName))
                {
                    targetList.Add(relationName);
                }

                return;
            }

            {
                var targetList = new List<string>() { relationName };
                mInfoForSingleRelationDict[outputConcept] = targetList;
            }
        }

        /// <summary>
        /// inputConcept -> relationName -> outputConcept
        /// </summary>
        /// <param name="inputConcept"></param>
        /// <param name="outputConcept"></param>
        /// <param name="relationName"></param>
        /// <returns></returns>
        public bool ContainsRelation(string inputConcept, string outputConcept, string relationName)
        {
            if (mInfoDict.ContainsKey(inputConcept))
            {
                var targetDict = mInfoDict[inputConcept];

                if(targetDict.ContainsKey(outputConcept))
                {
                    var targetList = targetDict[outputConcept];
                    return targetList.Contains(relationName);
                }
            }

            return false;
        }

        /// <summary>
        /// relationName -> outputConcept
        /// </summary>
        /// <param name="outputConcept"></param>
        /// <param name="relationName"></param>
        /// <returns></returns>
        public bool ContainsRelation(string outputConcept, string relationName)
        {
            if(mInfoForSingleRelationDict.ContainsKey(outputConcept))
            {
                var targetList = mInfoForSingleRelationDict[outputConcept];
                return targetList.Contains(relationName);
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
