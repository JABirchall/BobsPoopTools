using System;

namespace Tools
{
    internal class Addresses
    {
        //free memory in XAM
        public static uint g_freememory = 0x81AA2000;

        //XAM 
        public static uint g_rguserinfo = 0x81AA2C6C;
        public static uint g_XamUserGetXUID = 0x816D7A48;
        public static uint g_XUserFindUserAddress = 0x81825D18;

        //XAM DEVKIT
        public static uint g_rguserinfoDEV = 0x81D44A94;
        public static uint g_XamUserGetXUIDDEV = 0x817A25B8;
        public static uint g_XUserFindUserAddressDEV = 0x819CD720;
    }
}

