using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class InternalVisionItem : IObjectToString
    {
        public Vector3 LocalDirection { get; set; }
        public Vector3 Point { get; set; }
        public float Distance { get; set; }
        public int InstanceID { get; set; }

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
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(LocalDirection)} = {LocalDirection}");
            sb.AppendLine($"{spaces}{nameof(Point)} = {Point}");
            sb.AppendLine($"{spaces}{nameof(Distance)} = {Distance}");
            sb.AppendLine($"{spaces}{nameof(InstanceID)} = {InstanceID}");
            return sb.ToString();
        }
    }
}
