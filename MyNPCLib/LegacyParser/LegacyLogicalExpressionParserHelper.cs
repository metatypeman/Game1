using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.LegacyParser
{
    public static class LegacyLogicalExpressionParserHelper
    {
        public static BaseQueryASTNode CreateNode(LegacyParserContext context, LegacyTokenKind? closingToken = null)
        {
            var currentToken = context.GetToken();

#if DEBUG
            //LogInstance.Log($"currentToken = {currentToken}");
#endif

            if(currentToken == null)
            {
                return null;
            }

            switch (currentToken.TokenKind)
            {
                case LegacyTokenKind.Word:
                    context.Recovery(currentToken);
                    return CreateConditionNode(context, closingToken);

                case LegacyTokenKind.OpenRoundBracket:
                    return CreateOpenRoundBracketNode(context);

                case LegacyTokenKind.Not:
                    return CreateNotNode(context);

                default: throw new LegacyUnexpectedTokenException(currentToken);
            }
        }

        private static BaseQueryASTNode CreateNotNode(LegacyParserContext context)
        {
            var parser = new LegacyNotNodeParser(context);
            parser.Run();
            return parser.Result;
        }

        private static BaseQueryASTNode CreateOpenRoundBracketNode(LegacyParserContext context)
        {
            var parser = new LegacyOpenRoundBracketNodeParser(context);
            parser.Run();
            return parser.Result;
        }

        private static BaseQueryASTNode CreateConditionNode(LegacyParserContext context, LegacyTokenKind? closingToken)
        {
            var parser = new LegacyConditionNodeParser(context, closingToken);
            parser.Run();
            return parser.Result;
        }
    }
}
