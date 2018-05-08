using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalObject : BaseAbstractLogicalObject
    {
        public override bool IsConcrete => false;

        public LogicalObject(string query, IEntityDictionary entityDictionary, ILogicalStorage source, QueriesCache queriesCache)
        {
#if DEBUG
            LogInstance.Log($"Begin LogicalObject query = {query}");
#endif
            mEntityDictionary = entityDictionary;
            mSource = source;
            mSource.OnChanged += MSource_OnChanged;

            mPlan = queriesCache.CreatePlan(query);

//            var rootQueryNode = QueryASTNodeFactory.CreateASTNode(query, entityDictionary);

//#if DEBUG
//            LogInstance.Log($"LogicalObject rootQueryNode = {rootQueryNode}");
//#endif

//            mPlan = QueryResolverASTNodeFactory.CreatePlan(rootQueryNode);

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

        public override ulong CurrentEntityId => throw new NotImplementedException();

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

                lock (mCurrentEnitiesIdListLockObj)
                {
                    UpdateCurrentEnitiesIdList();
                }

                return mSource.GetPropertyValue(mCurrentEnitiesIdList, propertyKey);
            }

            set
            {
#if DEBUG
                LogInstance.Log($"LogicalObject this set propertyKey = {propertyKey} value = {value}");
#endif

                lock (mCurrentEnitiesIdListLockObj)
                {
                    UpdateCurrentEnitiesIdList();
                }

                mSource.SetPropertyValue(mCurrentEnitiesIdList, propertyKey, value);
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

                lock (mCurrentEnitiesIdListLockObj)
                {
                    UpdateCurrentEnitiesIdList();
                }

                return mSource.GetPropertyValue(mCurrentEnitiesIdList, propertyKey);
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                LogInstance.Log($"LogicalObject this set propertyName = {propertyName} propertyKey = {propertyKey} value = {value}");
#endif

                lock (mCurrentEnitiesIdListLockObj)
                {
                    UpdateCurrentEnitiesIdList();
                }

                mSource.SetPropertyValue(mCurrentEnitiesIdList, propertyKey, value);
            }
        }
    }
}
