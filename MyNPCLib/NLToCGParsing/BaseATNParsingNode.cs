using MyNPCLib.SimpleWordsDict;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public abstract class BaseATNParsingNode
    {
        protected BaseATNParsingNode(GoalOfATNExtendToken goal, CompositionCommand compositionCommand, ContextOfATNParsing context)
        {
            Context = context;
            CompositionCommand = compositionCommand;
            Goal = goal;

#if DEBUG
            LogInstance.Log($"Context.State = {Context.State}");
            LogInstance.Log($"Context = {Context}");
            LogInstance.Log($"CompositionCommand = {CompositionCommand}");
            LogInstance.Log($"Goal = {Goal}");
#endif
        }

        protected ContextOfATNParsing Context { get; private set; }
        protected CompositionCommand CompositionCommand { get; set; }
        protected GoalOfATNExtendToken Goal { get; private set; }

        public void Run()
        {
            NormalizeCompositionCommand();
            ImplementInternalState();
            BornNewNodes();
            ProcessTasks();
        }

        protected abstract void NormalizeCompositionCommand();
        protected abstract void ImplementInternalState();
        protected abstract void BornNewNodes();

        protected IList<KeyValuePair<ATNExtendedToken, GoalOfATNExtendToken>> GetСlusterOfExtendedTokensWithGoals()
        {
            var сlusterOfExtendedTokens = Context.GetСlusterOfExtendedTokens();

            var result = new List<KeyValuePair<ATNExtendedToken, GoalOfATNExtendToken>>();

            if (сlusterOfExtendedTokens.IsEmpty())
            {
                return result;
            }

            foreach (var extendedToken in сlusterOfExtendedTokens)
            {
                var goalsList = GetGoals(extendedToken);

                foreach (var goal in goalsList)
                {
                    result.Add(new KeyValuePair<ATNExtendedToken, GoalOfATNExtendToken>(extendedToken, goal));
                }
            }
            return result;
        }

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
                                    result.Add(GoalOfATNExtendToken.NP);
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
                                                        result.Add(GoalOfATNExtendToken.Would);
                                                    }
                                                    else
                                                    {
                                                        if(content == "shell")
                                                        {
                                                            result.Add(GoalOfATNExtendToken.Shell);
                                                        }
                                                        else
                                                        {
                                                            if(content == "should")
                                                            {
                                                                result.Add(GoalOfATNExtendToken.Should);
                                                            }
                                                            else
                                                            {
                                                                if(content == "be")
                                                                {
                                                                    result.Add(GoalOfATNExtendToken.Be);
                                                                }
                                                                else
                                                                {
                                                                    result.Add(GoalOfATNExtendToken.FToBe);
                                                                }
                                                            }
                                                        }
                                                    }                                                 
                                                }
                                            }
                                            else
                                            {
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
                                                        if(content == "can")
                                                        {
                                                            result.Add(GoalOfATNExtendToken.Can);
                                                        }
                                                        else
                                                        {
                                                            if (content == "could")
                                                            {
                                                                result.Add(GoalOfATNExtendToken.Could);
                                                            }
                                                            else
                                                            {
                                                                if (content == "must")
                                                                {
                                                                    result.Add(GoalOfATNExtendToken.Must);
                                                                }
                                                                else
                                                                {
                                                                    if (content == "may")
                                                                    {
                                                                        result.Add(GoalOfATNExtendToken.May);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (content == "might")
                                                                        {
                                                                            result.Add(GoalOfATNExtendToken.Might);
                                                                        }
                                                                        else
                                                                        {
                                                                            if(content == "let")
                                                                            {
                                                                                result.Add(GoalOfATNExtendToken.Let);
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }                                         
                                                    }
                                                }                     
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

        private List<IATNNodeFactory> mTasksList = new List<IATNNodeFactory>();

        protected void AddTask(IATNNodeFactory factory)
        {
            mTasksList.Add(factory);
        }

        private void ProcessTasks()
        {
            if(mTasksList.Count == 0)
            {
                return;
            }

#if DEBUG
            LogInstance.Log($"mTasksList.Count = {mTasksList.Count}");
#endif

            var n = 0;

            foreach(var task in mTasksList)
            {
                n++;

#if DEBUG
                LogInstance.Log($"n = {n}");
                LogInstance.Log($"task.Goal = {task.Goal}");
                LogInstance.Log($"Context.State = {Context.State}");
#endif

                ContextOfATNParsing newConext = null;

                if(n < mTasksList.Count)
                {
                    newConext = Context.Fork();
                }
                else
                {
                    newConext = Context;
                }

                newConext.State = StateOfATNParsingHelper.CreareState(newConext.State, task.Goal);

                var node = task.Create(newConext);
                node.Run();
            }
        }
    }
}
