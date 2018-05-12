﻿using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class TestedBlackBoard : BaseBlackBoard, IObjectToString
    {
        public override void Bootstrap()
        {
#if UNITY_EDITOR
            LogInstance.Log("TestedBlackBoard Bootstrap");
#endif
        }
    
        public BaseAbstractLogicalObject InstanceIdOfRifle { get; set; }
        public Vector3? EthanPosition { get; set; }
      
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
            var nextSpaces = StringHelper.Spaces(nextN);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(InstanceIdOfRifle)} = {InstanceIdOfRifle}");
            if(EthanPosition.HasValue)
            {
                sb.AppendLine($"{spaces}{nameof(EthanPosition)} = {EthanPosition}");
            }
            else
            {
                sb.AppendLine($"{spaces}{nameof(EthanPosition)} = null");            
            }
            return sb.ToString();
        }
    }
}
