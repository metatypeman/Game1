using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyNPCLib;

namespace Assets.Scripts
{
    public interface IMoveHumanoidCommandsPackage : IObjectToString
    {
        List<IMoveHumanoidCommand> Commands { get; }
    }
}
