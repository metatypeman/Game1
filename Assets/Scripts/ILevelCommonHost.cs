using MyNPCLib;
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
        InternalLogicalObjectsBus LogicalObjectsBus { get; }
        GameObjectsBus GameObjectsBus { get; }
    }
}
