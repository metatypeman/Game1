using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class HostVisionObject: IHostVisionObject
    {
        public ulong EntityId { get; set; }
        public IList<IVisionItem> VisionItems { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(EntityId)} = {EntityId}");
            if (VisionItems == null)
            {
                sb.AppendLine($"{spaces}{nameof(VisionItems)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VisionItems)}");
                foreach (var visionItem in VisionItems)
                {
                    sb.Append(visionItem.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(VisionItems)}");
            }
            return sb.ToString();
        }
    }
}
