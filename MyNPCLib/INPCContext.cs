using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib
{
    public interface INPCContext
    {
        INPCProcess Send(INPCCommand command);
    }
}
