using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib.Logical
{
    public class VisionObjectImpl: IObjectToString
    {
        public VisionObjectImpl(ulong entityId, IList<IVisionItem> visionItems)
        {
            mEntityId = entityId;
            mVisionItems = visionItems;
        }

        private ulong mEntityId;

        public ulong EntityId => mEntityId;

        private readonly object mVisionItemsLockObj = new object();
        private IList<IVisionItem> mVisionItems = new List<IVisionItem>();
        public IList<IVisionItem> VisionItems
        {
            get
            {
                lock(mVisionItemsLockObj)
                {
                    return mVisionItems;
                }
            }

            set
            {
                lock (mVisionItemsLockObj)
                {
                    mVisionItems = value;
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                lock (mVisionItemsLockObj)
                {
                    return mVisionItems.Count > 0;
                }
            }
        }

        public Vector3? GetGlobalPosition()
        {
            lock (mVisionItemsLockObj)
            {
                var count = mVisionItems.Count;

#if DEBUG
                //LogInstance.Log($"count = {count}");
#endif

                if (count == 0)
                {
                    return null;
                }

                var result = Vector3.Zero;

                foreach(var item in mVisionItems)
                {
#if DEBUG
                    //LogInstance.Log($"item = {item}");
#endif

                    result += item.Point;
                }

#if DEBUG
                //LogInstance.Log($"result = {result}");
#endif

                return new Vector3(result.X/count, result.Y / count, result.Z / count);
            }         
        }

        public override string ToString()
        {
            return ToString(0u);
        }

        public string ToString(uint n)
        {
            return this.GetDefaultToStringInformation(n);
        }

        public string PropertiesToSting(uint n)
        {
            var spaces = StringHelper.Spaces(n);
            var nextN = n + 4;
            var sb = new StringBuilder();
            sb.AppendLine($"{spaces}{nameof(EntityId)} = {EntityId}");
            sb.AppendLine($"{spaces}{nameof(IsVisible)} = {IsVisible}");
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
