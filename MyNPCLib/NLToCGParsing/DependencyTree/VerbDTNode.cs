using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class VerbDTNode : BaseDTNode
    {
        private enum KindOfDTNodeProperty
        {
            PrepositionalObject
        }

        public override bool IsVerbDTNode => true;
        public override VerbDTNode AsVerbDTNode => this;

        private List<PrepositionalDTNode> mPrepositionalObjectsList = new List<PrepositionalDTNode>();

        public List<PrepositionalDTNode> PrepositionalObjectsList
        {
            get
            {
                return mPrepositionalObjectsList;
            }
        }

        public void AddPrepositionalObject(PrepositionalDTNode prepositionalDTNode)
        {
#if DEBUG
            LogInstance.Log($"prepositionalDTNode = {prepositionalDTNode}");
#endif

            if(prepositionalDTNode == null)
            {
                return;
            }

            if(mPrepositionalObjectsList.Contains(prepositionalDTNode))
            {
                return;
            }

            mPrepositionalObjectsList.Add(prepositionalDTNode);

            prepositionalDTNode.NRemoveParentIfNot(this);
            prepositionalDTNode.NSetParent(this);
            mKindsOfDTNodePropertiesDict.Add(prepositionalDTNode, KindOfDTNodeProperty.PrepositionalObject);
            mKindsOfDTChildDict.Add(prepositionalDTNode, KindOfDTChild.Object);
        }

        //public IList<NounDTNode> NounSubjectsList { get; set; } = new List<NounDTNode>();

        //public IList<NounDTNode> NounObjectsList { get; set; } = new List<NounDTNode>();
        //public IList<PrepositionalDTNode> PrepositionalObjectsList { get; set; } = new List<PrepositionalDTNode>();

        public override void SetValue(BaseDTNode obj, KindOfDTChild kindOfDTChild)
        {
#if DEBUG
            LogInstance.Log($"obj = {obj}");
            LogInstance.Log($"kindOfDTChild = {kindOfDTChild}");
#endif

            switch (kindOfDTChild)
            {
                case KindOfDTChild.Object:
                    if(obj.IsPrepositionalDTNode)
                    {
                        AddPrepositionalObject(obj.AsPrepositionalDTNode);
                        return;
                    }
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kindOfDTChild), kindOfDTChild, null);
            }
        }

        protected override void OnRemoveObjFromProp(BaseDTNode obj)
        {
#if DEBUG
            LogInstance.Log($"obj = {obj}");
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
                case KindOfDTNodeProperty.PrepositionalObject:
                    mPrepositionalObjectsList.Remove(obj.AsPrepositionalDTNode);
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
            //if (NounSubjectsList == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(NounSubjectsList)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(NounSubjectsList)}");
            //    foreach(var item in NounSubjectsList)
            //    {
            //        sb.Append(item.ToString(nextN));
            //    }   
            //    sb.AppendLine($"{spaces}End {nameof(NounSubjectsList)}");
            //}
            //if (NounObjectsList == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(NounObjectsList)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(NounObjectsList)}");
            //    foreach (var item in NounObjectsList)
            //    {
            //        sb.Append(item.ToString(nextN));
            //    }
            //    sb.AppendLine($"{spaces}End {nameof(NounObjectsList)}");
            //}
            if (PrepositionalObjectsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(PrepositionalObjectsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(PrepositionalObjectsList)}");
                foreach (var item in PrepositionalObjectsList)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(PrepositionalObjectsList)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));

            //if (NounSubjectsList == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(NounSubjectsList)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(NounSubjectsList)}");
            //    foreach (var item in NounSubjectsList)
            //    {
            //        sb.Append(item.ToShortString(nextN));
            //    }
            //    sb.AppendLine($"{spaces}End {nameof(NounSubjectsList)}");
            //}

            //if (NounObjectsList == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(NounObjectsList)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(NounObjectsList)}");
            //    foreach (var item in NounObjectsList)
            //    {
            //        sb.Append(item.ToShortString(nextN));
            //    }
            //    sb.AppendLine($"{spaces}End {nameof(NounObjectsList)}");
            //}

            if (PrepositionalObjectsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(PrepositionalObjectsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(PrepositionalObjectsList)}");
                foreach (var item in PrepositionalObjectsList)
                {
                    sb.Append(item.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(PrepositionalObjectsList)}");
            }

            return sb.ToString();
        }
    }
}
