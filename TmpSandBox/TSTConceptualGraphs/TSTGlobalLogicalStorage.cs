using System;
using System.Collections.Generic;
using System.Text;
using MyNPCLib;

namespace TmpSandBox.TSTConceptualGraphs
{
    public class TSTGlobalLogicalStorage: TSTBaseLogicalStorage
    {
        public override TSTKindOfLogicalStorage Kind => TSTKindOfLogicalStorage.Global;

        public override ITSTLogicalStorage Fork()
        {
            var localStorage = new TSTLocalLogicalStorage();

            return localStorage;
        }
    }
}
