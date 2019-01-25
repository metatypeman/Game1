using MyNPCLib;
using MyNPCLib.Logical;
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
        public BaseAbstractLogicalObject EntityOfRifle { get; set; }
        public ulong EntityIdOfInitRifle { get; set; }
        public string Team { get; set; }
        public bool IsShooting { get; set; }

        public override void Bootstrap()
        {
#if DEBUG
            Log("Begin");
#endif

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

            queryStr = "{: team(?X,?Y) :}";
            queryStorage = RuleInstanceFactory.ConvertStringToQueryCGStorage(queryStr, Context.EntityDictionary);
            querySearchResultCGStorage = Context.MainCGStorage.Search(queryStorage);

            actionExpression = querySearchResultCGStorage.GetResultOfVar(keyOfActionQuestionVar);

#if DEBUG
            LogInstance.Log($"actionExpression = {actionExpression}");
#endif

            if (actionExpression != null)
            {
                Team = actionExpression?.FoundExpression?.AsConcept.Name;
            }

#if DEBUG
            LogInstance.Log($"Team = {Team}");
#endif

            if (EntityIdOfInitRifle > 0)
            {
#if DEBUG
                Log($"EntityIdOfInitRifle = {EntityIdOfInitRifle}");
#endif

                var rifle = Context.GetLogicalObject(EntityIdOfInitRifle);

#if DEBUG
                Log($"(rifle == null) = {rifle == null}");
#endif

                EntityOfRifle = rifle;
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

        public string PropertiesToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsReadyForsoundCommandExecuting)} = {IsReadyForsoundCommandExecuting}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(EntityOfRifle)} = {EntityOfRifle}");
            sb.AppendLine($"{spaces}{nameof(EntityIdOfInitRifle)} = {EntityIdOfInitRifle}");
            sb.AppendLine($"{spaces}{nameof(Team)} = {Team}");
            return sb.ToString();
        }
    }
}
