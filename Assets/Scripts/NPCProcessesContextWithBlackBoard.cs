using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class NPCProcessesContextWithBlackBoard<T>: NPCProcessesContext where T: class, new()
    {
        public NPCProcessesContextWithBlackBoard(IMoveHumanoidController movehumanoidController)
            : base(movehumanoidController)
        {
            var blackBoard = new T();
            RegisterInstance<T>(blackBoard);
        }
    }
}
