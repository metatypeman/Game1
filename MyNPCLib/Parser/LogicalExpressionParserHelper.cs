using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public static class LogicalExpressionParserHelper
    {
        public static BaseQueryASTNode CreateNode(ParserContext context)
        {
            var currentToken = context.GetToken();

#if DEBUG
            LogInstance.Log($"LogicalExpressionParserHelper CreateNode currentToken = {currentToken}");
#endif

            if(currentToken == null)
            {
                return null;
            }

            context.Recovery(currentToken);

            switch (currentToken.TokenKind)
            {
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
    }
}
