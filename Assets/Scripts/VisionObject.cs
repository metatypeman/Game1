using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class VisionObject : IObjectToString
    {
        public int InstanceID { get; set; }
        public GameObject GameObject { get; set; }
        public IReadOnlyLogicalObject LogicalObject { get; set; }
        public List<VisionItem> VisionItems { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(InstanceID)} = {InstanceID}");
            if (GameObject == null)
            {
                sb.AppendLine($"{spaces}{nameof(GameObject)} = null");
            }
            else
            {
                sb.Append($"{spaces}{nameof(GameObject)} = {GameObjectHelper.GameObjectToString(GameObject, nextN)}");
            }

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
