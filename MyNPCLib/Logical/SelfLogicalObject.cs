using MyNPCLib.CGStorage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.Logical
{
    public class SelfLogicalObject : BaseAbstractLogicalObject
    {
        public SelfLogicalObject(IEntityLogger entityLogger, IEntityDictionary entityDictionary, ICGStorage source, SystemPropertiesDictionary systemPropertiesDictionary, INPCHostContext npcHostContext)
             : base(entityLogger, systemPropertiesDictionary)
        {
            mSelfEntityId = npcHostContext.SelfEntityId;
            mEntityDictionary = entityDictionary;
            mSource = source;
            mNPCHostContext = npcHostContext;
        }

        private ulong mSelfEntityId;
        private IEntityDictionary mEntityDictionary;
        private ICGStorage mSource;
        private INPCHostContext mNPCHostContext { get; set; }

        public override bool IsConcrete => true;
        public override IList<ulong> CurrentEntitiesIdList => new List<ulong>() { mSelfEntityId };
        public override ulong CurrentEntityId => mSelfEntityId;
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

            if(GetKindOfSystemProperty(propertyKey) != KindOfSystemProperties.Undefined)
            {
                return;
            }

            mSource.SetPropertyValueAsAsObject(mSelfEntityId, propertyKey, value);        
        }

        private object NGetProperty(ulong propertyKey)
        {
#if DEBUG
            //Log($"propertyKey = {propertyKey}");
#endif
            var kindOfSystemProperty = GetKindOfSystemProperty(propertyKey);

            switch (kindOfSystemProperty)
            {
                case KindOfSystemProperties.Undefined:
                    return mSource.GetPropertyValueAsObject(mSelfEntityId, propertyKey);

                case KindOfSystemProperties.GlobalPosition:
                    return mNPCHostContext.GlobalPosition;

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
