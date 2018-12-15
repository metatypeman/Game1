using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.NPCScripts.PixKeeper
{
    public class PixKeeperBlackBoard : BaseBlackBoard, IObjectToString
    {
        public HumanoidBodyCommand LastCommand { get; set; }
        public bool IsReadyForsoundCommandExecuting { get; set; }
        public string Name { get; set; } = "Jake";

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsReadyForsoundCommandExecuting)} = {IsReadyForsoundCommandExecuting}");
            sb.AppendLine($"{spaces}{nameof(Name)} = {Name}");
            return sb.ToString();
        }
    }
}
