using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public abstract class OldTstBaseConcreteNPCProcessWithBlackBoard: OldBaseNPCProcessWithBlackBoard<OldTstBlackBoard>
    {
        protected OldTstBaseConcreteNPCProcessWithBlackBoard()
        {
        }

        protected OldTstBaseConcreteNPCProcessWithBlackBoard(OldNPCProcessesContext context)
            : base(context)
        {
        }
    }
}
