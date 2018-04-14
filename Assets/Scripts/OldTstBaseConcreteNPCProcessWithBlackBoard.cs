using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public abstract class OldTstBaseConcreteNPCProcessWithBlackBoard: BaseNPCProcessWithBlackBoard<OldTstBlackBoard>
    {
        protected OldTstBaseConcreteNPCProcessWithBlackBoard()
        {
        }

        protected OldTstBaseConcreteNPCProcessWithBlackBoard(NPCProcessesContext context)
            : base(context)
        {
        }
    }
}
