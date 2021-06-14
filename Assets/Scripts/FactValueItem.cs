//using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public class FactValueItem//: IObjectToString
    {
        public string PropertyName;
        public string StringValue;
        public GameObject GameObjectValue;

        //public override string ToString()
        //{
        //    return ToString(0u);
        //}

        //public string ToString(uint n)
        //{
        //    return this.GetDefaultToStringInformation(n);
        //}

        //public string PropertiesToString(uint n)
        //{
        //    var spaces = StringHelper.Spaces(n);
        //    var nextN = n + 4;
        //    var sb = new StringBuilder();
        //    sb.AppendLine($"{spaces}{nameof(PropertyName)} = {PropertyName}");
        //    sb.AppendLine($"{spaces}{nameof(StringValue)} = {StringValue}");
        //    sb.AppendLine($"{spaces}HasGameObjectValue = {GameObjectValue != null}");
        //    return sb.ToString();
        //}
    }
}
