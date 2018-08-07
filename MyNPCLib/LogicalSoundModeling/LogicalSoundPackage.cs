using MyNPCLib.PersistLogicalData;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MyNPCLib.LogicalSoundModeling
{
    public class LogicalSoundPackage: IObjectToString
    {
        public LogicalSoundPackage(Vector3 pos, IList<RuleInstance> soundFactsList)
        {
            mPosition = pos;
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
            return sb.ToString();
        }
    }
}
