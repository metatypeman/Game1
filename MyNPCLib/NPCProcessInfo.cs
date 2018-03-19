using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyNPCLib
{
    public class NPCProcessInfo : IObjectToString
    {
        public NPCProcessStartupMode StartupMode { get; set; } = NPCProcessStartupMode.NewInstance;
        public string Name { get; set; }
        public ulong Key { get; set; }
        public Type Type { get; set; }
        public TypeInfo TypeInfo { get; set; }
        public List<NPCProcessEntryPointInfo> EntryPointsInfoList { get; set; } = new List<NPCProcessEntryPointInfo>();
        
        public override string ToString()
        {
            return ToString(0);
        }

        public string ToString(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}Begin {nameof(NPCProcessInfo)}");
            sb.Append(PropertiesToSting(n));
            sb.AppendLine($"{spaces}End {nameof(NPCProcessInfo)}");
            return sb.ToString();
        }

        public string PropertiesToSting(int n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(StartupMode)} = {StartupMode}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            sb.AppendLine($"{spaces}NameOfType = {Type.FullName}");
            if (EntryPointsInfoList == null)
            {
                sb.AppendLine($"{spaces}{nameof(EntryPointsInfoList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin{nameof(EntryPointsInfoList)}");
                foreach(var entryPiontInfo in EntryPointsInfoList)
                {
                    sb.Append(entryPiontInfo.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End{nameof(EntryPointsInfoList)}");
            }
            return sb.ToString();
        }
    }
}
