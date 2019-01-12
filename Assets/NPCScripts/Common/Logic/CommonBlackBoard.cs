using MyNPCLib;
using MyNPCLib.Parser.LogicalExpression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.Common.Logic
{
    public class CommonBlackBoard : BaseBlackBoard, IObjectToString
    {
        public HumanoidBodyCommand LastCommand { get; set; }
        public bool IsReadyForsoundCommandExecuting { get; set; }
        public string Name { get; set; }

        public override void Bootstrap()
        {
            var queryStr = "{: name(?X,?Y) :}";

            var queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, Context.EntityDictionary);
            var query = queryStorage.MainRuleInstance;
            var keyOfActionQuestionVar = Context.EntityDictionary.GetKey("?Y");

            var querySearchResultCGStorage = Context.MainCGStorage.Search(queryStorage);

            var actionExpression = querySearchResultCGStorage.GetResultOfVar(keyOfActionQuestionVar);

#if DEBUG
            //LogInstance.Log($"actionExpression = {actionExpression}");
#endif

            if (actionExpression != null)
            {
                Name = actionExpression?.FoundExpression?.AsConcept.Name;
            }

#if DEBUG
            //LogInstance.Log($"Name = {Name}");
#endif
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsReadyForsoundCommandExecuting)} = {IsReadyForsoundCommandExecuting}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            return sb.ToString();
        }
    }
}
