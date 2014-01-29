using System;

namespace EatonWorks.Auxiliary.Miscellaneous
{
    ///OS C# class 
    ///Created by Eaton 
    ///Usage permitted in open or closed source environments. 

    public static class OS
    {
        public static bool IsXPOrAbove()
        {
            if (Environment.OSVersion.Version.Major > 5) return true;
            return Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1;
        }

        public static bool IsVistaOrAbove()
        {
            if (Environment.OSVersion.Version.Major > 6) return true;
            return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 0;
        }

        public static bool Is7OrAbove()
        {
            if (Environment.OSVersion.Version.Major > 6) return true;
            return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1;
        }

        public static bool Is8OrAbove()
        {
            if (Environment.OSVersion.Version.Major > 6) return true;
            return Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2;
        }
    }
}
