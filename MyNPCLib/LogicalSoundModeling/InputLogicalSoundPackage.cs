using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib.LogicalSoundModeling
{
    public class InputLogicalSoundPackage: IObjectToString
    {
        public InputLogicalSoundPackage(Vector3 pos, float power, IList<RuleInstance> soundFactsList)
        {
            mPosition = pos;
            mPower = power;
            mSoundFactsList = soundFactsList;
        }

        private Vector3 mPosition;

        public Vector3 Position
        {
            get
            {
                return mPosition;
            }
        }

        private float mPower;

        public float Power
        {
            get
            {
                return mPower;
            }
        }

        private IList<RuleInstance> mSoundFactsList;

        public IList<RuleInstance> SoundFactsList
        {
            get
            {
                return mSoundFactsList;
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
            sb.AppendLine($"{spaces}{nameof(Position)} = {mPosition}");
            sb.AppendLine($"{spaces}{nameof(Power)} = {mPower}");
            if(mSoundFactsList == null)
            {
                sb.AppendLine($"{spaces}{nameof(SoundFactsList)} = null");
            }
            else
            {
                sb.AppendLine($"{spaces}Begin {nameof(SoundFactsList)}");
                foreach (var soundFact in mSoundFactsList)
                {
                    sb.Append(soundFact.ToString(nextN));
                }
                sb.AppendLine($"{spaces}End {nameof(SoundFactsList)}");
            }
            return sb.ToString();
        }
    }
}
