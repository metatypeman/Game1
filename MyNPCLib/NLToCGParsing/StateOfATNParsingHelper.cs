using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public static class StateOfATNParsingHelper
    {
        public static StateOfATNParsing CreareState(StateOfATNParsing state, GoalOfATNExtendToken goal)
        {
#if DEBUG
            //LogInstance.Log($"state = {state}");
            //LogInstance.Log($"goal = {goal}");
#endif

            switch(state)
            {
                case StateOfATNParsing.Undefined:
                    switch(goal)
                    {
                        case GoalOfATNExtendToken.NP:
                            return StateOfATNParsing.NP;

                        case GoalOfATNExtendToken.BaseV:
                            return StateOfATNParsing.ImperativeVP;

                        default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
                    }

                case StateOfATNParsing.NP:
                    switch (goal)
                    {
                        case GoalOfATNExtendToken.BaseV:
                            return StateOfATNParsing.NP_VP;

                        case GoalOfATNExtendToken.NP:
                            return StateOfATNParsing.NP;

                        default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
                    }

                case StateOfATNParsing.NP_VP:
                    switch (goal)
                    {
                        case GoalOfATNExtendToken.BaseV:
                            return StateOfATNParsing.NP_VP;

                        case GoalOfATNExtendToken.NP:
                            return StateOfATNParsing.NP_VP;

                        default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
                    }

                case StateOfATNParsing.ImperativeVP:
                    switch (goal)
                    {
                        case GoalOfATNExtendToken.BaseV:
                            return StateOfATNParsing.ImperativeVP;

                        case GoalOfATNExtendToken.PP:
                            return StateOfATNParsing.ImperativeVP;

                        case GoalOfATNExtendToken.AP:
                            return StateOfATNParsing.ImperativeVP;

                        case GoalOfATNExtendToken.NP:
                            return StateOfATNParsing.ImperativeVP;

                        default: throw new ArgumentOutOfRangeException(nameof(goal), goal, null);
                    }

                default: throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
