using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class TstConcreteNPCProcessesContextWithBlackBoard: NPCProcessesContextWithBlackBoard<TstBlackBoard>
    {
        public TstConcreteNPCProcessesContextWithBlackBoard(IMoveHumanoidController movehumanoidController)
            : base(movehumanoidController)
        {
        }
    }
}
