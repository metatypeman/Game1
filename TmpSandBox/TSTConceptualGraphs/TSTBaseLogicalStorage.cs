using System;
using System.Collections.Generic;
using System.Text;
using MyNPCLib;

namespace TmpSandBox.TSTConceptualGraphs
{
    public abstract class TSTBaseLogicalStorage : ITSTLogicalStorage
    {
        public abstract TSTKindOfLogicalStorage Kind { get; }

        public ITSTLogicalStorage Query(TSTConceptualGraph graph)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Query graph = {graph}");

            var localStorage = new TSTQueryLogicalStorage();

            return localStorage;
        }

        public ITSTLogicalStorage Query(TSTGnuClaySentence sentence)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Query sentence = {sentence}");

            var localStorage = new TSTQueryLogicalStorage();

            return localStorage;
        }

        public ITSTLogicalStorage Query(TSTPredicateSentence sentence)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Query sentence = {sentence}");

            var localStorage = new TSTQueryLogicalStorage();

            return localStorage;
        }

        public TSTConceptualGraph GetConceptualGraph()
        {
            var graph = new TSTConceptualGraph();

            return graph;
        }

        public TSTGnuClaySentence GetGnuClaySentence()
        {
            var sentence = new TSTGnuClaySentence();

            return sentence;
        }

        public TSTPredicateSentence GetPredicateSentence()
        {
            var sentence = new TSTPredicateSentence();

            return sentence;
        }

        public abstract ITSTLogicalStorage Fork();

        public void Accept(ITSTLogicalStorage storage)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Accept storage = {storage}");
        }

        public void Accept(TSTConceptualGraph graph)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Accept graph = {graph}");
        }

        public void Accept(TSTGnuClaySentence sentence)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Accept sentence = {sentence}");
        }

        public void Accept(TSTPredicateSentence sentence)
        {
            NLog.LogManager.GetCurrentClassLogger().Info($"Accept sentence = {sentence}");
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public virtual string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(Kind)} = {Kind}");
            return sb.ToString();
        }
    }
}
