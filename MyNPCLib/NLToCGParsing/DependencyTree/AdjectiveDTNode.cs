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

        public override void SetValue(BaseDTNode obj, KindOfDTChild kindOfDTChild)
        {
#if DEBUG
            //LogInstance.Log($"obj = {obj}");
            //LogInstance.Log($"kindOfDTChild = {kindOfDTChild}");
#endif

            switch (kindOfDTChild)
            {
                default: throw new ArgumentOutOfRangeException(nameof(kindOfDTChild), kindOfDTChild, null);
            }
        }

        protected override void OnRemoveObjFromProp(BaseDTNode obj)
        {
#if DEBUG
            //LogInstance.Log($"obj = {obj}");
#endif

            if (!mKindsOfDTNodePropertiesDict.ContainsKey(obj))
            {
                return;
            }

            var kinfOfDTNode = mKindsOfDTNodePropertiesDict[obj];
            mKindsOfDTNodePropertiesDict.Remove(obj);
            mKindsOfDTChildDict.Remove(obj);

            switch (kinfOfDTNode)
            {
                default: throw new ArgumentOutOfRangeException(nameof(kinfOfDTNode), kinfOfDTNode, null);
            }
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
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));
            return sb.ToString();
        }
    }
}
