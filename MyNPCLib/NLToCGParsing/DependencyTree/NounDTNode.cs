using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class NounDTNode : BaseDTNode
    {
        private enum KindOfDTNodeProperty
        {
            Undefined,
            Ajective,
            Determiner
        }

        public override bool IsNounDTNode => true;
        public override NounDTNode AsNounDTNode => this;

        private List<AdjectiveDTNode> mAjectivesList = new List<AdjectiveDTNode>();

        public List<AdjectiveDTNode> AjectivesList
        {
            get
            {
                return mAjectivesList;
            }
        }

        public void AddAjective(AdjectiveDTNode adjectiveDTNode)
        {
#if DEBUG
            //LogInstance.Log($"adjectiveDTNode = {adjectiveDTNode}");
#endif

            if (adjectiveDTNode == null)
            {
                return;
            }

            if (mAjectivesList.Contains(adjectiveDTNode))
            {
                return;
            }

            mAjectivesList.Add(adjectiveDTNode);

            adjectiveDTNode.NRemoveParentIfNot(this);
            adjectiveDTNode.NSetParent(this);
            mKindsOfDTNodePropertiesDict.Add(adjectiveDTNode, KindOfDTNodeProperty.Ajective);
            mKindsOfDTChildDict.Add(adjectiveDTNode, KindOfDTChild.Object);
        }

        private List<BaseDTNode> mDeterminersList = new List<BaseDTNode>();

        public List<BaseDTNode> DeterminersList
        {
            get
            {
                return mDeterminersList;
            }
        }

        public void AddDeterminer(BaseDTNode determinerDTNode)
        {
#if DEBUG
            //LogInstance.Log($"determinerDTNode = {determinerDTNode}");
#endif

            if (determinerDTNode == null)
            {
                return;
            }

            if (mDeterminersList.Contains(determinerDTNode))
            {
                return;
            }

            mDeterminersList.Add(determinerDTNode);

            determinerDTNode.NRemoveParentIfNot(this);
            determinerDTNode.NSetParent(this);
            mKindsOfDTNodePropertiesDict.Add(determinerDTNode, KindOfDTNodeProperty.Determiner);
            mKindsOfDTChildDict.Add(determinerDTNode, KindOfDTChild.Object);
        }

        public override void SetValue(BaseDTNode obj, KindOfDTChild kindOfDTChild)
        {
#if DEBUG
            //LogInstance.Log($"obj = {obj}");
            //LogInstance.Log($"kindOfDTChild = {kindOfDTChild}");
#endif

            switch (kindOfDTChild)
            {
                case KindOfDTChild.Object:
                    if(obj.IsAdjectiveDTNode)
                    {
                        AddAjective(obj.AsAdjectiveDTNode);
                        return;
                    }
                    break;

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
                case KindOfDTNodeProperty.Ajective:
                    mAjectivesList.Remove(obj.AsAdjectiveDTNode);
                    break;

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
            if (AjectivesList == null)
            {
                sb.AppendLine($"{spaces}{nameof(AjectivesList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AjectivesList)}");
                foreach (var item in AjectivesList)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(AjectivesList)}");
            }
            if (DeterminersList == null)
            {
                sb.AppendLine($"{spaces}{nameof(DeterminersList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(DeterminersList)}");
                foreach (var item in DeterminersList)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(DeterminersList)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));
            if (AjectivesList == null)
            {
                sb.AppendLine($"{spaces}{nameof(AjectivesList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AjectivesList)}");
                foreach (var item in AjectivesList)
                {
                    sb.Append(item.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(AjectivesList)}");
            }
            if (DeterminersList == null)
            {
                sb.AppendLine($"{spaces}{nameof(DeterminersList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(DeterminersList)}");
                foreach (var item in DeterminersList)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(DeterminersList)}");
            }
            return sb.ToString();
        }
    }
}
