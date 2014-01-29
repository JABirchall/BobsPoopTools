namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Runtime.CompilerServices;

    public class TooManyProfilesException : ProfilesException
    {
        //public TooManyProfilesException(string deviceName, uint max) : base("The {0} in the console already stores the maximum of {1} profiles. A profile must be deleted before a new one can be created.", new object[] { deviceName, max })
        //{
        //    this.DeviceName = deviceName;
        //    this.MaxProfileCount = max;
        //}

        //public string DeviceName { get; private set; }

        //public uint MaxProfileCount { get; private set; }
    }
}

