using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using XDevkit;

namespace XDevkitExt
{
    public static class console
    {
        public static uint Connection_Code;
        private static string debuga;
        private static string debugb;
        private static byte[] myBuffer = new byte[0x20];
        private static uint xbOut;
        public static XDevkit.IXboxConsole Xbox_Console;
        public static XDevkit.IXboxManager Xbox_Manager;
        public static string Xbox_Type;

        public static bool Connect(string XboxName = null)
        {
            if (!Xbox_Console.isConnected())
            {
                Xbox_Manager = (XDevkit.XboxManager) Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("A5EB45D8-F3B6-49B9-984A-0D313AB60342")));
                if (string.IsNullOrEmpty(XboxName))
                {
                    Xbox_Console = Xbox_Manager.OpenConsole(Xbox_Manager.DefaultConsole);
                }
                else
                {
                    Xbox_Console = Xbox_Manager.OpenConsole(XboxName);
                }
                try
                {
                    Connection_Code = Xbox_Console.OpenConnection(null);
                    if (!Xbox_Console.isConnected())
                    {
                        Xbox_Console.DebugTarget.ConnectAsDebugger("XDevkitExt", XDevkit.XboxDebugConnectFlags.Force);
                    }
                    return Xbox_Console.isConnected();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }

        public static string ConsoleType(this XDevkit.IXboxConsole xbc)
        {
            if (!xbc.isConnected())
            {
                throw new Exception("Please connect to the console first.");
            }
            return Xbox_Type;
        }

        public static byte[] getMemory(this XDevkit.IXboxConsole xbc, uint address, uint numBytes)
        {
            byte[] data = new byte[numBytes];
            if (xbc.isConnected())
            {
                xbc.DebugTarget.GetMemory(address, numBytes, data, out xbOut);
                xbc.DebugTarget.InvalidateMemoryCache(true, address, numBytes);
            }
            return data;
        }

        public static bool isConnected(this XDevkit.IXboxConsole xbc)
        {
            try
            {
                if (xbc.DebugTarget.IsDebuggerConnected(out debuga, out debugb))
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool ReadBool(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 1);
            return (myBuffer[0] != 0);
        }

        public static byte ReadByte(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 1);
            return myBuffer[0];
        }

        public static float ReadFloat(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 4);
            Array.Reverse(myBuffer, 0, 4);
            return BitConverter.ToSingle(myBuffer, 0);
        }

