using MyNPCLib.IndexedPersistLogicalData;
using MyNPCLib.PersistLogicalData;
using MyNPCLib.Variants;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.VariantsConverting
{
    public static class VariantsConvertor
    {
        public static BaseVariant ConvertObjectToVariant(object source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            throw new NotImplementedException();
        }

        public static BaseVariant ConvertResultOfVarToVariant(ResultOfVarOfQueryToRelation source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            throw new NotImplementedException();
        }

        public static BaseExpressionNode ConvertVariantToExpressionNode(BaseVariant source)
        {
#if DEBUG
            LogInstance.Log($"source = {source}");
#endif

            throw new NotImplementedException();
        }
    }
}
