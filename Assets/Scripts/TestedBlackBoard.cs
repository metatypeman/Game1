using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class TestedBlackBoard : BaseBlackBoard, IObjectToString
    {
        public override void Bootstrap()
        {
#if UNITY_EDITOR
            Debug.Log("TestedBlackBoard Bootstrap");
#endif

            mNPCRayScaner = Context.GetInstance<INPCRayScaner>();
        }
    
        public int PossibleIdOfRifle { get; set; }
        public int InstanceIdOfRifle { get; set; }
        public Vector3? EthanPosition { get; set; }

        private INPCRayScaner mNPCRayScaner { get; set; }

        public List<InternalVisionObject> InternalVisibleObjects
        {
            get
            {
#if UNITY_EDITOR
                Debug.Log("TestedBlackBoard InternalVisibleObjects");
#endif
                //if (mNPCRayScaner == null)
                //{
                    return new List<InternalVisionObject>();
                //}

                //return mNPCRayScaner.InternalVisibleObjects;
            }
        }
        
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
            sb.AppendLine($"{spaces}{nameof(PossibleIdOfRifle)} = {PossibleIdOfRifle}");
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
