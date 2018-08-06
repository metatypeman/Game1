using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LogicalSearchEngine
{
    public class EntityConditionScopeForConvertorToCompleteRuleInstance
    {
        public EntityConditionScopeForConvertorToCompleteRuleInstance(IEntityDictionary entityDictionary, string varName, List<string> values)
        {
            mEntityDictionary = entityDictionary;
            mVarName = varName;
            mValues = values;
        }

        private IEntityDictionary mEntityDictionary;
        private string mVarName;
        private List<string> mValues;
        public EntityConditionScopeForConvertorToCompleteRuleInstance Next { get; set; }

        public void Run(List<RuleInstance>.Enumerator resultListEnumerator)
        {
            if(Next != null)
            {
                throw new NotImplementedException();
            }

#if DEBUG
            //LogInstance.Log($"mVarName = {mVarName}");
#endif

            foreach (var value in mValues)
            {
#if DEBUG
                //LogInstance.Log($"value = {value}");
#endif

                if(!resultListEnumerator.MoveNext())
                {
                    throw new NotSupportedException();
                }

                var currentResult = resultListEnumerator.Current;

#if DEBUG
                //LogInstance.Log($"currentResult = {currentResult}");
#endif

                var entitiesCondition = currentResult.EntitiesConditions;

                if(entitiesCondition != null)
                {
                    throw new NotSupportedException();
                }

                entitiesCondition = new EntitiesConditions();
                currentResult.EntitiesConditions = entitiesCondition;

                var itemsList = new List<EntityConditionItem>();
                entitiesCondition.Items = itemsList;

                var item = new EntityConditionItem();
                itemsList.Add(item);
                item.Name = value;
                item.Key = mEntityDictionary.GetKey(value);
                item.VariableName = mVarName;
                item.VariableKey = mEntityDictionary.GetKey(mVarName);
            }
        }
    }
}
