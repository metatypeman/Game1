using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class NounDTNode : BaseDTNode
    {
        private enum KindOfDTNodeProperty
        {
            Undefined
        }

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
            sb.Append(base.PropertiesToShortSting(n));
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
