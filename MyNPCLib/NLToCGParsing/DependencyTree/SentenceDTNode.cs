using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class SentenceDTNode : BaseDTNode
    {
        public override bool IsSentenceDTNode => true;
        public override SentenceDTNode AsSentenceDTNode => this;

        public GrammaticalAspect Aspect { get; set; } = GrammaticalAspect.Undefined;
        public GrammaticalTenses Tense { get; set; } = GrammaticalTenses.Undefined;
        public GrammaticalVoice Voice { get; set; } = GrammaticalVoice.Undefined;
        public GrammaticalMood Mood { get; set; } = GrammaticalMood.Undefined;
        public KindOfModal Modal { get; set; } = KindOfModal.Undefined;
        //public VerbDTNode Verb { get; set; }

        //public override void SetObject(BaseDTNode obj)
        //{
        //    throw new NotImplementedException();
        //}

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
            //if(Verb == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(Verb)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(Verb)}");
            //    sb.Append(Verb.ToString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(Verb)}");
            //}
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
            //if (Verb == null)
            //{
            //    sb.AppendLine($"{spaces}{nameof(Verb)} = null");
            //}
            //else
            //{
            //    sb.AppendLine($"{spaces}Begin {nameof(Verb)}");
            //    sb.Append(Verb.ToShortString(nextN));
            //    sb.AppendLine($"{spaces}End {nameof(Verb)}");
            //}
            return sb.ToString();
        }
    }
}
