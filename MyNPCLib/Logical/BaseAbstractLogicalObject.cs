using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class BaseAbstractLogicalObject
    {
        public abstract IList<ulong> CurrentEnitiesIdList();

        private static bool NEqual(BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            LogInstance.Log("BaseAbstractLogicalObject NEqual");
#endif
            if(ReferenceEquals(item1, item2))
            {
                return true;
            }

#if DEBUG
            LogInstance.Log("BaseAbstractLogicalObject NEqual NEXT");
#endif

            var entitiesIdListOfItem1 = item1.CurrentEnitiesIdList();

#if DEBUG
            LogInstance.Log($"BaseAbstractLogicalObject NEqual entitiesIdListOfItem1.Count = {entitiesIdListOfItem1.Count}");
            foreach (var entityId in entitiesIdListOfItem1)
            {
                LogInstance.Log($"BaseAbstractLogicalObject NEqual entityId = {entityId}");
            }
#endif

            var entitiesIdListOfItem2 = item2.CurrentEnitiesIdList();

#if DEBUG
            LogInstance.Log($"BaseAbstractLogicalObject NEqual entitiesIdListOfItem2.Count = {entitiesIdListOfItem2.Count}");
            foreach (var entityId in entitiesIdListOfItem2)
            {
                LogInstance.Log($"BaseAbstractLogicalObject NEqual entityId = {entityId}");
            }
#endif

            if(entitiesIdListOfItem1.Count == entitiesIdListOfItem2.Count)
            {
#if DEBUG
                LogInstance.Log("BaseAbstractLogicalObject NEqual entitiesIdListOfItem1.Count == entitiesIdListOfItem2.Count");
#endif

                var exceptList = entitiesIdListOfItem1.Except(entitiesIdListOfItem2);

#if DEBUG
                LogInstance.Log($"BaseAbstractLogicalObject NEqual exceptList.Count() = {exceptList.Count()}");
#endif

                if(exceptList.Count() == 0)
                {
                    return true;
                }

                return false;
            }

#if DEBUG
            LogInstance.Log("BaseAbstractLogicalObject NEqual NEXT NEXT");
#endif

            IList<ulong> bigerList = null;
            IList<ulong> smallerList = null;

            if(entitiesIdListOfItem1.Count > entitiesIdListOfItem2.Count)
            {
                bigerList = entitiesIdListOfItem1;
                smallerList = entitiesIdListOfItem2;
            }
            else
            {
                bigerList = entitiesIdListOfItem2;
                smallerList = entitiesIdListOfItem1;
            }

            d
            throw new NotImplementedException();

            //return true;
        }

        public static bool operator == (BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            LogInstance.Log("BaseAbstractLogicalObject ==");
#endif
            return NEqual(item1, item2);
        }

        public static bool operator !=(BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            LogInstance.Log("BaseAbstractLogicalObject !=");
#endif
            return !NEqual(item1, item2);
        }
    }
}
