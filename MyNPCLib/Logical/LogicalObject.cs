using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalObject : OtherLogicalObject
    {
        public override bool IsConcrete => false;

        public LogicalObject(string query, IEntityDictionary entityDictionary, ILogicalStorage source, QueriesCache queriesCache, SystemPropertiesDictionary systemPropertiesDictionary, VisionObjectsStorage visionObjectsStorage)
            : base (systemPropertiesDictionary)
        {
#if DEBUG
            LogInstance.Log($"Begin LogicalObject query = {query}");
#endif
            mEntityDictionary = entityDictionary;
            mVisionObjectsStorage = visionObjectsStorage;
            mSource = source;
            mSource.OnChanged += MSource_OnChanged;

            mPlan = queriesCache.CreatePlan(query);

#if DEBUG
            LogInstance.Log($"End LogicalObject query = {query}");
#endif
        }

        private IEntityDictionary mEntityDictionary;

        private void MSource_OnChanged()
        {
#if DEBUG
            LogInstance.Log("LogicalObject MSource_OnChanged");
#endif

            lock (mCurrentEnitiesIdListLockObj)
            {
                mNeedUpdateEnitiesIdList = true;
            }
        }

        private BaseQueryResolverASTNode mPlan;
        private ILogicalStorage mSource;
        private VisionObjectsStorage mVisionObjectsStorage;
        private bool mNeedUpdateEnitiesIdList = true;
        private readonly object mCurrentEnitiesIdListLockObj = new object();
        private IList<ulong> mCurrentEnitiesIdList;

        public override IList<ulong> CurrentEntitiesIdList
        {
            get
            {
#if DEBUG
                LogInstance.Log("Begin LogicalObject CurrentEnitiesIdList");
#endif

                lock (mCurrentEnitiesIdListLockObj)
                {
                    UpdateCurrentEnitiesIdList();

                    return mCurrentEnitiesIdList;
                }
            }     
        }

        public override ulong CurrentEntityId
        {
            get
            {
#if DEBUG
                LogInstance.Log($"LogicalObject CurrentEntityId CurrentEntitiesIdList?.Count = {CurrentEntitiesIdList?.Count}");
#endif

                if (CurrentEntitiesIdList.Count == 0)
                {
                    return 0ul;
                }

                return CurrentEntitiesIdList.First();
            }
        } 

        private void UpdateCurrentEnitiesIdList()
        {
#if DEBUG
            LogInstance.Log("Begin LogicalObject UpdateCurrentEnitiesIdList");
#endif

            if(!mNeedUpdateEnitiesIdList)
            {
                return;
            }

            mNeedUpdateEnitiesIdList = false;

            mCurrentEnitiesIdList = mSource.GetEntitiesIdList(mPlan);

#if DEBUG
            LogInstance.Log("End LogicalObject UpdateCurrentEnitiesIdList");
#endif
        }

        public override object this[ulong propertyKey]
        {
            get
            {
#if DEBUG
                LogInstance.Log($"LogicalObject this get propertyKey = {propertyKey}");
#endif

                return NGetProperty(propertyKey);
            }

            set
            {
#if DEBUG
                LogInstance.Log($"LogicalObject this set propertyKey = {propertyKey} value = {value}");
#endif

                NSetProperty(propertyKey, value);
            }
        }

        public override object this[string propertyName]
        {
            get
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                LogInstance.Log($"LogicalObject this get propertyName = {propertyName} propertyKey = {propertyKey}");
#endif

                return NGetProperty(propertyKey);
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                LogInstance.Log($"LogicalObject this set propertyName = {propertyName} propertyKey = {propertyKey} value = {value}");
#endif

                NSetProperty(propertyKey, value);
            }
        }

        private void NSetProperty(ulong propertyKey, object value)
        {
#if DEBUG
            LogInstance.Log($"LogicalObject NSetProperty propertyKey = {propertyKey} value = {value}");
#endif

            lock (mCurrentEnitiesIdListLockObj)
            {
                UpdateCurrentEnitiesIdList();
            }

            mSource.SetPropertyValue(mCurrentEnitiesIdList, propertyKey, value);
        }

        private object NGetProperty(ulong propertyKey)
        {
#if DEBUG
            LogInstance.Log($"LogicalObject NGetProperty propertyKey = {propertyKey}");
#endif

            var kindOfSystemProperty = GetKindOfSystemProperty(propertyKey);

            lock (mCurrentEnitiesIdListLockObj)
            {
                UpdateCurrentEnitiesIdList();
            }

            switch (kindOfSystemProperty)
            {
                case KindOfSystemProperties.Undefined:
                    return mSource.GetPropertyValue(mCurrentEnitiesIdList, propertyKey);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfSystemProperty), kindOfSystemProperty, null);
            }
        }

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(CurrentEntityId)} = {CurrentEntityId}");
            return sb.ToString();
        }
    }
}
