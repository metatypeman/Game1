using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public class ATNExtendedToken : IObjectToString
    {
        public KindOfATNToken Kind { get; set; } = KindOfATNToken.Unknown;
        public string Content { get; set; } = string.Empty;
        public int Pos { get; set; }
        public int Line { get; set; }
        public GrammaticalPartOfSpeech PartOfSpeech { get; set; } = GrammaticalPartOfSpeech.Undefined;
        public bool IsName { get; set; }
        public bool IsShortForm { get; set; }
        public GrammaticalGender Gender { get; set; } = GrammaticalGender.Neuter;
        public GrammaticalNumberOfWord Number { get; set; } = GrammaticalNumberOfWord.Neuter;
        public bool IsCountable { get; set; }
        public bool IsGerund { get; set; }
        public bool IsPossessive { get; set; }
        public GrammaticalPerson Person { get; set; } = GrammaticalPerson.Neuter;
        public TypeOfPronoun TypeOfPronoun { get; set; } = TypeOfPronoun.Undefined;
        public CaseOfPersonalPronoun CaseOfPersonalPronoun { get; set; } = CaseOfPersonalPronoun.Undefined;
        public VerbType VerbType { get; set; } = VerbType.BaseForm;
        public GrammaticalTenses Tense { get; set; } = GrammaticalTenses.All;
        public bool IsModal { get; set; }
        public bool IsFormOfToBe { get; set; }
        public bool IsFormOfToHave { get; set; }
        public bool IsFormOfToDo { get; set; }
        public GrammaticalComparison Comparison { get; set; } = GrammaticalComparison.None;
        public bool IsQuestionWord { get; set; }
        public bool IsDeterminer { get; set; }
        public NumeralType NumeralType { get; set; } = NumeralType.Undefined;
        public IList<string> LogicalMeaning { get; set; }
        public IList<string> FullLogicalMeaning { get; set; }
        public string RootWord { get; set; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextNSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(Content)} = {Content}");
            sb.AppendLine($"{spaces}{nameof(Pos)} = {Pos}");
            sb.AppendLine($"{spaces}{nameof(Line)} = {Line}");
            sb.AppendLine($"{spaces}{nameof(PartOfSpeech)} = {PartOfSpeech}");
            sb.AppendLine($"{spaces}{nameof(IsName)} = {IsName}");
            sb.AppendLine($"{spaces}{nameof(IsShortForm)} = {IsShortForm}");
            sb.AppendLine($"{spaces}{nameof(Gender)} = {Gender}");
            sb.AppendLine($"{spaces}{nameof(Number)} = {Number}");
            sb.AppendLine($"{spaces}{nameof(IsCountable)} = {IsCountable}");
            sb.AppendLine($"{spaces}{nameof(IsGerund)} = {IsGerund}");
            sb.AppendLine($"{spaces}{nameof(IsPossessive)} = {IsPossessive}");
            sb.AppendLine($"{spaces}{nameof(Person)} = {Person}");
            sb.AppendLine($"{spaces}{nameof(TypeOfPronoun)} = {TypeOfPronoun}");
            sb.AppendLine($"{spaces}{nameof(CaseOfPersonalPronoun)} = {CaseOfPersonalPronoun}");
            sb.AppendLine($"{spaces}{nameof(VerbType)} = {VerbType}");
            sb.AppendLine($"{spaces}{nameof(Tense)} = {Tense}");
            sb.AppendLine($"{spaces}{nameof(IsModal)} = {IsModal}");
            sb.AppendLine($"{spaces}{nameof(IsFormOfToBe)} = {IsFormOfToBe}");
            sb.AppendLine($"{spaces}{nameof(IsFormOfToHave)} = {IsFormOfToHave}");
            sb.AppendLine($"{spaces}{nameof(IsFormOfToDo)} = {IsFormOfToDo}");
            sb.AppendLine($"{spaces}{nameof(Comparison)} = {Comparison}");
            sb.AppendLine($"{spaces}{nameof(IsQuestionWord)} = {IsQuestionWord}");
            sb.AppendLine($"{spaces}{nameof(IsDeterminer)} = {IsDeterminer}");
            sb.AppendLine($"{spaces}{nameof(NumeralType)} = {NumeralType}");
            if (LogicalMeaning == null)
            {
                sb.AppendLine($"{spaces}{nameof(LogicalMeaning)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(LogicalMeaning)}");
                foreach (var item in LogicalMeaning)
                {
                    sb.AppendLine($"{nextNSpaces}{item}");
                }
                sb.AppendLine($"{spaces}End {nameof(LogicalMeaning)}");
            }

            if (FullLogicalMeaning == null)
            {
                sb.AppendLine($"{spaces}{nameof(FullLogicalMeaning)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(FullLogicalMeaning)}");
                foreach (var item in FullLogicalMeaning)
                {
                    sb.AppendLine($"{nextNSpaces}{item}");
                }
                sb.AppendLine($"{spaces}End {nameof(FullLogicalMeaning)}");
            }
            sb.AppendLine($"{spaces}{nameof(RootWord)} = {RootWord}");
            
            return sb.ToString();
        }
    }
}
