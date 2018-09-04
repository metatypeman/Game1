using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalObject : OtherLogicalObject
    {
        public override bool IsConcrete => false;

        public LogicalObject(IEntityLogger entityLogger, string query, IEntityDictionary entityDictionary, IOldLogicalStorage oldSource, ICGStorage source, QueriesCache queriesCache, SystemPropertiesDictionary systemPropertiesDictionary, VisionObjectsStorage visionObjectsStorage)
            : base (entityLogger, systemPropertiesDictionary)
        {
#if DEBUG
            Log($"Begin query = {query}");
#endif
            mEntityDictionary = entityDictionary;
            mVisionObjectsStorage = visionObjectsStorage;
            mSource = source;
            mOldSource = oldSource;
            mOldSource.OnChanged += MSource_OnChanged;

            mPlan = queriesCache.CreatePlan(query);

#if DEBUG
            Log($"End query = {query}");
#endif
        }

        private IEntityDictionary mEntityDictionary;

        private void MSource_OnChanged()
        {
#if DEBUG
            Log("Begin");
#endif

            lock (mCurrentEnitiesIdListLockObj)
            {
                mNeedUpdateEnitiesIdList = true;
            }
        }

        private BaseQueryResolverASTNode mPlan;
        private IOldLogicalStorage mOldSource;
        private ICGStorage mSource;
        private VisionObjectsStorage mVisionObjectsStorage;
        private bool mNeedUpdateEnitiesIdList = true;
        private readonly object mCurrentEnitiesIdListLockObj = new object();     
        private IList<ulong> mCurrentEnitiesIdList;
        private readonly object mPrimaryEntityIdLockObj = new object(); 
        private ulong mPrimaryEntityId;

        private void FindPrimaryEntityId()
        {
#if DEBUG
            //Log("Begin");
#endif

            lock (mPrimaryEntityIdLockObj)
            {
                if (mCurrentEnitiesIdList.Count == 0)
                {
                    mPrimaryEntityId = 0ul;
                    CurrentVisionObjectImpl = null;

#if DEBUG
                    //Log("mCurrentEnitiesIdList.Count == 0");
#endif

                    return;
                }

                if(mCurrentEnitiesIdList.Count == 1)
                {
                    var newPrimaryEntityId = mCurrentEnitiesIdList.First();

                    if(mPrimaryEntityId == newPrimaryEntityId)
                    {
#if DEBUG
                        //Log($"mCurrentEnitiesIdList.Count == 1 mPrimaryEntityId == newPrimaryEntityId newPrimaryEntityId = {newPrimaryEntityId}");
#endif
                        if(CurrentVisionObjectImpl == null)
                        {
                            CurrentVisionObjectImpl = mVisionObjectsStorage.GetVisionObjectImpl(mPrimaryEntityId);
                        }
                        return;
                    }

                    mPrimaryEntityId = newPrimaryEntityId;

                    CurrentVisionObjectImpl = mVisionObjectsStorage.GetVisionObjectImpl(mPrimaryEntityId);

#if DEBUG
                    //Log($"mCurrentEnitiesIdList.Count == 1 ff mPrimaryEntityId = {mPrimaryEntityId}");
#endif

                    return;
                }

                var implsDict = mVisionObjectsStorage.GetVisionObjectsImplDict(mCurrentEnitiesIdList);

                if(implsDict.Count == 0)
                {
                    mPrimaryEntityId = mCurrentEnitiesIdList.First();
                    CurrentVisionObjectImpl = null;

#if DEBUG
                    //Log($"implsDict.Count == 0 mPrimaryEntityId = {mPrimaryEntityId}");
#endif

                    return;
                }

                if(implsDict.ContainsKey(mPrimaryEntityId))
                {
#if DEBUG
                    //Log("implsDict.ContainsKey(mPrimaryEntityId)");
#endif

                    return;
                }

                var firstImplItem = implsDict.First();

                mPrimaryEntityId = firstImplItem.Key;
                CurrentVisionObjectImpl = firstImplItem.Value;

#if DEBUG
                //Log($"End mPrimaryEntityId = {mPrimaryEntityId}");
#endif
            }
        }

        public override IList<ulong> CurrentEntitiesIdList
        {
            get
            {
#if DEBUG
                //Log("Begin");
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
                //Log($"CurrentEntitiesIdList?.Count = {CurrentEntitiesIdList?.Count}");
#endif

                lock (mCurrentEnitiesIdListLockObj)
                {
                    UpdateCurrentEnitiesIdList();

                    return mPrimaryEntityId;
                }                
            }
        } 

        private void UpdateCurrentEnitiesIdList()
        {
#if DEBUG
            //Log("Begin");
#endif

            if(!mNeedUpdateEnitiesIdList)
            {
                FindPrimaryEntityId();
                return;
            }

            mNeedUpdateEnitiesIdList = false;

            mCurrentEnitiesIdList = mOldSource.GetEntitiesIdList(mPlan);

            FindPrimaryEntityId();

#if DEBUG
            //Log("End");
#endif
        }

        public override object this[ulong propertyKey]
        {
            get
            {
#if DEBUG
                //Log($"propertyKey = {propertyKey}");
#endif

                return NGetProperty(propertyKey);
            }

            set
            {
#if DEBUG
                //Log($"propertyKey = {propertyKey} value = {value}");
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
                //Log($"propertyName = {propertyName} propertyKey = {propertyKey}");
#endif

                return NGetProperty(propertyKey);
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                //Log($"propertyName = {propertyName} propertyKey = {propertyKey} value = {value}");
#endif

                NSetProperty(propertyKey, value);
            }
        }

        private void NSetProperty(ulong propertyKey, object value)
        {
#if DEBUG
            //Log($"propertyKey = {propertyKey} value = {value}");
#endif

            lock (mCurrentEnitiesIdListLockObj)
            {
                UpdateCurrentEnitiesIdList();
            }

            CommonSetProperty(propertyKey, value);
        }

        protected override void ConcreteSetProperty(ulong propertyKey, object value)
        {
            mOldSource.SetPropertyValue(mPrimaryEntityId, propertyKey, value);
        }

        private object NGetProperty(ulong propertyKey)
        {
#if DEBUG
            //Log($"propertyKey = {propertyKey}");
#endif

            lock (mCurrentEnitiesIdListLockObj)
            {
                UpdateCurrentEnitiesIdList();
            }

            return CommonGetProperty(propertyKey);
        }

        protected override object ConcreteGetPropertyFromStorage(ulong propertyKey)
        {
            return mOldSource.GetPropertyValue(mPrimaryEntityId, propertyKey);
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
