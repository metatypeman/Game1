using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class PlanesConnectionInfo: IObjectToString
    {
        public bool IsDirect { get; set; }
        public IWayPoint WayPoint { get; set; }
        public List<ILinkOfWayPoints> LinksList { get; set; }

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
            var nextNSpaces = StringHelper.Spaces(nextN);

            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsDirect)} = {IsDirect}");
            sb.AppendLine($"{spaces}{nameof(WayPoint)} = {WayPoint?.Name}");

            if (LinksList == null)
            {
                sb.AppendLine($"{spaces}{nameof(LinksList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(LinksList)}");
                foreach (var item in LinksList)
                {
                    sb.AppendLine($"{nextNSpaces}{item.Name}");
                }
                sb.AppendLine($"{spaces}End {nameof(LinksList)}");
            }
            return sb.ToString();
        }
    }
}
