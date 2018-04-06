using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.NPCBehaviour
{
    public class MyBlackBoard
    {
        public MyBlackBoard()
        {
            NLog.LogManager.GetCurrentClassLogger().Info("MyBlackBoard");
        }

        public int TstValue { get; set; }
    }
}
