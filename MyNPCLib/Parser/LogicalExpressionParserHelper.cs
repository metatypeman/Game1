using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public static class LogicalExpressionParserHelper
    {
        public static BaseQueryASTNode CreateNode(ParserContext context, TokenKind? closingToken = null)
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
                case TokenKind.Word:
                    context.Recovery(currentToken);
                    return CreateConditionNode(context, closingToken);

                case TokenKind.OpenRoundBracket:
                    return CreateOpenRoundBracketNode(context);

                case TokenKind.Not:
                    return CreateNotNode(context);

                default: throw new UnexpectedTokenException(currentToken);
            }
        }

        private static BaseQueryASTNode CreateNotNode(ParserContext context)
        {
            var parser = new NotNodeParser(context);
            parser.Run();
            return parser.Result;
        }

        private static BaseQueryASTNode CreateOpenRoundBracketNode(ParserContext context)
        {
            var parser = new OpenRoundBracketNodeParser(context);
            parser.Run();
            return parser.Result;
        }

        private static BaseQueryASTNode CreateConditionNode(ParserContext context, TokenKind? closingToken)
        {
            var parser = new ConditionNodeParser(context, closingToken);
            parser.Run();
            return parser.Result;
        }
    }
}
