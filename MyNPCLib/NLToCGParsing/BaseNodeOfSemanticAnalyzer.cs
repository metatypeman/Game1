using MyNPCLib.CG;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing
{
    public abstract class BaseNodeOfSemanticAnalyzer
    {
        protected BaseNodeOfSemanticAnalyzer(ContextOfSemanticAnalyzer context)
        {
            Context = context;
        }

        protected ContextOfSemanticAnalyzer Context { get; private set; }
        protected RolesStorageOfSemanticAnalyzer PrimaryRolesDict { get; private set; } = new RolesStorageOfSemanticAnalyzer();

        protected string GetName(ATNExtendedToken extendedToken)
        {
            var rootWord = extendedToken.RootWord;

            if (string.IsNullOrWhiteSpace(rootWord))
            {
                return extendedToken.Content;
            }

            return rootWord;
        }
    }
}
