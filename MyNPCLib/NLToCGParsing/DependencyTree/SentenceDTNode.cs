using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class SentenceDTNode : BaseDTNode
    {
        private enum KindOfDTNodeProperty
        {
            Verb
        }

        public override bool IsSentenceDTNode => true;
        public override SentenceDTNode AsSentenceDTNode => this;
        
        public GrammaticalAspect Aspect { get; set; } = GrammaticalAspect.Undefined;
        public GrammaticalTenses Tense { get; set; } = GrammaticalTenses.Undefined;
        public GrammaticalVoice Voice { get; set; } = GrammaticalVoice.Undefined;
        public GrammaticalMood Mood { get; set; } = GrammaticalMood.Undefined;
        public KindOfModal Modal { get; set; } = KindOfModal.Undefined;

        private VerbDTNode mVerb;

        public VerbDTNode Verb
        {
            get
            {
                return mVerb;
            }

            set
            {
                if(mVerb == value)
                {
                    return;
                }

                if(mVerb != null)
                {
                    mVerb.NRemoveParent(this);
                }

                mVerb = value;

                if (mVerb != null)
                {
                    mVerb.NRemoveParentIfNot(this);
                    mVerb.NSetParent(this);
                    mKindsOfDTNodePropertiesDict.Add(mVerb, KindOfDTNodeProperty.Verb);
                    mKindsOfDTChildDict.Add(mVerb, KindOfDTChild.Root);
                }
            }
        }

        public override void SetValue(BaseDTNode obj, KindOfDTChild kindOfDTChild)
        {
#if DEBUG
            //LogInstance.Log($"obj = {obj}");
            //LogInstance.Log($"kindOfDTChild = {kindOfDTChild}");
#endif

            switch(kindOfDTChild)
            {
                default: throw new ArgumentOutOfRangeException(nameof(kindOfDTChild), kindOfDTChild, null);
            }
        }

        protected override void OnRemoveObjFromProp(BaseDTNode obj)
        {
            if(!mKindsOfDTNodePropertiesDict.ContainsKey(obj))
            {
                return;
            }

            var kinfOfDTNode = mKindsOfDTNodePropertiesDict[obj];
            mKindsOfDTNodePropertiesDict.Remove(obj);
            mKindsOfDTChildDict.Remove(obj);

            switch(kinfOfDTNode)
            {
                case KindOfDTNodeProperty.Verb:
                    mVerb = null;
                    break;

                default: throw new ArgumentOutOfRangeException(nameof(kinfOfDTNode), kinfOfDTNode, null);
            }
        }

        private Dictionary<BaseDTNode, KindOfDTNodeProperty> mKindsOfDTNodePropertiesDict = new Dictionary<BaseDTNode, KindOfDTNodeProperty>();
        private Dictionary<BaseDTNode, KindOfDTChild> mKindsOfDTChildDict = new Dictionary<BaseDTNode, KindOfDTChild>();

        public override KindOfDTChild GetKindOfDTChild(BaseDTNode obj)
        {
            if(obj == null)
            {
                return KindOfDTChild.Undefined;
            }

            if(mKindsOfDTChildDict.ContainsKey(obj))
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
            sb.AppendLine($"{spaces}{nameof(Aspect)} = {Aspect}");
            sb.AppendLine($"{spaces}{nameof(Tense)} = {Tense}");
            sb.AppendLine($"{spaces}{nameof(Voice)} = {Voice}");
            sb.AppendLine($"{spaces}{nameof(Modal)} = {Modal}");
            sb.AppendLine($"{spaces}{nameof(Mood)} = {Mood}");
            if (Verb == null)
            {
                sb.AppendLine($"{spaces}{nameof(Verb)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Verb)}");
                sb.Append(Verb.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Verb)}");
            }
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Aspect)} = {Aspect}");
            sb.AppendLine($"{spaces}{nameof(Tense)} = {Tense}");
            sb.AppendLine($"{spaces}{nameof(Voice)} = {Voice}");
            sb.AppendLine($"{spaces}{nameof(Modal)} = {Modal}");
            sb.AppendLine($"{spaces}{nameof(Mood)} = {Mood}");
            if (Verb == null)
            {
                sb.AppendLine($"{spaces}{nameof(Verb)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(Verb)}");
                sb.Append(Verb.ToShortString(nextN));
                sb.AppendLine($"{spaces}End {nameof(Verb)}");
            }
            return sb.ToString();
        }
    }
}
