using SymOntoClay.UnityAsset.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymOntoClay.Unity3D
{
    public class PlatformLogger: IPlatformLogger
    {
        private static readonly PlatformLogger __instance = new PlatformLogger();

        /// <summary>
        /// Gets instance of the class.
        /// </summary>
        public static PlatformLogger Instance => __instance;

        private PlatformLogger()
        {
        }
    }
}
