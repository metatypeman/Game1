using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class PrepositionalDTNode : BaseDTNode
    {
        private enum KindOfDTNodeProperty
        {
            NounObject,
            AdjectiveObject,
        }

        public override bool IsPrepositionalDTNode => true;
        public override PrepositionalDTNode AsPrepositionalDTNode => this;

        private NounDTNode mNounObject;
        public NounDTNode NounObject
        {
            get
            {
                return mNounObject;
            }

            set
            {
                if (mNounObject == value)
                {
                    return;
                }

                if (mNounObject != null)
                {
                    mNounObject.NRemoveParent(this);
                }

                mNounObject = value;

                if (mNounObject != null)
                {
                    mNounObject.NRemoveParentIfNot(this);
                    mNounObject.NSetParent(this);
                    mKindsOfDTNodePropertiesDict.Add(mNounObject, KindOfDTNodeProperty.NounObject);
                    mKindsOfDTChildDict.Add(mNounObject, KindOfDTChild.Object);
                }
            }
        }

        private AdjectiveDTNode mAdjectiveObject;
        public AdjectiveDTNode AdjectiveObject
        {
            get
            {
                return mAdjectiveObject;
            }

            set
            {
                if (mAdjectiveObject == value)
                {
                    return;
                }

                if (mAdjectiveObject != null)
                {
                    mAdjectiveObject.NRemoveParent(this);
                }

                mAdjectiveObject = value;

                if (mAdjectiveObject != null)
                {
                    mAdjectiveObject.NRemoveParentIfNot(this);
                    mAdjectiveObject.NSetParent(this);
                    mKindsOfDTNodePropertiesDict.Add(mAdjectiveObject, KindOfDTNodeProperty.AdjectiveObject);
                    mKindsOfDTChildDict.Add(mAdjectiveObject, KindOfDTChild.Object);
                }
            }
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
                    if(obj.IsNounDTNode)
                    {
                        NounObject = obj.AsNounDTNode;
                        return;
                    }

                    if (obj.IsAdjectiveDTNode)
                    {
                        AdjectiveObject = obj.AsAdjectiveDTNode;
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
                case KindOfDTNodeProperty.NounObject:
                    mNounObject = null;
                    break;

                case KindOfDTNodeProperty.AdjectiveObject:
                    mAdjectiveObject = null;
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
            if (NounObject == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounObject)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounObject)}");
                sb.Append(NounObject.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NounObject)}");
            }
            if (AdjectiveObject == null)
            {
                sb.AppendLine($"{spaces}{nameof(AdjectiveObject)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AdjectiveObject)}");
                sb.Append(AdjectiveObject.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(AdjectiveObject)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));

            if (NounObject == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounObject)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounObject)}");
                sb.Append(NounObject.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(NounObject)}");
            }

            if (AdjectiveObject == null)
            {
                sb.AppendLine($"{spaces}{nameof(AdjectiveObject)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(AdjectiveObject)}");
                sb.Append(AdjectiveObject.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(AdjectiveObject)}");
            }

            return sb.ToString();
        }
    }
}
