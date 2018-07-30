using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLToCGParsing.DependencyTree
{
    public class VerbDTNode : IObjectToString, IShortObjectToString
    {
        public ATNExtendedToken VerbExtendedToken { get; set; }

        public IList<NounDTNode> NounSubjectsList { get; set; } = new List<NounDTNode>();

        public IList<NounDTNode> NounObjectsList { get; set; } = new List<NounDTNode>();
        public IList<PrepositionalDTNode> PrepositionalObjectsList { get; set; } = new List<PrepositionalDTNode>();

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
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (VerbExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(VerbExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VerbExtendedToken)}");
                sb.Append(VerbExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(VerbExtendedToken)}");
            }
            if (NounSubjectsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounSubjectsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounSubjectsList)}");
                foreach(var item in NounSubjectsList)
                {
                    sb.Append(item.ToString(nextN));
                }   
                sb.AppendLine($"{spaces}End {nameof(NounSubjectsList)}");
            }
            if (NounObjectsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounObjectsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounObjectsList)}");
                foreach (var item in NounObjectsList)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(NounObjectsList)}");
            }
            if (PrepositionalObjectsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(PrepositionalObjectsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(PrepositionalObjectsList)}");
                foreach (var item in PrepositionalObjectsList)
                {
                    sb.Append(item.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(PrepositionalObjectsList)}");
            }
            return sb.ToString();
        }

        public string ToShortString()
        {
            return ToShortString(0u);
        }

        public string ToShortString(uint n)
        {
            return this.GetDefaultToShortStringInformation(n);
        }

        public string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            if (VerbExtendedToken == null)
            {
                sb.AppendLine($"{spaces}{nameof(VerbExtendedToken)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VerbExtendedToken)}");
                sb.Append(VerbExtendedToken.ToString(nextN));
                sb.AppendLine($"{spaces}End {nameof(VerbExtendedToken)}");
            }

            if (NounSubjectsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounSubjectsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounSubjectsList)}");
                foreach (var item in NounSubjectsList)
                {
                    sb.Append(item.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(NounSubjectsList)}");
            }

            if (NounObjectsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(NounObjectsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NounObjectsList)}");
                foreach (var item in NounObjectsList)
                {
                    sb.Append(item.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(NounObjectsList)}");
            }

            if (PrepositionalObjectsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(PrepositionalObjectsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(PrepositionalObjectsList)}");
                foreach (var item in PrepositionalObjectsList)
                {
                    sb.Append(item.ToShortString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(PrepositionalObjectsList)}");
            }

            return sb.ToString();
        }
    }
}
