using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class AdjectiveDTNode : BaseDTNode
    {
        private enum KindOfDTNodeProperty
        {
            Undefined
        }

        public override bool IsAdjectiveDTNode => true;
        public override AdjectiveDTNode AsAdjectiveDTNode => this;

        //public ATNExtendedToken AdjectiveExtendedToken { get; set; }

        //public override void SetObject(BaseDTNode obj)
        //{
        //    throw new NotImplementedException();
        //}

        protected override void OnRemoveObjFromProp(BaseDTNode obj)
        {
#if DEBUG
            LogInstance.Log($"obj = {obj}");
#endif

            throw new NotImplementedException();
        }

        private Dictionary<BaseDTNode, KindOfDTNodeProperty> mKindsOfDTNodePropertiesDict = new Dictionary<BaseDTNode, KindOfDTNodeProperty>();
        private Dictionary<BaseDTNode, KindOfDTChild> mKindsOfDTChildDict = new Dictionary<BaseDTNode, KindOfDTChild>();

        public override KindOfDTChild GetKindOfDTChild(BaseDTNode obj)
        {
            if (obj == null)
            {
                return KindOfDTChild.Undefined;
            }

            if (mKindsOfDTChildDict.ContainsKey(obj))
            {
                return mKindsOfDTChildDict[obj];
            }

            return KindOfDTChild.Undefined;
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            //if (AdjectiveExtendedToken == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(AdjectiveExtendedToken)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(AdjectiveExtendedToken)}");
            //    sb.Append(AdjectiveExtendedToken.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(AdjectiveExtendedToken)}");
            //}
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));
            //if (AdjectiveExtendedToken == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(AdjectiveExtendedToken)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(AdjectiveExtendedToken)}");
            //    sb.Append(AdjectiveExtendedToken.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(AdjectiveExtendedToken)}");
            //}
            return sb.ToString();
        }
    }
}
