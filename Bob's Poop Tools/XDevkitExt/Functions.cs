namespace XDevkitExt
{
    using System;
    using System.Runtime.CompilerServices;
    using XDevkit;

    public static class Functions
    {
        public static uint resolveFunction(this XDevkit.IXboxConsole xbc, string moduleName, uint ordinal)
        {
            if (xbc.isConnected())
            {
                uint handle = xbc.XexGetModuleHandle(moduleName);
                return xbc.XexGetProcedureAddress(handle, ordinal);
            }
            return 0;
        }

        public static uint XexGetModuleHandle(this XDevkit.IXboxConsole xbc, string moduleName)
        {
            uint num = 0;
            if (xbc.isConnected())
            {
                xbc.CallSysFunction(xbc.abcdresfunctxrpc("xboxkrnl.exe", 0x195), new object[] { moduleName, num });
                return num;
            }
            return 0;
        }

        public static uint XexGetProcedureAddress(this XDevkit.IXboxConsole xbc, uint handle, uint ordinal)
        {
            uint num = 0;
            if (xbc.isConnected())
            {
                xbc.CallSysFunction(xbc.abcdresfunctxrpc("xboxkrnl.exe", 0x197), new object[] { handle, ordinal, num });
                return num;
            }
            return 0;
        }

        public static void XNotifyQueueUI(this XDevkit.IXboxConsole xbc, uint type, string text)
        {
            byte[] buffer = xbc.WideChar(text);
            xbc.CallSysFunction(xbc.resolveFunction("xam.xex", 0x290), new object[] { type, 0, 2, buffer, 0 });
        }
    }
}

