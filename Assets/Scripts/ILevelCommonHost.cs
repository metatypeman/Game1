using MyNPCLib;
using MyNPCLib.Logical;
using MyNPCLib.LogicalSoundModeling;
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
        LogicalObjectsBus LogicalObjectsBus { get; }
        QueriesCache QueriesCache { get; }
        HandThingsBus HandThingsBus { get; }
        LogicalSoundBus LogicalSoundBus { get; }
    }
}
