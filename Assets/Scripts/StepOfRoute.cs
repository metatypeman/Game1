//using MyNPCLib;
//using MyNPCLib.NavigationSupport;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Assets.Scripts
//{
//    public class StepOfRoute : IStepOfRoute
//    {
//        public IRoute Route { get; set; }
//        public IList<IPointInfo> TargetPoints { get; set; } = new List<IPointInfo>();
//        public IList<IList<IPlane>> PathsList { get; set; } = new List<IList<IPlane>>();
//        public IPlane CurrentPlane { get; set; }

//        public override string ToString()
//        {
//            return ToString(0u);
//        }

//        public string ToString(uint n)
//        {
//            return this.GetDefaultToStringInformation(n);
//        }

//        public string PropertiesToString(uint n)
//        {
//            var spaces = StringHelper.Spaces(n);
//            var nextN = n + 4;
//            var nextNSpaces = StringHelper.Spaces(nextN);
//            var sb = new StringBuilder();
//            if (PathsList == null)
//            {
//                sb.AppendLine($"{spaces}{nameof(PathsList)} = null");
//            }
//            else
//            {
//                sb.AppendLine($"{spaces}Begin {nameof(PathsList)}");
//                foreach (var item in PathsList)
//                {
//                    sb.AppendLine($"{nextNSpaces}{PathsHelper.DisplayPath(item)}");
//                }
//                sb.AppendLine($"{spaces}End {nameof(PathsList)}");
//            }
//            sb.AppendLine($"{spaces}{nameof(CurrentPlane)} = {CurrentPlane?.Name}");
//            return sb.ToString();
//        }

//        public string ToShortString()
//        {
//            return ToShortString(0u);
//        }

//        public string ToShortString(uint n)
//        {
//            return this.GetDefaultToShortStringInformation(n);
//        }

//        public string PropertiesToShortString(uint n)
//        {
//            var spaces = StringHelper.Spaces(n);
//            var nextN = n + 4;
//            var nextNSpaces = StringHelper.Spaces(nextN);
//            var sb = new StringBuilder();
//            if (PathsList == null)
//            {
//                sb.AppendLine($"{spaces}{nameof(PathsList)} = null");
//            }
//            else
//            {
//                sb.AppendLine($"{spaces}Begin {nameof(PathsList)}");
//                foreach (var item in PathsList)
//                {
//                    sb.AppendLine($"{nextNSpaces}{PathsHelper.DisplayPath(item)}");
//                }
//                sb.AppendLine($"{spaces}End {nameof(PathsList)}");
//            }
//            sb.AppendLine($"{spaces}{nameof(CurrentPlane)} = {CurrentPlane?.Name}");
//            return sb.ToString();
//        }

//        public string ToBriefString()
//        {
//            return ToBriefString(0u);
//        }

//        public string ToBriefString(uint n)
//        {
//            return this.GetDefaultToBriefStringInformation(n);
//        }

//        public string PropertiesToBriefString(uint n)
//        {
//            var spaces = StringHelper.Spaces(n);
//            var nextN = n + 4;
//            var nextNSpaces = StringHelper.Spaces(nextN);
//            var sb = new StringBuilder();
//            if (PathsList == null)
//            {
//                sb.AppendLine($"{spaces}{nameof(PathsList)} = null");
//            }
//            else
//            {
//                sb.AppendLine($"{spaces}Begin {nameof(PathsList)}");
//                foreach (var item in PathsList)
//                {
//                    sb.AppendLine($"{nextNSpaces}{PathsHelper.DisplayPath(item)}");
//                }
//                sb.AppendLine($"{spaces}End {nameof(PathsList)}");
//            }

//            sb.AppendLine($"{spaces}{nameof(CurrentPlane)} = {CurrentPlane?.Name}");

//            return sb.ToString();
//        }
//    }
//}
