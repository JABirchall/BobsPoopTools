namespace Microsoft.Test.Xbox.Profiles
{
    using Microsoft.Test.Xbox.XDRPC;
    using System;
    using System.Runtime.CompilerServices;
    using XDevkit;

    public static class ConsoleProfilesManagerFactory
    {
        //private const uint MIN_FLASH_VERSION = 0x20384100;

        //public static ConsoleProfilesManager CreateConsoleProfilesManager(this IXboxConsole console)
        //{
        //    console.ValidateXDRPCProfilesSupport();
        //    return new ConsoleProfilesManager(console, new XDRPCProfileSupport(console));
        //}

        //internal static void ValidateXDRPCProfilesSupport(this IXboxConsole console)
        //{
        //    if (!console.SupportsRPC())
        //    {
        //        throw new ProfilesNotSupportedException();
        //    }
        //    uint systemVersion = new XDRPCXConfigSettingsSupport(console).GetSystemVersion();
        //    if (systemVersion < 0x20384100)
        //    {
        //        throw new ProfilesNotSupportedException(0x20384100, systemVersion);
        //    }
        //}
    }
}

