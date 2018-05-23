using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.TSTConceptualGraphs
{
    public class TSTConceptualGraphParser
    {
        public TSTConceptualGraph Parse(string text)
        {
#if DEBUG
            NLog.LogManager.GetCurrentClassLogger().Info($"Parse text = {text}");
#endif

            var graph = new TSTConceptualGraph();

            return graph;
        }
    }
}
