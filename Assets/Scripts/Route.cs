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
    public class Route : IRoute
    {
        public StatusOfRoute Status { get; set; } = StatusOfRoute.Unknown;
        public Vector3 TargetPosition { get; set; }
        public IList<IStepOfRoute> NextSteps { get; set; } = new List<IStepOfRoute>();
        public IList<IPointInfo> NextPoints { get; set; } = new List<IPointInfo>();
        public IList<IList<IPlane>> InitPathsList { get; set; }

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
            sb.AppendLine($"{spaces}{nameof(Status)} = {Status}");
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");

            if (NextSteps == null)
            {
                sb.AppendLine($"{spaces}{nameof(NextSteps)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NextSteps)}");
                foreach (var item in NextSteps)
                {
                    sb.Append($"{item.ToString(nextN)}");
                }
                sb.AppendLine($"{spaces}End {nameof(NextSteps)}");
            }

            if (NextPoints == null)
            {
                sb.AppendLine($"{spaces}{nameof(NextPoints)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NextPoints)}");
                foreach (var item in NextPoints)
                {
                    sb.Append($"{item.ToString(nextN)}");
                }
                sb.AppendLine($"{spaces}End {nameof(NextPoints)}");
            }

            if (InitPathsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(InitPathsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(InitPathsList)}");
                foreach (var item in InitPathsList)
                {
                    sb.AppendLine($"{nextNSpaces}{PathsHelper.DisplayPath(item)}");
                }
                sb.AppendLine($"{spaces}End {nameof(InitPathsList)}");
            }

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
            var nextNSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Status)} = {Status}");
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");

            if (NextSteps == null)
            {
                sb.AppendLine($"{spaces}{nameof(NextSteps)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NextSteps)}");
                foreach (var item in NextSteps)
                {
                    sb.Append($"{item.ToShortString(nextN)}");
                }
                sb.AppendLine($"{spaces}End {nameof(NextSteps)}");
            }

            if (NextPoints == null)
            {
                sb.AppendLine($"{spaces}{nameof(NextPoints)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NextPoints)}");
                foreach (var item in NextPoints)
                {
                    sb.Append($"{item.ToShortString(nextN)}");
                }
                sb.AppendLine($"{spaces}End {nameof(NextPoints)}");
            }

            if (InitPathsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(InitPathsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(InitPathsList)}");
                foreach (var item in InitPathsList)
                {
                    sb.AppendLine($"{nextNSpaces}{PathsHelper.DisplayPath(item)}");
                }
                sb.AppendLine($"{spaces}End {nameof(InitPathsList)}");
            }

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
            var nextNSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();

            sb.AppendLine($"{spaces}{nameof(Status)} = {Status}");
            sb.AppendLine($"{spaces}{nameof(TargetPosition)} = {TargetPosition}");

            if (NextSteps == null)
            {
                sb.AppendLine($"{spaces}{nameof(NextSteps)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NextSteps)}");
                foreach (var item in NextSteps)
                {
                    sb.Append($"{item.ToBriefString(nextN)}");
                }
                sb.AppendLine($"{spaces}End {nameof(NextSteps)}");
            }

            if (NextPoints == null)
            {
                sb.AppendLine($"{spaces}{nameof(NextPoints)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(NextPoints)}");
                foreach (var item in NextPoints)
                {
                    sb.Append($"{item.ToBriefString(nextN)}");
                }
                sb.AppendLine($"{spaces}End {nameof(NextPoints)}");
            }

            if (InitPathsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(InitPathsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(InitPathsList)}");
                foreach (var item in InitPathsList)
                {
                    sb.AppendLine($"{nextNSpaces}{PathsHelper.DisplayPath(item)}");
                }
                sb.AppendLine($"{spaces}End {nameof(InitPathsList)}");
            }

            return sb.ToString();
        }
    }
}
