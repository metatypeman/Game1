using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public abstract class BaseATNParsingNode
    {
        protected BaseATNParsingNode(ContextOfATNParsing context)
        {
            Context = context;
        }

        protected ContextOfATNParsing Context { get; private set; }
        protected IList<GoalOfATNExtendToken> GetGoals(ATNExtendedToken extendedToken)
        {
#if DEBUG
            LogInstance.Log($"extendedToken = {extendedToken}");
#endif

            var result = new List<GoalOfATNExtendToken>();

            var extendedTokenLind = extendedToken.Kind;
            var partOfSpeech = extendedToken.PartOfSpeech;

            switch (extendedTokenLind)
            {
                case KindOfATNToken.Word:
                    switch(partOfSpeech)
                    {
                        case GrammaticalPartOfSpeech.Noun:
                            result.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Pronoun:
                            result.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Adjective:
                            result.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Verb:
                            {
                                if(extendedToken.IsGerund)
                                {
                                    result.Add(GoalOfATNExtendToken.Ving);
                                }
                                else
                                {
                                    var verbType = extendedToken.VerbType;
                                    switch (verbType)
                                    {
                                        case VerbType.BaseForm:
                                            result.Add(GoalOfATNExtendToken.BaseV);
                                            break;

                                        case VerbType.Form_2:
                                            result.Add(GoalOfATNExtendToken.V2f);
                                            break;

                                        case VerbType.Form_3:
                                            result.Add(GoalOfATNExtendToken.V3f);
                                            break;
                                    }
                                    if(extendedToken.IsFormOfToDo)
                                    {
                                        result.Add(GoalOfATNExtendToken.FToDo);
                                    }
                                    else
                                    {
                                        if(extendedToken.IsFormOfToHave)
                                        {
                                            result.Add(GoalOfATNExtendToken.FToHave);
                                        }
                                        else
                                        {
                                            var content = extendedToken.Content;

                                            if (extendedToken.IsFormOfToBe)
                                            {
                                                if(content == "will")
                                                {
                                                    result.Add(GoalOfATNExtendToken.Will);
                                                }
                                                else
                                                {
                                                    if(content == "would")
                                                    {
                                                        throw new NotImplementedException();
                                                    }
                                                    else
                                                    {
                                                        if(content == "shell")
                                                        {
                                                            throw new NotImplementedException();
                                                        }
                                                        else
                                                        {
                                                            if(content == "should")
                                                            {
                                                                throw new NotImplementedException();
                                                            }
                                                            else
                                                            {
                                                                throw new NotImplementedException();
                                                            }
                                                        }
                                                    }                                                 
                                                }
                                            }
                                            else
                                            {
                                                throw new NotImplementedException();
                                            }                   
                                        }
                                    }                              
                                }                             
                            }
                            break;

                        case GrammaticalPartOfSpeech.Adverb:
                            result.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Preposition:
                            result.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Conjunction:
                            throw new NotImplementedException();

                        case GrammaticalPartOfSpeech.Interjection:
                            throw new NotImplementedException();

                        case GrammaticalPartOfSpeech.Article:
                            result.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Numeral:
                            result.Add(GoalOfATNExtendToken.NP);
                            break;

                        default: throw new ArgumentOutOfRangeException(nameof(partOfSpeech), partOfSpeech, null);
                    }
                    break;
            }

            return result;
        }
    }
}
