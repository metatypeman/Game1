using MyNPCLib.NLToCGParsing;
using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CommonServiceGrammaticalElements
{
    public static class GrammaticalElementsHeper
    {
        public static string GetAspectName(GrammaticalAspect aspect)
        {
            switch (aspect)
            {
                case GrammaticalAspect.Undefined:
                    return string.Empty;

                case GrammaticalAspect.Simple:
                    return CGGramamaticalNamesOfConcepts.GrammaticalAspect_Simple;

                case GrammaticalAspect.Continuous:
                    return CGGramamaticalNamesOfConcepts.GrammaticalAspect_Continuous;

                case GrammaticalAspect.Perfect:
                    return CGGramamaticalNamesOfConcepts.GrammaticalAspect_Perfect;

                case GrammaticalAspect.PerfectContinuous:
                    return CGGramamaticalNamesOfConcepts.GrammaticalAspect_PerfectContinuous;

                default: throw new ArgumentOutOfRangeException(nameof(aspect), aspect, null);
            }
        }

        public static GrammaticalAspect GetAspectFromName(string aspectName)
        {
            if(string.IsNullOrWhiteSpace(aspectName))
            {
                return GrammaticalAspect.Undefined;
            }

            if(aspectName == CGGramamaticalNamesOfConcepts.GrammaticalAspect_Simple)
            {
                return GrammaticalAspect.Simple;
            }

            if (aspectName == CGGramamaticalNamesOfConcepts.GrammaticalAspect_Continuous)
            {
                return GrammaticalAspect.Continuous;
            }

            if (aspectName == CGGramamaticalNamesOfConcepts.GrammaticalAspect_Perfect)
            {
                return GrammaticalAspect.Perfect;
            }

            if (aspectName == CGGramamaticalNamesOfConcepts.GrammaticalAspect_PerfectContinuous)
            {
                return GrammaticalAspect.PerfectContinuous;
            }

            return GrammaticalAspect.Undefined;
        }

        public static string GetTenseName(GrammaticalTenses tense)
        {
            switch (tense)
            {
                case GrammaticalTenses.Undefined:
                    return string.Empty;

                case GrammaticalTenses.All:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_All;

                case GrammaticalTenses.Present:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_Present;

                case GrammaticalTenses.Past:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_Past;

                case GrammaticalTenses.Future:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_Future;

                case GrammaticalTenses.FutureInThePast:
                    return CGGramamaticalNamesOfConcepts.GrammaticalTenses_FutureInThePast;

                default: throw new ArgumentOutOfRangeException(nameof(tense), tense, null);
            }
        }

        public static GrammaticalTenses GetTenseFromName(string tenseName)
        {
            if(string.IsNullOrWhiteSpace(tenseName))
            {
                return GrammaticalTenses.Undefined;
            }

            if(tenseName == CGGramamaticalNamesOfConcepts.GrammaticalTenses_All)
            {
                return GrammaticalTenses.All;
            }

            if (tenseName == CGGramamaticalNamesOfConcepts.GrammaticalTenses_Present)
            {
                return GrammaticalTenses.Present;
            }

            if (tenseName == CGGramamaticalNamesOfConcepts.GrammaticalTenses_Past)
            {
                return GrammaticalTenses.Past;
            }

            if (tenseName == CGGramamaticalNamesOfConcepts.GrammaticalTenses_Future)
            {
                return GrammaticalTenses.Future;
            }

            if (tenseName == CGGramamaticalNamesOfConcepts.GrammaticalTenses_FutureInThePast)
            {
                return GrammaticalTenses.FutureInThePast;
            }

            return GrammaticalTenses.Undefined;
        }

        public static string GetVoiceName(GrammaticalVoice voice)
        {
            switch (voice)
            {
                case GrammaticalVoice.Undefined:
                    return string.Empty;

                case GrammaticalVoice.Active:
                    return CGGramamaticalNamesOfConcepts.GrammaticalVoice_Active;

                case GrammaticalVoice.Passive:
                    return CGGramamaticalNamesOfConcepts.GrammaticalVoice_Passive;

                default: throw new ArgumentOutOfRangeException(nameof(voice), voice, null);
            }
        }

        public static GrammaticalVoice GetVoiceFromName(string voiceName)
        {
            if(string.IsNullOrWhiteSpace(voiceName))
            {
                return GrammaticalVoice.Undefined;
            }

            if(voiceName == CGGramamaticalNamesOfConcepts.GrammaticalVoice_Active)
            {
                return GrammaticalVoice.Active;
            }

            if (voiceName == CGGramamaticalNamesOfConcepts.GrammaticalVoice_Passive)
            {
                return GrammaticalVoice.Passive;
            }

            return GrammaticalVoice.Undefined;
        }

        public static string GetMoodName(GrammaticalMood mood)
        {
            switch (mood)
            {
                case GrammaticalMood.Undefined:
                    return string.Empty;

                case GrammaticalMood.Indicative:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Indicative;

                case GrammaticalMood.Subjunctive:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Subjunctive;

                case GrammaticalMood.Imperative:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Imperative;

                case GrammaticalMood.Jussive:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Jussive;

                case GrammaticalMood.Potential:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Potential;

                case GrammaticalMood.Hypothetical:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Hypothetical;

                case GrammaticalMood.Hortative:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Hortative;

                case GrammaticalMood.Optative:
                    return CGGramamaticalNamesOfConcepts.GrammaticalMood_Optative;

                default: throw new ArgumentOutOfRangeException(nameof(mood), mood, null);
            }
        }

        public static GrammaticalMood GetMoodFromName(string moodName)
        {
            if(string.IsNullOrWhiteSpace(moodName))
            {
                return GrammaticalMood.Undefined;
            }

            if(moodName == CGGramamaticalNamesOfConcepts.GrammaticalMood_Indicative)
            {
                return GrammaticalMood.Indicative;
            }

            if (moodName == CGGramamaticalNamesOfConcepts.GrammaticalMood_Subjunctive)
            {
                return GrammaticalMood.Subjunctive;
            }

            if (moodName == CGGramamaticalNamesOfConcepts.GrammaticalMood_Imperative)
            {
                return GrammaticalMood.Imperative;
            }

            if (moodName == CGGramamaticalNamesOfConcepts.GrammaticalMood_Jussive)
            {
                return GrammaticalMood.Jussive;
            }

            if (moodName == CGGramamaticalNamesOfConcepts.GrammaticalMood_Potential)
            {
                return GrammaticalMood.Potential;
            }

            if (moodName == CGGramamaticalNamesOfConcepts.GrammaticalMood_Hypothetical)
            {
                return GrammaticalMood.Hypothetical;
            }

            if (moodName == CGGramamaticalNamesOfConcepts.GrammaticalMood_Hortative)
            {
                return GrammaticalMood.Hortative;
            }

            if (moodName == CGGramamaticalNamesOfConcepts.GrammaticalMood_Optative)
            {
                return GrammaticalMood.Optative;
            }

            return GrammaticalMood.Undefined;
        }

        public static string GetModalName(KindOfModal modal)
        {
            switch (modal)
            {
                case KindOfModal.Undefined:
                    return string.Empty;

                case KindOfModal.None:
                    return CGGramamaticalNamesOfConcepts.KindOfModal_None;

                case KindOfModal.Must:
                    return CGGramamaticalNamesOfConcepts.KindOfModal_Must;

                case KindOfModal.Can:
                    return CGGramamaticalNamesOfConcepts.KindOfModal_Can;

                case KindOfModal.May:
                    return CGGramamaticalNamesOfConcepts.KindOfModal_May;

                default: throw new ArgumentOutOfRangeException(nameof(modal), modal, null);
            }
        }

        public static KindOfModal GetModalFromName(string modalName)
        {
            if(string.IsNullOrWhiteSpace(modalName))
            {
                return KindOfModal.Undefined;
            }

            if(modalName == CGGramamaticalNamesOfConcepts.KindOfModal_None)
            {
                return KindOfModal.None;
            }

            if (modalName == CGGramamaticalNamesOfConcepts.KindOfModal_Must)
            {
                return KindOfModal.Must;
            }

            if (modalName == CGGramamaticalNamesOfConcepts.KindOfModal_Can)
            {
                return KindOfModal.Can;
            }

            if (modalName == CGGramamaticalNamesOfConcepts.KindOfModal_May)
            {
                return KindOfModal.May;
            }

            return KindOfModal.Undefined;
        }

        public static KindOfGrammaticalRelation GetKindOfGrammaticalRelationFromName(string relationName)
        {
            if(string.IsNullOrWhiteSpace(relationName))
            {
                return KindOfGrammaticalRelation.Undefined;
            }

            if(relationName == CGGramamaticalNamesOfRelations.AspectName)
            {
                return KindOfGrammaticalRelation.Aspect;
            }

            if (relationName == CGGramamaticalNamesOfRelations.TenseName)
            {
                return KindOfGrammaticalRelation.Tense;
            }

            if (relationName == CGGramamaticalNamesOfRelations.VoiceName)
            {
                return KindOfGrammaticalRelation.Voice;
            }

            if (relationName == CGGramamaticalNamesOfRelations.MoodName)
            {
                return KindOfGrammaticalRelation.Mood;
            }

            if (relationName == CGGramamaticalNamesOfRelations.ModalName)
            {
                return KindOfGrammaticalRelation.Modal;
            }

            return KindOfGrammaticalRelation.Undefined;
        }

        public static bool IsEntityCondition(string relationName)
        {
            if(relationName == CGGramamaticalNamesOfRelations.EntityCondition)
            {
                return true;
            }

            return false;
        }
    }
}
