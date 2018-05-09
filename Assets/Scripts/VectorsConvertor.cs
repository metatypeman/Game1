using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class VectorsConvertor
    {
        public static System.Numerics.Vector3 UnityToNumeric(Vector3 value)
        {
            return new System.Numerics.Vector3(value.x, value.y, value.z);
        }

        public static System.Numerics.Vector3? NullableUnityToNumeric(Vector3? value)
        {
            if(value.HasValue)
            {
                return UnityToNumeric(value.Value);
            }

            return null;
        }

        public static Vector3 NumericToUnity(System.Numerics.Vector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static Vector3? NullableNumericToUnity(System.Numerics.Vector3? value)
        {
            if (value.HasValue)
            {
                return NumericToUnity(value.Value);
            }

            return null;
        }
    }
}
