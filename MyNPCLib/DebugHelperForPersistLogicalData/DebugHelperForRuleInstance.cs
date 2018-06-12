using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.DebugHelperForPersistLogicalData
{
    public class ContextForDebugHelperForRuleInstance
    {
        public string MainView { get; set; }
        public List<string> AnnotationsViews { get; set; } = new List<string>();
    }

    public static class DebugHelperForRuleInstance
    {
        private static string ToString(ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            if(context.AnnotationsViews.Count == 0)
            {
                return context.MainView;
            }

            return sb.ToString();
        }

        public static string ToString(RuleInstance source)
        {
            var context = new ContextForDebugHelperForRuleInstance();
            context.MainView = ToString(source, context);
            return ToString(context);
        }

        private static string ToString(RuleInstance source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append($"{{:{source.Name}");
            if(source.Part_1 != null || source.Part_2 != null)
            {
                var markBetweenParts = GetMarkBetweenParts(source);

                if(source.Part_1 != null)
                {
                    sb.Append(ToString(source.Part_1, context));
                }
                
                sb.Append(markBetweenParts);

                if (source.Part_2 != null)
                {
                    sb.Append(ToString(source.Part_2, context));
                }
            }
            sb.Append(":}}");
            return sb.ToString();
        }

        private static string GetMarkBetweenParts(RuleInstance source)
        {
            if(source.Part_1 == null || source.Part_2 == null)
            {
                return string.Empty;
            }

            if(source.IsPart_1_Active && source.IsPart_2_Active)
            {
                return "<->";
            }

            if(source.IsPart_1_Active)
            {
                return "->";
            }

            return "<-";
        }

        public static string ToString(RulePart source)
        {
            var context = new ContextForDebugHelperForRuleInstance();
            context.MainView = ToString(source, context);
            return ToString(context);
        }

        private static string ToString(RulePart source, ContextForDebugHelperForRuleInstance context)
        {
            var sb = new StringBuilder();
            sb.Append($"{{}}");
            return sb.ToString();
        }

        public static string 
    }
}
