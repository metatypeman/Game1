using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CGStorage
{
    public abstract class BaseCGStorage: ICGStorage
    {
        public abstract KindOfCGStorage Kind { get; }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            return sb.ToString();
        }
    }
}