        public static short ReadInt16(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 2);
            Array.Reverse(myBuffer, 0, 2);
            return BitConverter.ToInt16(myBuffer, 0);
        }

        public static int ReadInt32(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 4);
            Array.Reverse(myBuffer, 0, 4);
            return BitConverter.ToInt32(myBuffer, 0);
        }

        public static long ReadInt64(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 8);
            Array.Reverse(myBuffer, 0, 8);
            return BitConverter.ToInt64(myBuffer, 0);
        }

        public static sbyte ReadSByte(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 1);
            return (sbyte) myBuffer[0];
        }

        public static string ReadString(this XDevkit.IXboxConsole xbc, uint offset)
        {
            return xbc.ReadString(offset, myBuffer);
        }

        public static string ReadString(this XDevkit.IXboxConsole xbc, uint offset, byte[] readBuffer)
        {
            readBuffer = xbc.getMemory(offset, (uint) readBuffer.Length);
            return new string(Encoding.ASCII.GetChars(readBuffer)).Split(new char[1])[0];
        }

        public static ushort ReadUInt16(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 2);
            Array.Reverse(myBuffer, 0, 2);
            return BitConverter.ToUInt16(myBuffer, 0);
        }

        public static uint ReadUInt32(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 4);
            Array.Reverse(myBuffer, 0, 4);
            return BitConverter.ToUInt32(myBuffer, 0);
        }

        public static ulong ReadUInt64(this XDevkit.IXboxConsole xbc, uint offset)
        {
            myBuffer = xbc.getMemory(offset, 8);
            Array.Reverse(myBuffer, 0, 8);
            return BitConverter.ToUInt64(myBuffer, 0);
        }

        public static void setMemory(this XDevkit.IXboxConsole xbc, uint address, byte[] data)
        {
            if (xbc.isConnected())
            {
                xbc.DebugTarget.SetMemory(address, (uint) data.Length, data, out xbOut);
            }
        }

        public static void WriteBool(this XDevkit.IXboxConsole xbc, uint offset, bool input)
        {
            myBuffer[0] = input ? ((byte) 1) : ((byte) 0);
            if (xbc.isConnected())
            {
                xbc.DebugTarget.SetMemory(offset, 1, myBuffer, out xbOut);
            }
        }

        public static void WriteByte(this XDevkit.IXboxConsole xbc, uint offset, byte input)
        {
            xbc.setMemory(offset, new byte[] { input });
        }

        public static void WriteFloat(this XDevkit.IXboxConsole xbc, uint offset, float input)
        {
            BitConverter.GetBytes(input).CopyTo(myBuffer, 0);
            Array.Reverse(myBuffer, 0, 4);
            xbc.setMemory(offset, new byte[] { myBuffer[0], myBuffer[1], myBuffer[2], myBuffer[3] });
        }

        public static void WriteInt16(this XDevkit.IXboxConsole xbc, uint offset, short input)
        {
            BitConverter.GetBytes(input).CopyTo(myBuffer, 0);
            Array.Reverse(myBuffer, 0, 2);
            xbc.setMemory(offset, new byte[] { myBuffer[0], myBuffer[1] });
        }

        public static void WriteInt32(this XDevkit.IXboxConsole xbc, uint offset, int input)
        {
            BitConverter.GetBytes(input).CopyTo(myBuffer, 0);
            Array.Reverse(myBuffer, 0, 4);
            xbc.setMemory(offset, new byte[] { myBuffer[0], myBuffer[1], myBuffer[2], myBuffer[3] });
        }

        public static void WriteInt64(this XDevkit.IXboxConsole xbc, uint offset, long input)
        {
            BitConverter.GetBytes(input).CopyTo(myBuffer, 0);
            Array.Reverse(myBuffer, 0, 8);
            xbc.setMemory(offset, new byte[] { myBuffer[0], myBuffer[1], myBuffer[2], myBuffer[3], myBuffer[4], myBuffer[5], myBuffer[6], myBuffer[7] });
        }

        public static void WriteSByte(this XDevkit.IXboxConsole xbc, uint offset, sbyte input)
        {
            xbc.setMemory(offset, new byte[] { (byte) input });
        }

        public static void WriteUInt16(this XDevkit.IXboxConsole xbc, uint offset, ushort input)
        {
            BitConverter.GetBytes(input).CopyTo(myBuffer, 0);
            Array.Reverse(myBuffer, 0, 2);
            xbc.setMemory(offset, new byte[] { myBuffer[0], myBuffer[1] });
        }

        public static void WriteUInt32(this XDevkit.IXboxConsole xbc, uint offset, uint input)
        {
            BitConverter.GetBytes(input).CopyTo(myBuffer, 0);
            Array.Reverse(myBuffer, 0, 4);
            xbc.setMemory(offset, new byte[] { myBuffer[0], myBuffer[1], myBuffer[2], myBuffer[3] });
        }

        public static void WriteUInt64(this XDevkit.IXboxConsole xbc, uint offset, ulong input)
        {
            BitConverter.GetBytes(input).CopyTo(myBuffer, 0);
            Array.Reverse(myBuffer, 0, 8);
            xbc.setMemory(offset, new byte[] { myBuffer[0], myBuffer[1], myBuffer[2], myBuffer[3], myBuffer[4], myBuffer[5], myBuffer[6], myBuffer[7] });
        }
    }
}

