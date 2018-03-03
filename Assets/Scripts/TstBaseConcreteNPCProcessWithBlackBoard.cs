using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public abstract class TstBaseConcreteNPCProcessWithBlackBoard: BaseNPCProcessWithBlackBoard<TstBlackBoard>
    {
        protected TstBaseConcreteNPCProcessWithBlackBoard()
        {
        }

        protected TstBaseConcreteNPCProcessWithBlackBoard(NPCProcessesContext context)
            : base(context)
        {
        }
    }
}
