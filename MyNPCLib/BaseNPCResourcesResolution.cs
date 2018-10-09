using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public enum NPCResourcesResolutionKind
    {
        Unknow,
        Allow,
        AllowAdd,
        Forbiden
    }

    public enum NPCResourceKind
    {
        Unknown,
        Body,
        Hand
    }

    public abstract class BaseNPCResourcesResolution : IObjectToString
    {
        public abstract NPCResourceKind Kind { get; }
        public NPCResourcesResolutionKind KindOfResult { get; set; } = NPCResourcesResolutionKind.Unknow;
        public ulong TargetProcessId { get; set; }

        public virtual NPCBodyResourcesResolution ToBodyResourcesResulution()
        {
            return null;
        }

        public virtual NPCHandResourcesResolution ToHandResourcesResulution()
        {
            return null;
        }

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
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            sb.AppendLine($"{spaces}{nameof(KindOfResult)} = {KindOfResult}");
            sb.AppendLine($"{spaces}{nameof(TargetProcessId)} = {TargetProcessId}");
            return sb.ToString();
        }
    }
}
