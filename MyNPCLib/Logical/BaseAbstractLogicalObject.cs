using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class BaseAbstractLogicalObject : ILogicalObject, IObjectToString
    {
        public abstract IList<ulong> CurrentEntitiesIdList { get; }
        public abstract ulong CurrentEntityId { get; }
        public abstract bool IsConcrete { get; }
        public abstract object this[ulong propertyKey] { get; set; }
        public abstract object this[string propertyName] { get; set; }

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
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsConcrete)} = {IsConcrete}");
            return sb.ToString();
        }

        private static bool NEqual(BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            //LogInstance.Log("BaseAbstractLogicalObject NEqual");
#endif
            if(ReferenceEquals(item1, item2))
            {
                return true;
            }

            var item1IsConcrete = item1.IsConcrete;
            var item2IsConcrete = item2.IsConcrete;

#if DEBUG
            //LogInstance.Log($"BaseAbstractLogicalObject NEqual NEXT item1IsConcrete = {item1IsConcrete} item2IsConcrete = {item2IsConcrete}");
#endif

            if(item1IsConcrete)
            {
                if(item2IsConcrete)
                {
                    return item1.CurrentEntityId == item2.CurrentEntityId;
                }
                else
                {
                    return item2.CurrentEntitiesIdList.Contains(item1.CurrentEntityId);
                }
            }

            if (item2IsConcrete)
            {
                return item1.CurrentEntitiesIdList.Contains(item2.CurrentEntityId);
            }

            var entitiesIdListOfItem1 = item1.CurrentEntitiesIdList;

#if DEBUG
            //LogInstance.Log($"BaseAbstractLogicalObject NEqual entitiesIdListOfItem1.Count = {entitiesIdListOfItem1.Count}");
            //foreach (var entityId in entitiesIdListOfItem1)
            //{
            //    LogInstance.Log($"BaseAbstractLogicalObject NEqual entityId = {entityId}");
            //}
#endif

            var entitiesIdListOfItem2 = item2.CurrentEntitiesIdList;

#if DEBUG
            //LogInstance.Log($"BaseAbstractLogicalObject NEqual entitiesIdListOfItem2.Count = {entitiesIdListOfItem2.Count}");
            //foreach (var entityId in entitiesIdListOfItem2)
            //{
            //    LogInstance.Log($"BaseAbstractLogicalObject NEqual entityId = {entityId}");
            //}
#endif

            if(entitiesIdListOfItem1.Count == entitiesIdListOfItem2.Count)
            {
#if DEBUG
                //LogInstance.Log("BaseAbstractLogicalObject NEqual entitiesIdListOfItem1.Count == entitiesIdListOfItem2.Count");
#endif

                var exceptList = entitiesIdListOfItem1.Except(entitiesIdListOfItem2);

#if DEBUG
                //LogInstance.Log($"BaseAbstractLogicalObject NEqual exceptList.Count() = {exceptList.Count()}");
#endif

                if(exceptList.Count() == 0)
                {
                    return true;
                }

                return false;
            }

#if DEBUG
            //LogInstance.Log("BaseAbstractLogicalObject NEqual NEXT NEXT");
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

            var exceptList_2 = smallerList.Except(smallerList);

            if (exceptList_2.Count() == 0)
            {
                return true;
            }

            return false;
        }

        public static bool operator == (BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            //LogInstance.Log("BaseAbstractLogicalObject ==");
#endif
            return NEqual(item1, item2);
        }

        public static bool operator !=(BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            //LogInstance.Log("BaseAbstractLogicalObject !=");
#endif
            return !NEqual(item1, item2);
        }
    }
}
