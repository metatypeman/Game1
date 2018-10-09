using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    public class Token : IObjectToString
    {
        public TokenKind TokenKind { get; set; } = TokenKind.Unknown;
        public TokenKind KeyWordTokenKind { get; set; } = TokenKind.Unknown;
        public string Content { get; set; } = string.Empty;
        public int Pos { get; set; }
        public int Line { get; set; }

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
            var tmpSb = new StringBuilder();

            tmpSb.AppendLine($"{spaces}{nameof(TokenKind)} = {TokenKind}");
            tmpSb.AppendLine($"{spaces}{nameof(KeyWordTokenKind)} = {KeyWordTokenKind}");
            tmpSb.AppendLine($"{spaces}{nameof(Content)} = {Content}");
            tmpSb.AppendLine($"{spaces}{nameof(Pos)} = {Pos}");
            tmpSb.AppendLine($"{spaces}{nameof(Line)} = {Line}");

            return tmpSb.ToString();
        }

        public string ToDebugString()
        {
            var tmpSb = new StringBuilder($"`{TokenKind}`({Line}, {Pos})");

            if (!string.IsNullOrWhiteSpace(Content))
            {
                tmpSb.Append($": `{Content}`");
            }

            return tmpSb.ToString();
        }
    }
}
