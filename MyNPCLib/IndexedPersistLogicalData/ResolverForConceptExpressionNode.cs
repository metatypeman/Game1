﻿using MyNPCLib.CGStorage;
using MyNPCLib.LogicalSearchEngine;
using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.IndexedPersistLogicalData
{
    [Serializable]
    public class ResolverForConceptExpressionNode : ResolverForBaseExpressionNode
    {
        public ConceptExpressionNode ConcreteOrigin { get; set; }
        public override BaseExpressionNode Origin => ConcreteOrigin;
        public override KindOfExpressionNode Kind => KindOfExpressionNode.Concept;
        public ulong Key { get; set; }

        public override void FillExecutingCard(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

#if DEBUG
            LogInstance.Log("End");
#endif
        }

        public override void FillExecutingCardForAnnotation(QueryExecutingCardForIndexedPersistLogicalData queryExecutingCard, LogicalSearchContext context)
        {
#if DEBUG
            LogInstance.Log("Begin");
#endif

#if DEBUG
            throw new NotImplementedException();
            LogInstance.Log("End");
#endif
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            return sb.ToString();
        }

        public override string PropertiesToShortSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToShortSting(n));
            sb.AppendLine($"{spaces}{nameof(Key)} = {Key}");
            return sb.ToString();
        }
    }
}
