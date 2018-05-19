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
        private readonly object mPrimaryEntityIdLockObj = new object(); 
        private ulong mPrimaryEntityId;

        private void FindPrimaryEntityId()
        {
#if DEBUG
            //LogInstance.Log("Begin LogicalObject FindPrimaryEntityId");
#endif

            lock (mPrimaryEntityIdLockObj)
            {
                if (mCurrentEnitiesIdList.Count == 0)
                {
                    mPrimaryEntityId = 0ul;
                    CurrentVisionObjectImpl = null;

#if DEBUG
                    //LogInstance.Log("LogicalObject FindPrimaryEntityId mCurrentEnitiesIdList.Count == 0");
#endif

                    return;
                }

                if(mCurrentEnitiesIdList.Count == 1)
                {
                    var newPrimaryEntityId = mCurrentEnitiesIdList.First();

                    if(mPrimaryEntityId == newPrimaryEntityId)
                    {
#if DEBUG
                        //LogInstance.Log($"LogicalObject FindPrimaryEntityId mCurrentEnitiesIdList.Count == 1 mPrimaryEntityId == newPrimaryEntityId newPrimaryEntityId = {newPrimaryEntityId}");
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
                    //LogInstance.Log($"LogicalObject FindPrimaryEntityId mCurrentEnitiesIdList.Count == 1 ff mPrimaryEntityId = {mPrimaryEntityId}");
#endif

                    return;
                }

                var implsDict = mVisionObjectsStorage.GetVisionObjectsImplDict(mCurrentEnitiesIdList);

                if(implsDict.Count == 0)
                {
                    mPrimaryEntityId = mCurrentEnitiesIdList.First();
                    CurrentVisionObjectImpl = null;

#if DEBUG
                    //LogInstance.Log($"LogicalObject FindPrimaryEntityId implsDict.Count == 0 mPrimaryEntityId = {mPrimaryEntityId}");
#endif

                    return;
                }

                if(implsDict.ContainsKey(mPrimaryEntityId))
                {
#if DEBUG
                    //LogInstance.Log("LogicalObject FindPrimaryEntityId implsDict.ContainsKey(mPrimaryEntityId)");
#endif

                    return;
                }

                var firstImplItem = implsDict.First();

                mPrimaryEntityId = firstImplItem.Key;
                CurrentVisionObjectImpl = firstImplItem.Value;

#if DEBUG
                //LogInstance.Log($"End LogicalObject FindPrimaryEntityId mPrimaryEntityId = {mPrimaryEntityId}");
#endif
            }
        }

        public override IList<ulong> CurrentEntitiesIdList
        {
            get
            {
#if DEBUG
                //LogInstance.Log("Begin LogicalObject CurrentEnitiesIdList");
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
                //LogInstance.Log($"LogicalObject CurrentEntityId CurrentEntitiesIdList?.Count = {CurrentEntitiesIdList?.Count}");
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
            //LogInstance.Log("Begin LogicalObject UpdateCurrentEnitiesIdList");
#endif

            if(!mNeedUpdateEnitiesIdList)
            {
                FindPrimaryEntityId();
                return;
            }

            mNeedUpdateEnitiesIdList = false;

            mCurrentEnitiesIdList = mSource.GetEntitiesIdList(mPlan);

            FindPrimaryEntityId();

#if DEBUG
            //LogInstance.Log("End LogicalObject UpdateCurrentEnitiesIdList");
#endif
        }

        public override object this[ulong propertyKey]
        {
            get
            {
#if DEBUG
                //LogInstance.Log($"LogicalObject this get propertyKey = {propertyKey}");
#endif

                return NGetProperty(propertyKey);
            }

            set
            {
#if DEBUG
                //LogInstance.Log($"LogicalObject this set propertyKey = {propertyKey} value = {value}");
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
                //LogInstance.Log($"LogicalObject this get propertyName = {propertyName} propertyKey = {propertyKey}");
#endif

                return NGetProperty(propertyKey);
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                //LogInstance.Log($"LogicalObject this set propertyName = {propertyName} propertyKey = {propertyKey} value = {value}");
#endif

                NSetProperty(propertyKey, value);
            }
        }

        private void NSetProperty(ulong propertyKey, object value)
        {
#if DEBUG
            //LogInstance.Log($"LogicalObject NSetProperty propertyKey = {propertyKey} value = {value}");
#endif

            lock (mCurrentEnitiesIdListLockObj)
            {
                UpdateCurrentEnitiesIdList();
            }

            CommonSetProperty(propertyKey, value);
        }

        protected override void ConcreteSetProperty(ulong propertyKey, object value)
        {
            mSource.SetPropertyValue(mCurrentEnitiesIdList, propertyKey, value);
        }

        private object NGetProperty(ulong propertyKey)
        {
#if DEBUG
            //LogInstance.Log($"LogicalObject NGetProperty propertyKey = {propertyKey}");
#endif

            lock (mCurrentEnitiesIdListLockObj)
            {
                UpdateCurrentEnitiesIdList();
            }

            return CommonGetProperty(propertyKey);
        }

        protected override object ConcreteGetPropertyFromStorage(ulong propertyKey)
        {
            return mSource.GetPropertyValue(mCurrentEnitiesIdList, propertyKey);
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
