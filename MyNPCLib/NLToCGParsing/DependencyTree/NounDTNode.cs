using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class NounDTNode : BaseDTNode
    {
        public override bool IsNounDTNode => true;
        public override NounDTNode AsNounDTNode => this;

//        public ATNExtendedToken NounExtendedToken { get; set; }

//        public void AddAjective(AdjectiveDTNode adjective)
//        {
//#if DEBUG
//            LogInstance.Log($"adjective = {adjective}");
//#endif

//            throw new NotImplementedException();
//        }

//        public override void SetObject(BaseDTNode obj)
//        {
//            throw new NotImplementedException();
//        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            //if (NounExtendedToken == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(NounExtendedToken)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(NounExtendedToken)}");
            //    sb.Append(NounExtendedToken.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(NounExtendedToken)}");
            //}
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            //if (NounExtendedToken == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(NounExtendedToken)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(NounExtendedToken)}");
            //    sb.Append(NounExtendedToken.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(NounExtendedToken)}");
            //}
            return sb.ToString();
        }
    }
}
