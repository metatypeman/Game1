//using MyNPCLib;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;

//namespace Assets.Scripts
//{
//    public static class GameObjectHelper
//    {
//        public static string GameObjectToString(GameObject gameObject, uint n)
//        {
//            var spaces = StringHelper.Spaces(n);
//            var sb = new StringBuilder();
//            sb.AppendLine($"{spaces}Begin {nameof(GameObject)}");
//            sb.AppendLine($"{spaces}InstanceID = {gameObject?.GetInstanceID()}");
//            sb.AppendLine($"{spaces}name = {gameObject?.name}");
//            sb.AppendLine($"{spaces}End {nameof(GameObject)}");
//            return sb.ToString();
//        }
//    }
//}
