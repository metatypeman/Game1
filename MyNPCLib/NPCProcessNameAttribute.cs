using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NPCProcessNameAttribute : Attribute
    {
        public NPCProcessNameAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
