using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface IThing : IObject
    {
        INPCProcess Send(INPCCommand command);
        object Get(string propertyName);
    }
}
