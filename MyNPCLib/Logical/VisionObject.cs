using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionObject: OtherLogicalObject
    {
        public VisionObject(IEntityLogger entityLogger, ulong entityId, VisionObjectImpl visionObjectImpl, IEntityDictionary entityDictionary, ILogicalStorage source, SystemPropertiesDictionary systemPropertiesDictionary)
            : base(entityLogger, systemPropertiesDictionary)
        {
            mEntityId = entityId;
            CurrentVisionObjectImpl = visionObjectImpl;
            mEntityDictionary = entityDictionary;
            mSource = source;
        }

        private ulong mEntityId;
        private IEntityDictionary mEntityDictionary;
        private ILogicalStorage mSource;

        public override bool IsConcrete => true;
        public override IList<ulong> CurrentEntitiesIdList => new List<ulong>() { mEntityId };
        public override ulong CurrentEntityId => mEntityId;
        public override object this[ulong propertyKey]
        {
            get
            {
#if DEBUG
                Log($"propertyKey = {propertyKey}");
#endif

                return CommonGetProperty(propertyKey);
            }

            set
            {
#if DEBUG
                Log($"propertyKey = {propertyKey} value = {value}");
#endif

                CommonSetProperty(propertyKey, value);
            }
        }

        public override object this[string propertyName]
        {
            get
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                LogInstance.Log($"VisionObject this get propertyName = {propertyName} propertyKey = {propertyKey}");
#endif

                return CommonGetProperty(propertyKey);
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                LogInstance.Log($"VisionObject this set propertyName = {propertyName} propertyKey = {propertyKey} value = {value}");
#endif

                CommonSetProperty(propertyKey, value);
            }
        }

        protected override void ConcreteSetProperty(ulong propertyKey, object value)
        {
            mSource.SetPropertyValue(mEntityId, propertyKey, value);
        }

        protected override object ConcreteGetPropertyFromStorage(ulong propertyKey)
        {
            return mSource.GetPropertyValue(mEntityId, propertyKey);
        }

        public IList<IVisionItem> VisionItems => CurrentVisionObjectImpl.VisionItems;

        public override string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.Append(base.PropertiesToSting(n));
            sb.AppendLine($"{spaces}{nameof(CurrentEntityId)} = {CurrentEntityId}");
            if (VisionItems == null)
            {
                sb.AppendLine($"{spaces}{nameof(VisionItems)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(VisionItems)}");
                foreach (var visionItem in VisionItems)
                {
                    sb.Append(visionItem.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(VisionItems)}");
            }
            return sb.ToString();
        }
    }
}
