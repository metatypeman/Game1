using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class NPCProcessesContext
    {
        private NPCSimpleDI mSimpleDI = new NPCSimpleDI();

        private List<BaseNPCProcess> mChildProcessesList = new List<BaseNPCProcess>();

        public void AddChild(BaseNPCProcess process)
        {
            if(process == null)
            {
                return;
            }


        }

        public void RemoveChild(BaseNPCProcess process)
        {
            if (process == null)
            {
                return;
            }


        }
    }
}
