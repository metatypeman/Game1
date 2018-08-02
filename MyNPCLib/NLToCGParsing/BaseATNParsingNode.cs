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
            //LogInstance.Log($"Context.State = {Context.State}");
            //LogInstance.Log($"Context = {Context}");
            //LogInstance.Log($"CompositionCommand = {CompositionCommand}");
            //LogInstance.Log($"Goal = {Goal}");
#endif
        }

        protected ContextOfATNParsing Context { get; private set; }
        protected CompositionCommand CompositionCommand { get; set; }
        protected GoalOfATNExtendToken Goal { get; private set; }

        public void Run()
        {
            NormalizeCompositionCommand();
            ImplementInternalState();
            if(!SuppressBornNewNodes)
            {
                BornNewNodes();
            }
            
            ProcessTasks();
        }

        protected abstract void NormalizeCompositionCommand();
        protected abstract void ImplementInternalState();
        protected abstract void BornNewNodes();
        protected bool SuppressBornNewNodes { get; set; }

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
                var resultOfGetGoals = GetGoals(extendedToken);
                var goalsList = resultOfGetGoals.Goals;

                foreach (var goal in goalsList)
                {
                    result.Add(new KeyValuePair<ATNExtendedToken, GoalOfATNExtendToken>(extendedToken, goal));
                }
            }
            return result;
        }

        protected ResultOfGetGoals GetGoals(ATNExtendedToken extendedToken)
        {
#if DEBUG
            //LogInstance.Log($"extendedToken = {extendedToken}");
#endif

            var result = new ResultOfGetGoals();
            result.ExtendedToken = extendedToken;
            var resultList = new List<GoalOfATNExtendToken>();
            result.Goals = resultList;

            var extendedTokenLind = extendedToken.Kind;
            var partOfSpeech = extendedToken.PartOfSpeech;

            switch (extendedTokenLind)
            {
                case KindOfATNToken.Word:
                    switch(partOfSpeech)
                    {
                        case GrammaticalPartOfSpeech.Noun:
                            resultList.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Pronoun:
                            resultList.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Adjective:
                            resultList.Add(GoalOfATNExtendToken.AP);
                            break;

                        case GrammaticalPartOfSpeech.Verb:
                            {
                                if(extendedToken.IsGerund)
                                {
                                    resultList.Add(GoalOfATNExtendToken.Ving);
                                    resultList.Add(GoalOfATNExtendToken.NP);
                                }
                                else
                                {
                                    var verbType = extendedToken.VerbType;
                                    switch (verbType)
                                    {
                                        case VerbType.BaseForm:
                                            resultList.Add(GoalOfATNExtendToken.BaseV);
                                            break;

                                        case VerbType.Form_2:
                                            resultList.Add(GoalOfATNExtendToken.V2f);
                                            break;

                                        case VerbType.Form_3:
                                            resultList.Add(GoalOfATNExtendToken.V3f);
                                            break;
                                    }
                                    if(extendedToken.IsFormOfToDo)
                                    {
                                        resultList.Add(GoalOfATNExtendToken.FToDo);
                                    }
                                    else
                                    {
                                        if(extendedToken.IsFormOfToHave)
                                        {
                                            resultList.Add(GoalOfATNExtendToken.FToHave);
                                        }
                                        else
                                        {
                                            var content = extendedToken.Content;

                                            if (extendedToken.IsFormOfToBe)
                                            {
                                                if(content == "will")
                                                {
                                                    resultList.Add(GoalOfATNExtendToken.Will);
                                                }
                                                else
                                                {
                                                    if(content == "would")
                                                    {
                                                        resultList.Add(GoalOfATNExtendToken.Would);
                                                    }
                                                    else
                                                    {
                                                        if(content == "shell")
                                                        {
                                                            resultList.Add(GoalOfATNExtendToken.Shell);
                                                        }
                                                        else
                                                        {
                                                            if(content == "should")
                                                            {
                                                                resultList.Add(GoalOfATNExtendToken.Should);
                                                            }
                                                            else
                                                            {
                                                                if(content == "be")
                                                                {
                                                                    resultList.Add(GoalOfATNExtendToken.Be);
                                                                }
                                                                else
                                                                {
                                                                    resultList.Add(GoalOfATNExtendToken.FToBe);
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
                                                    resultList.Add(GoalOfATNExtendToken.FToDo);
                                                }
                                                else
                                                {
                                                    if(extendedToken.IsFormOfToHave)
                                                    {
                                                        resultList.Add(GoalOfATNExtendToken.FToHave);
                                                    }
                                                    else
                                                    {
                                                        if(content == "can")
                                                        {
                                                            resultList.Add(GoalOfATNExtendToken.Can);
                                                        }
                                                        else
                                                        {
                                                            if (content == "could")
                                                            {
                                                                resultList.Add(GoalOfATNExtendToken.Could);
                                                            }
                                                            else
                                                            {
                                                                if (content == "must")
                                                                {
                                                                    resultList.Add(GoalOfATNExtendToken.Must);
                                                                }
                                                                else
                                                                {
                                                                    if (content == "may")
                                                                    {
                                                                        resultList.Add(GoalOfATNExtendToken.May);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (content == "might")
                                                                        {
                                                                            resultList.Add(GoalOfATNExtendToken.Might);
                                                                        }
                                                                        else
                                                                        {
                                                                            if(content == "let")
                                                                            {
                                                                                resultList.Add(GoalOfATNExtendToken.Let);
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
                            throw new NotImplementedException();
                        //resultList.Add(GoalOfATNExtendToken.NP);
                        //break;

                        case GrammaticalPartOfSpeech.Preposition:
                            resultList.Add(GoalOfATNExtendToken.PP);
                            break;

                        case GrammaticalPartOfSpeech.Conjunction:
                            throw new NotImplementedException();

                        case GrammaticalPartOfSpeech.Interjection:
                            throw new NotImplementedException();

                        case GrammaticalPartOfSpeech.Article:
                            resultList.Add(GoalOfATNExtendToken.NP);
                            break;

                        case GrammaticalPartOfSpeech.Numeral:
                            throw new NotImplementedException();
                        //resultList.Add(GoalOfATNExtendToken.NP);
                        //break;

                        default: throw new ArgumentOutOfRangeException(nameof(partOfSpeech), partOfSpeech, null);
                    }
                    break;

                case KindOfATNToken.Point:
                    resultList.Add(GoalOfATNExtendToken.Point);
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
            //LogInstance.Log($"mTasksList.Count = {mTasksList.Count}");
#endif

            var n = 0;

            foreach(var task in mTasksList)
            {
                n++;

#if DEBUG
                //LogInstance.Log($"n = {n}");
                //LogInstance.Log($"task.Goal = {task.Goal}");
                //LogInstance.Log($"Context.State = {Context.State}");
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

        protected void PutSentenceToResult()
        {
#if DEBUG
            //LogInstance.Log("Begin");
#endif

            Context.PutSentenceToResult();

#if DEBUG
            //LogInstance.Log("End");
#endif
        }
    }
}
