using MyNPCLib.CG;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.InternalCG
{
    public class InternalConceptualGraph: BaseInternalConceptCGNode
    {
        public override bool IsConceptualGraph => true;
        public override InternalConceptualGraph AsConceptualGraph => this;
        public override KindOfCGNode Kind => KindOfCGNode.Graph;
        public GrammaticalTenses Tense { get; set; } = GrammaticalTenses.Undefined;
        public GrammaticalAspect Aspect { get; set; } = GrammaticalAspect.Undefined;
        public GrammaticalVoice Voice { get; set; } = GrammaticalVoice.Undefined;
        public GrammaticalMood Mood { get; set; } = GrammaticalMood.Undefined;
        public bool? Conditional { get; set; }
        public KindOfModal Modal { get; set; } = KindOfModal.Undefined;

        private IList<BaseInternalCGNode> mChildren = new List<BaseInternalCGNode>();

        public IList<BaseInternalCGNode> Children
        {
            get
            {
                return mChildren.ToList();
            }
        }

        public override IList<ICGNode> ChildrenNodes => mChildren.Cast<ICGNode>().ToList();

        public void AddChild(BaseInternalCGNode child)
        {
            if (child == null)
            {
                return;
            }

            if (mChildren.Contains(child))
            {
                return;
            }

            NAddChild(child);
            child.NSetParent(this);
        }

        internal void NAddChild(BaseInternalCGNode child)
        {
            if (!mChildren.Contains(child))
            {
                mChildren.Add(child);
            }
        }

        public void RemoveChild(BaseInternalCGNode child)
        {
            if (child == null)
            {
                return;
            }

            if (!mChildren.Contains(child))
            {
                return;
            }

            NRemoveChild(child);
            NRemoveParent(this);
        }

        internal void NRemoveChild(BaseInternalCGNode child)
        {
            if (mChildren.Contains(child))
            {
                mChildren.Remove(child);
            }
        }

        public override void Destroy()
        {
            Parent = null;
            foreach(var child in Children)
            {
                RemoveChild(child);
            }
            foreach (var node in Inputs)
            {
                NSRemoveInputNode(node);
            }

            foreach (var node in Outputs)
            {
                NSRemoveOutputNode(node);
            }
        }

        private Dictionary<string, string> mRealtionLinkedVarNameDict = new Dictionary<string, string>();
        public void AddLinkRelationToVarName(string relationName, string varName)
        {
            mRealtionLinkedVarNameDict[relationName] = varName;
        }

        public string GetVarNameForRelation(string relationName)
        {
            if(mRealtionLinkedVarNameDict.ContainsKey(relationName))
            {
                return mRealtionLinkedVarNameDict[relationName];
            }

            return string.Empty;
        }

        public int MaxVarCount { get; set; }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpace = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(Number)} = {Number}");
            sb.AppendLine($"{spaces}{nameof(Tense)} = {Tense}");
            sb.AppendLine($"{spaces}{nameof(Aspect)} = {Aspect}");
            sb.AppendLine($"{spaces}{nameof(Conditional)} = {Conditional}");
            sb.AppendLine($"{spaces}{nameof(Voice)} = {Voice}");
            sb.AppendLine($"{spaces}{nameof(Modal)} = {Modal}");
            sb.AppendLine($"{spaces}{nameof(Mood)} = {Mood}");
            sb.AppendLine($"{spaces}Begin {nameof(Children)}");
            foreach (var child in Children)
            {
                sb.Append(child.PropertiesToShortSting(nextN));
            }
            sb.AppendLine($"{spaces}End {nameof(Children)}");
            sb.AppendLine($"{spaces}Begin RealtionLinkedVarNameDict");
            foreach (var relationLinkedVarKVPItem in mRealtionLinkedVarNameDict)
            {
                sb.AppendLine($"{nextNSpace}{relationLinkedVarKVPItem.Key} = {relationLinkedVarKVPItem.Value}");
            }
            sb.AppendLine($"{spaces}End RealtionLinkedVarNameDict");
            sb.AppendLine($"{spaces}{nameof(MaxVarCount)} = {MaxVarCount}");
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpace = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Number)} = {Number}");
            sb.AppendLine($"{spaces}{nameof(Tense)} = {Tense}");
            sb.AppendLine($"{spaces}{nameof(Aspect)} = {Aspect}");
            sb.AppendLine($"{spaces}{nameof(Conditional)} = {Conditional}");
            sb.AppendLine($"{spaces}{nameof(Voice)} = {Voice}");
            sb.AppendLine($"{spaces}{nameof(Modal)} = {Modal}");
            sb.AppendLine($"{spaces}{nameof(Mood)} = {Mood}");
            sb.AppendLine($"{spaces}Begin RealtionLinkedVarNameDict");
            foreach (var relationLinkedVarKVPItem in mRealtionLinkedVarNameDict)
            {
                sb.AppendLine($"{nextNSpace}{relationLinkedVarKVPItem.Key} = {relationLinkedVarKVPItem.Value}");
            }
            sb.AppendLine($"{spaces}End RealtionLinkedVarNameDict");
            sb.AppendLine($"{spaces}{nameof(MaxVarCount)} = {MaxVarCount}");
            sb.Append(base.PropertiesToShortSting(n));
            return sb.ToString();
        }
    }
}
