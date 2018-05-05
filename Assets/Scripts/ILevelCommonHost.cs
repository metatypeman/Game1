using MyNPCLib;
using MyNPCLib.Logical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface ILevelCommonHost
    {
        IEntityDictionary EntityDictionary { get; }
        NPCProcessInfoCache NPCProcessInfoCache { get; }
        LogicalIndexStorage LogicalObjectsBus { get; }
        GameObjectsBus GameObjectsBus { get; }
    }
}
