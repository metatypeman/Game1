using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionItem: IVisionItem
    {
        public Vector3 LocalDirection { get; set; }
        public Vector3 Point { get; set; }
        public float Distance { get; set; }

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
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(LocalDirection)} = {LocalDirection}");
            sb.AppendLine($"{spaces}{nameof(Point)} = {Point}");
            sb.AppendLine($"{spaces}{nameof(Distance)} = {Distance}");
            return sb.ToString();
        }
    }
}
