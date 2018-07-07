using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.CommonServiceGrammaticalElements
{
    public static class SpecialElementsHeper
    {
        public static KindOfSpecialRelation GetKindOfSpecialRelation(string relationName)
        {
            if(string.IsNullOrWhiteSpace(relationName))
            {
                return KindOfSpecialRelation.Undefinded;
            }

            if(relationName == SpecialNamesOfRelations.ObjectRelationName)
            {
                return KindOfSpecialRelation.Object;
            }

            if (relationName == SpecialNamesOfRelations.ExperiencerRelationName)
            {
                return KindOfSpecialRelation.Experiencer;
            }

            if (relationName == SpecialNamesOfRelations.StateRelationName)
            {
                return KindOfSpecialRelation.State;
            }

            if (relationName == SpecialNamesOfRelations.AgentRelationName)
            {
                return KindOfSpecialRelation.Agent;
            }

            if (relationName == SpecialNamesOfRelations.ActionRelationName)
            {
                return KindOfSpecialRelation.Action;
            }

            return KindOfSpecialRelation.Undefinded;
        }
    }
}
