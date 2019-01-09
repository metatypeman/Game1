using MyNPCLib;
using MyNPCLib.NavigationSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class PointInfo: IPointInfo
    {
        public IRoute Route { get; set; }
        public IStepOfRoute StepOfRoute { get; set; }
        public bool IsFinal { get; set; }
        public Vector3? Position { get; set; }
        public IWayPoint WayPoint { get; set; }
        public IPlane Plane { get; set; }
        public bool IsFirstPartOfLink { get; set; }

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
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsFinal)} = {IsFinal}");
            sb.AppendLine($"{spaces}{nameof(Position)} = {Position}");
            sb.AppendLine($"{spaces}{nameof(WayPoint)} = {WayPoint?.Name}");
            sb.AppendLine($"{spaces}{nameof(Plane)} = {Plane?.Name}");
            sb.AppendLine($"{spaces}{nameof(IsFirstPartOfLink)} = {IsFirstPartOfLink}");
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

        public string PropertiesToShortString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsFinal)} = {IsFinal}");
            sb.AppendLine($"{spaces}{nameof(Position)} = {Position}");
            sb.AppendLine($"{spaces}{nameof(WayPoint)} = {WayPoint?.Name}");
            sb.AppendLine($"{spaces}{nameof(Plane)} = {Plane?.Name}");
            sb.AppendLine($"{spaces}{nameof(IsFirstPartOfLink)} = {IsFirstPartOfLink}");
            return sb.ToString();
        }

        public string ToBriefString()
        {
            return ToBriefString(0u);
        }

        public string ToBriefString(uint n)
        {
            return this.GetDefaultToBriefStringInformation(n);
        }

        public string PropertiesToBriefString(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsFinal)} = {IsFinal}");
            sb.AppendLine($"{spaces}{nameof(Position)} = {Position}");
            sb.AppendLine($"{spaces}{nameof(WayPoint)} = {WayPoint?.Name}");
            sb.AppendLine($"{spaces}{nameof(Plane)} = {Plane?.Name}");
            sb.AppendLine($"{spaces}{nameof(IsFirstPartOfLink)} = {IsFirstPartOfLink}");
            return sb.ToString();
        }
    }
}
