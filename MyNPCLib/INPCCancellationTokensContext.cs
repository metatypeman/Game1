using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyNPCLib
{
    public interface INPCCancellationTokensContext
    {
        void RegCancellationToken(int taskId, CancellationToken token);
        CancellationToken? GetCancellationToken(int taskId);
        void UnRegCancellationToken(int taskId);
    }
}
