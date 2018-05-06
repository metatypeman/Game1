﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class SelfLogicalObject : BaseAbstractLogicalObject
    {
        public SelfLogicalObject(ulong selfEntityId, IEntityDictionary entityDictionary, ILogicalStorage source)
        {
            mSelfEntityId = selfEntityId;
            mEntityDictionary = entityDictionary;
            mSource = source;
        }

        private ulong mSelfEntityId;
        private IEntityDictionary mEntityDictionary;
        private ILogicalStorage mSource;

        public override bool IsConcrete => true;
        public override IList<ulong> CurrentEntitiesIdList => new List<ulong>() { mSelfEntityId };
        public override ulong CurrentEntityId => mSelfEntityId;
        public override object this[ulong propertyKey]
        {
            get
            {
#if DEBUG
                LogInstance.Log($"SelfLogicalObject this get propertyKey = {propertyKey}");
#endif

                return mSource.GetPropertyValue(mSelfEntityId, propertyKey);
            }

            set
            {
#if DEBUG
                LogInstance.Log($"SelfLogicalObject this set propertyKey = {propertyKey} value = {value}");
#endif

                mSource.SetPropertyValue(mSelfEntityId, propertyKey, value);
            }
        }

        public override object this[string propertyName]
        {
            get
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                LogInstance.Log($"SelfLogicalObject this get propertyName = {propertyName} propertyKey = {propertyKey}");
#endif

                return mSource.GetPropertyValue(mSelfEntityId, propertyKey);
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                LogInstance.Log($"SelfLogicalObject this set propertyName = {propertyName} propertyKey = {propertyKey} value = {value}");
#endif

                mSource.SetPropertyValue(mSelfEntityId, propertyKey, value);
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
