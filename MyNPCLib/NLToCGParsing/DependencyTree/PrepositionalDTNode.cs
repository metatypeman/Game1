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
            AdjectiveObject
        }

        public override bool IsPrepositionalDTNode => true;
        public override PrepositionalDTNode AsPrepositionalDTNode => this;

        //        public ATNExtendedToken PrepositionalExtendedToken { get; set; }

        //        private NounDTNode mNounObject;
        //        public NounDTNode NounObject
        //        {
        //            get
        //            {
        //                return mNounObject;
        //            }

        //            set
        //            {
        //                if(mNounObject == value)
        //                {
        //                    return;
        //                }

        //#if DEBUG
        //                LogInstance.Log("Begin");
        //#endif

        //                if(mNounObject != null)
        //                {
        //#if DEBUG
        //                    LogInstance.Log("mNounObject != null (1)");
        //#endif

        //                    throw new NotImplementedException();
        //                }

        //                mNounObject = value;

        //                if (mNounObject != null)
        //                {
        //#if DEBUG
        //                    LogInstance.Log("mNounObject != null (2)");
        //#endif

        //                    mNounObject.NSetParent(this);
        //                    mKindsOfDTNodePropertiesDict.Add(mAdjectiveObject, KindOfDTNodeProperty.NounObject);

        //#if DEBUG
        //                    LogInstance.Log($"mNounObject.Parent = {mNounObject.Parent}");
        //#endif
        //                }
        //            }
        //        }

        //        private AdjectiveDTNode mAdjectiveObject;
        //        public AdjectiveDTNode AdjectiveObject
        //        {
        //            get
        //            {
        //                return mAdjectiveObject;
        //            }

        //            set
        //            {
        //                if(mAdjectiveObject == value)
        //                {
        //                    return;
        //                }

        //#if DEBUG
        //                LogInstance.Log("Begin");
        //#endif

        //                if (mAdjectiveObject != null)
        //                {
        //#if DEBUG
        //                    LogInstance.Log("mAdjectiveObject != null (1)");
        //#endif

        //                    throw new NotImplementedException();
        //                }

        //                mAdjectiveObject = value;

        //                if (mAdjectiveObject != null)
        //                {
        //#if DEBUG
        //                    LogInstance.Log("mAdjectiveObject != null (2)");
        //#endif

        //                    mAdjectiveObject.NSetParent(this);
        //                    mKindsOfDTNodePropertiesDict.Add(mAdjectiveObject, KindOfDTNodeProperty.AdjectiveObject);

        //#if DEBUG
        //                    LogInstance.Log($"mAdjectiveObject.Parent = {mAdjectiveObject.Parent}");
        //#endif
        //                }
        //            }
        //        }

        //        public override void SetObject(BaseDTNode obj)
        //        {
        //#if DEBUG
        //            LogInstance.Log($"obj = {obj}");
        //#endif

        //            if(obj.IsNounDTNode)
        //            {
        //                NounObject = obj.AsNounDTNode;
        //                return;
        //            }

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
            //if (PrepositionalExtendedToken == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(PrepositionalExtendedToken)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(PrepositionalExtendedToken)}");
            //    sb.Append(PrepositionalExtendedToken.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(PrepositionalExtendedToken)}");
            //}

            //if (NounObject == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(NounObject)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(NounObject)}");
            //    sb.Append(NounObject.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(NounObject)}");
            //}
            //if (AdjectiveObject == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(AdjectiveObject)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(AdjectiveObject)}");
            //    sb.Append(AdjectiveObject.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(AdjectiveObject)}");
            //}
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));
            //if (PrepositionalExtendedToken == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(PrepositionalExtendedToken)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(PrepositionalExtendedToken)}");
            //    sb.Append(PrepositionalExtendedToken.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(PrepositionalExtendedToken)}");
            //}

            //if (NounObject == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(NounObject)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(NounObject)}");
            //    sb.Append(NounObject.ToShortString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(NounObject)}");
            //}

            //if (AdjectiveObject == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(AdjectiveObject)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(AdjectiveObject)}");
            //    sb.Append(AdjectiveObject.ToShortString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(AdjectiveObject)}");
            //}

            return sb.ToString();
        }
    }
}
