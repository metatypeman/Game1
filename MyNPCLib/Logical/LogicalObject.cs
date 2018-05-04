using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class LogicalObject : BaseAbstractLogicalObject
    {
        public LogicalObject(string query, IEntityDictionary entityDictionary, ILogicalStorage source)
        {
#if DEBUG
            LogInstance.Log($"Begin LogicalObject query = {query}");
#endif

            mSource = source;
            mSource.OnChanged += MSource_OnChanged;
            var rootQueryNode = QueryASTNodeFactory.CreateASTNode(query, entityDictionary);

#if DEBUG
            LogInstance.Log($"LogicalObject rootQueryNode = {rootQueryNode}");
#endif

            mPlan = QueryResolverASTNodeFactory.CreatePlan(rootQueryNode);

#if DEBUG
            LogInstance.Log($"End LogicalObject query = {query}");
#endif
        }

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

        public override IList<ulong> CurrentEnitiesIdList()
        {
#if DEBUG
            LogInstance.Log("Begin LogicalObject CurrentEnitiesIdList");
#endif

            lock(mCurrentEnitiesIdListLockObj)
            {
                UpdateCurrentEnitiesIdList();

                return mCurrentEnitiesIdList;
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

            mCurrentEnitiesIdList = mPlan.GetEntitiesIdList(mSource);

#if DEBUG
            LogInstance.Log("End LogicalObject UpdateCurrentEnitiesIdList");
#endif
        }
    }
}
