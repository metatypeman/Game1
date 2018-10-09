using System;
using System.Collections.Generic;
using System.Text;
using MyNPCLib;

namespace TmpSandBox.TSTConceptualGraphs
{
    public class TSTLocalLogicalStorage: TSTBaseLogicalStorage
    {
        public override TSTKindOfLogicalStorage Kind => TSTKindOfLogicalStorage.Local;

        public override ITSTLogicalStorage Fork()
        {
            var localStorage = new TSTLocalLogicalStorage();

            return localStorage;
        }
    }
}
