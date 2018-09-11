using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public abstract class BaseAbstractLogicalObject : IEquatable<BaseAbstractLogicalObject>, ILogicalObject, IObjectToString
    {
        protected BaseAbstractLogicalObject(IEntityLogger entityLogger, SystemPropertiesDictionary systemPropertiesDictionary)
        {
            mEntityLogger = entityLogger;
            mSystemPropertiesDictionary = systemPropertiesDictionary;
        }

        private IEntityLogger mEntityLogger;
        private readonly SystemPropertiesDictionary mSystemPropertiesDictionary;

        public abstract IList<ulong> CurrentEntitiesIdList { get; }
        public abstract ulong CurrentEntityId { get; }
        public abstract bool IsConcrete { get; }
        public abstract object this[ulong propertyKey] { get; set; }
        public abstract object this[string propertyName] { get; set; }

        [MethodForLoggingSupport]
        protected void Log(string message)
        {
            mEntityLogger?.Log(message);
        }

        [MethodForLoggingSupport]
        protected void Error(string message)
        {
            mEntityLogger?.Error(message);
        }

        [MethodForLoggingSupport]
        protected void Warning(string message)
        {
            mEntityLogger?.Warning(message);
        }

        protected KindOfSystemProperties GetKindOfSystemProperty(ulong propertyKey)
        {
#if DEBUG
            //Log($"propertyKey = {propertyKey}");
#endif

            return mSystemPropertiesDictionary.GetKindOfSystemProperty(propertyKey);
        }

        public T GetValue<T>(ulong propertyKey)
        {
            T result = default(T);

            try
            {
                result = (T)this[propertyKey];
            }
            catch (Exception e)
            {
#if DEBUG
                Error($"propertyKey = {propertyKey} e = {e}");
#endif
            }

            return result;
        }

        public T GetValue<T>(string propertyName)
        {
            T result = default(T);

            try
            {
                result = (T)this[propertyName];
            }
            catch (Exception e)
            {
#if DEBUG
                Error($"propertyName = {propertyName} e = {e}");
#endif
            }

            return result;
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
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(IsConcrete)} = {IsConcrete}");
            return sb.ToString();
        }

        private static bool NEqual(BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            //LogInstance.Log("Begin");
#endif
            if(ReferenceEquals(item1, item2))
            {
                return true;
            }

            if(ReferenceEquals(item1, null))
            {
                return false;
            }

            if(ReferenceEquals(item2, null))
            {
                return false;
            }

            var item1IsConcrete = item1.IsConcrete;
            var item2IsConcrete = item2.IsConcrete;

#if DEBUG
            //LogInstance.Log($"NEXT item1IsConcrete = {item1IsConcrete} item2IsConcrete = {item2IsConcrete}");
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
            //LogInstance.Log($"entitiesIdListOfItem1.Count = {entitiesIdListOfItem1.Count}");
            //foreach (var entityId in entitiesIdListOfItem1)
            //{
            //    LogInstance.Log($"entityId = {entityId}");
            //}
#endif

            var entitiesIdListOfItem2 = item2.CurrentEntitiesIdList;

#if DEBUG
            //LogInstance.Log($"entitiesIdListOfItem2.Count = {entitiesIdListOfItem2.Count}");
            //foreach (var entityId in entitiesIdListOfItem2)
            //{
            //    LogInstance.Log($"entityId = {entityId}");
            //}
#endif

            if(entitiesIdListOfItem1.Count == entitiesIdListOfItem2.Count)
            {
#if DEBUG
                //LogInstance.Log("entitiesIdListOfItem1.Count == entitiesIdListOfItem2.Count");
#endif

                var exceptList = entitiesIdListOfItem1.Except(entitiesIdListOfItem2);

#if DEBUG
                //LogInstance.Log($"exceptList.Count() = {exceptList.Count()}");
#endif

                if(exceptList.Count() == 0)
                {
                    return true;
                }

                return false;
            }

#if DEBUG
            //LogInstance.Log("NEXT NEXT");
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

        public bool Equals(BaseAbstractLogicalObject other)
        {
            return NEqual(this, other);
        }

        public static bool operator == (BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            //LogInstance.Log("==");
#endif
            return NEqual(item1, item2);
        }

        public static bool operator !=(BaseAbstractLogicalObject item1, BaseAbstractLogicalObject item2)
        {
#if DEBUG
            //LogInstance.Log("!=");
#endif
            return !NEqual(item1, item2);
        }
    }
}
