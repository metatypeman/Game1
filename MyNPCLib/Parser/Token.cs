using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Parser
{
    /// <summary>
    /// Represents some fragment from code string.
    /// </summary>
    public class Token: IObjectToString
    {
        /// <summary>
        /// It represents by the instance of this class.
        /// </summary>
        public TokenKind TokenKind = TokenKind.Unknown;

        /// <summary>
        /// Which key word is represent by the instance of this class.
        /// </summary>
        public TokenKind KeyWordTokenKind = TokenKind.Unknown;

        /// <summary>
        /// The content of this fragment, is not empty if field TokenKind equal TokenKind.Word or TokenKind.Var. Else is empty.
        /// </summary>
        public string Content = string.Empty;

        public int Pos;
        public int Line;

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
