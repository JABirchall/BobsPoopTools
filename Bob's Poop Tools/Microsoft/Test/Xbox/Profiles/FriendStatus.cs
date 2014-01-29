using System;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace Microsoft.Test.Xbox.Profiles
{
    public enum FriendStatus
    {
        Away = 0x10000,
        Busy = 0x20000,
        Offline = -1,
        Online = 0
    }
}