using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public class TypeIsNotNPCProcessException: Exception
    {
        public TypeIsNotNPCProcessException(Type wrongType)
            : base($"The type `{wrongType.FullName}` is not a subclass of `{typeof(BaseNPCProcess)}`")
        {
            WrongType = wrongType;
        }

        public Type WrongType { get; private set; }
    }
}
