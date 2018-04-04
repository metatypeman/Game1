using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    public class StubOfHumanoidBodyController: IHumanoidBodyController
    {
        private StatesOfHumanoidBodyController mStates = new StatesOfHumanoidBodyController();

        public StatesOfHumanoidBodyController States {
            get
            {
                return mStates;
            }
        }
    }
}
