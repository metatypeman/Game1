using MyNPCLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace TmpSandBox.TSTConceptualGraphs
{
    public interface ITSTLogicalStorage : IObjectToString
    {
        TSTKindOfLogicalStorage Kind { get; }
        ITSTLogicalStorage Query(TSTConceptualGraph graph);
        ITSTLogicalStorage Query(TSTGnuClaySentence sentence);
        ITSTLogicalStorage Query(TSTPredicateSentence sentence);
        TSTConceptualGraph GetConceptualGraph();
        TSTGnuClaySentence GetGnuClaySentence();
        TSTPredicateSentence GetPredicateSentence();
        ITSTLogicalStorage Fork();
        void Accept(ITSTLogicalStorage storage);
        void Accept(TSTConceptualGraph graph);
        void Accept(TSTGnuClaySentence sentence);
        void Accept(TSTPredicateSentence sentence);
    }
}
