using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionObject: BaseAbstractLogicalObject
    {
        public VisionObject(ulong entityId, IList<IVisionItem> visionItems, IEntityDictionary entityDictionary, ILogicalStorage source, SystemPropertiesDictionary systemPropertiesDictionary)
            : base(systemPropertiesDictionary)
        {
            mEntityId = entityId;
            VisionItems = visionItems;
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
                LogInstance.Log($"VisionObject this get propertyKey = {propertyKey}");
#endif

                return NGetProperty(propertyKey);
            }

            set
            {
#if DEBUG
                LogInstance.Log($"VisionObject this set propertyKey = {propertyKey} value = {value}");
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
                LogInstance.Log($"VisionObject this get propertyName = {propertyName} propertyKey = {propertyKey}");
#endif

                return NGetProperty(propertyKey);
            }

            set
            {
                var propertyKey = mEntityDictionary.GetKey(propertyName);

#if DEBUG
                LogInstance.Log($"VisionObject this set propertyName = {propertyName} propertyKey = {propertyKey} value = {value}");
#endif

                NSetProperty(propertyKey, value);
            }
        }

        private void NSetProperty(ulong propertyKey, object value)
        {
#if DEBUG
            LogInstance.Log($"VisionObject NSetProperty propertyKey = {propertyKey} value = {value}");
#endif

            mSource.SetPropertyValue(mEntityId, propertyKey, value);
        }

        private object NGetProperty(ulong propertyKey)
        {
#if DEBUG
            LogInstance.Log($"VisionObject NGetProperty propertyKey = {propertyKey}");
#endif

            var kindOfSystemProperty = GetKindOfSystemProperty(propertyKey);

            switch (kindOfSystemProperty)
            {
                case KindOfSystemProperties.Undefined:
                    return mSource.GetPropertyValue(mEntityId, propertyKey);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfSystemProperty), kindOfSystemProperty, null);
            }
        }

        public IList<IVisionItem> VisionItems { get; set; }

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
