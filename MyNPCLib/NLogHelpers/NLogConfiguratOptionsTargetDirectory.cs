using System;
using System.Collections.Generic;
using System.Text;

namespace MyNPCLib.NLogHelpers
{
    public enum NLogConfiguratOptionsTargetDirectory
    {
        None,
        USERPROFILE,
        VisualStudioDir,
        APPDATA,
        LOCALAPPDATA,
        Path,
        PathOrUserProfileIfNotExist
    }
}
