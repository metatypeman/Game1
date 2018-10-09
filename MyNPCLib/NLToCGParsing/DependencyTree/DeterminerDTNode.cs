using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class DeterminerDTNode: BaseDTNode
    {
        public override bool IsDeterminerDTNode => true;
        public override DeterminerDTNode AsDeterminerDTNode => this;

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

            throw new NotImplementedException();
        }

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
    }
}
