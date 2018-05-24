using System;
using System.Collections.Generic;
using System.Text;
using MyNPCLib;

namespace TmpSandBox.TSTConceptualGraphs
{
    public class TSTQueryLogicalStorage : TSTBaseLogicalStorage
    {
        public override TSTKindOfLogicalStorage Kind => TSTKindOfLogicalStorage.Query;

        public override ITSTLogicalStorage Fork()
        {
            var localStorage = new TSTQueryLogicalStorage();

            return localStorage;
        }
    }
}
