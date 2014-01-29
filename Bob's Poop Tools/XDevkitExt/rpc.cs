namespace XDevkitExt
{
    using Microsoft.Test.Xbox.XDRPC;
    using System;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using XDevkit;

    public static class rpc
    {
        public static bool activeConnection = false;
        public static bool activeTransfer;
        private static uint[] buffcheck = new uint[15];
        private static uint bufferAddress;
        private static uint bufferAddressRead = 0x91c0adca;
        private static uint bytePointer;
        private static int firstRan = 0;
        private static uint floatPointer;
        public static uint g;
        private static uint meh;
        private static int multiple;
        private static byte[] nulled = new byte[100];
        private static int sa;
        private static uint stringPointer;

        public static uint abcdresfunctxrpc(this XDevkit.IXboxConsole xbCon, string titleID, uint ord)
        {
            if (firstRan == 0)
            {
                byte[] buffer = new byte[4];
                xbCon.DebugTarget.GetMemory(0x91c088ae, 4, buffer, out meh);
                xbCon.DebugTarget.InvalidateMemoryCache(true, 0x91c088ae, 4);
                Array.Reverse(buffer);
                bufferAddress = BitConverter.ToUInt32(buffer, 0);
                firstRan = 1;
                stringPointer = bufferAddress + 0x5dc;
                floatPointer = bufferAddress + 0xa8c;
                bytePointer = bufferAddress + 0xc80;
                xbCon.DebugTarget.SetMemory(bufferAddress, 100, nulled, out meh);
                xbCon.DebugTarget.SetMemory(stringPointer, 100, nulled, out meh);
            }
            byte[] bytes = Encoding.ASCII.GetBytes(titleID);
            xbCon.DebugTarget.SetMemory(stringPointer, (uint) bytes.Length, bytes, out meh);
            long[] argument = new long[2];
            argument[0] = stringPointer;
            string str = Convert.ToString(titleID);
            stringPointer += (uint) (str.Length + 1);
            argument[1] = ord;
            byte[] data = getData(argument);
            xbCon.DebugTarget.SetMemory(bufferAddress + 8, (uint) data.Length, data, out meh);
            byte[] array = BitConverter.GetBytes((uint) 0x82000001);
            Array.Reverse(array);
            xbCon.DebugTarget.SetMemory(bufferAddress, 4, array, out meh);
            Thread.Sleep(50);
            byte[] buffer5 = new byte[4];
            xbCon.DebugTarget.GetMemory(bufferAddress + 0xffc, 4, buffer5, out meh);
            xbCon.DebugTarget.InvalidateMemoryCache(true, bufferAddress + 0xffc, 4);
            Array.Reverse(buffer5);
            return BitConverter.ToUInt32(buffer5, 0);
        }

        public static uint Call(this XDevkit.IXboxConsole xbCon, uint address, params object[] arg)
        {
            if (((XDevkit.IXboxConsole) xbCon).SupportsRPC())
            {
                bool flag = false;
                if (xbCon != null)
                {
                    XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, address);
                    XDRPCArgumentInfo[] args = new XDRPCArgumentInfo[arg.Length];
                    XDRPCArgumentInfo<float>[] infoArray2 = new XDRPCArgumentInfo<float>[arg.Length];
                    for (int i = 0; i < arg.Length; i++)
                    {
                        object obj2 = arg[i];
                        if (obj2 is string)
                        {
                            args[i] = new XDRPCStringArgumentInfo((string) obj2);
                        }
                        else if (obj2 is int)
                        {
                            args[i] = new XDRPCArgumentInfo<int>((int) obj2);
                        }
                        else if (obj2 is uint)
                        {
                            args[i] = new XDRPCArgumentInfo<uint>((uint) obj2);
                        }
                        else if (obj2 is float)
                        {
                            infoArray2[i] = new XDRPCArgumentInfo<float>((float) obj2);
                            flag = true;
                        }
                        else if (obj2 is long)
                        {
                            args[i] = new XDRPCArgumentInfo<long>((long) obj2);
                        }
                        else if (obj2 is byte[])
                        {
                            args[i] = new XDRPCArrayArgumentInfo<byte[]>((byte[]) obj2);
                        }
                        else if (obj2 is short)
                        {
                            args[i] = new XDRPCArgumentInfo<short>((short) obj2);
                        }
                        else if (obj2 is byte)
                        {
                            args[i] = new XDRPCArgumentInfo<byte>((byte) obj2);
                        }
                        else if (obj2 is char[])
                        {
                            args[i] = new XDRPCArrayArgumentInfo<char[]>((char[]) obj2);
                        }
                        else if (obj2 is char)
                        {
                            args[i] = new XDRPCArgumentInfo<char>((char) obj2);
                        }
                        else
                        {
                            Console.WriteLine("Invalid Arg");
                            return 0;
                        }
                    }
                    try
                    {
                        if (flag)
                        {
                            return (uint) ((XDevkit.IXboxConsole) xbCon).ExecuteRPC<float>(options, infoArray2);
                        }
                        return ((XDevkit.IXboxConsole) xbCon).ExecuteRPC<uint>(options, args);
                    }
                    catch (Exception)
                    {
                        return 0;
                    }
                }
                return 0;
            }
            long[] argument = new long[9];
            if (firstRan == 0)
            {
                byte[] buffer = new byte[4];
                xbCon.DebugTarget.GetMemory(0x91c088ae, 4, buffer, out meh);
                xbCon.DebugTarget.InvalidateMemoryCache(true, 0x91c088ae, 4);
                Array.Reverse(buffer);
                bufferAddress = BitConverter.ToUInt32(buffer, 0);
                firstRan = 1;
                stringPointer = bufferAddress + 0x5dc;
                floatPointer = bufferAddress + 0xa8c;
                bytePointer = bufferAddress + 0xc80;
                xbCon.DebugTarget.SetMemory(bufferAddress, 100, nulled, out meh);
                xbCon.DebugTarget.SetMemory(stringPointer, 100, nulled, out meh);
            }
            if (bufferAddress == 0)
            {
                byte[] buffer2 = new byte[4];
                xbCon.DebugTarget.GetMemory(0x91c088ae, 4, buffer2, out meh);
                xbCon.DebugTarget.InvalidateMemoryCache(true, 0x91c088ae, 4);
                Array.Reverse(buffer2);
                bufferAddress = BitConverter.ToUInt32(buffer2, 0);
            }
            stringPointer = bufferAddress + 0x5dc;
            floatPointer = bufferAddress + 0xa8c;
            bytePointer = bufferAddress + 0xc80;
            int num4 = 0;
            int index = 0;
            foreach (object obj3 in arg)
            {
                if (obj3 is byte)
                {
                    byte[] buffer3 = (byte[]) obj3;
                    argument[index] = BitConverter.ToUInt32(buffer3, 0);
                }
                else if (obj3 is byte[])
                {
                    byte[] buffer4 = (byte[]) obj3;
                    xbCon.DebugTarget.SetMemory(bytePointer, (uint) buffer4.Length, buffer4, out meh);
                    argument[index] = bytePointer;
                    bytePointer += (uint) (buffer4.Length + 2);
                }
                else if (obj3 is float)
                {
                    byte[] buffer5 = BitConverter.GetBytes(float.Parse(Convert.ToString(obj3)));
                    xbCon.DebugTarget.SetMemory(floatPointer, (uint) buffer5.Length, buffer5, out meh);
                    argument[index] = floatPointer;
                    floatPointer += (uint) (buffer5.Length + 2);
                }
                else if (obj3 is float[])
                {
                    byte[] dst = new byte[12];
                    int num6 = 0;
                    for (num6 = 0; num6 <= 2; num6++)
                    {
                        byte[] buffer7 = new byte[4];
                        Buffer.BlockCopy((Array) obj3, num6 * 4, buffer7, 0, 4);
                        Array.Reverse(buffer7);
                        Buffer.BlockCopy(buffer7, 0, dst, 4 * num6, 4);
                    }
                    xbCon.DebugTarget.SetMemory(floatPointer, (uint) dst.Length, dst, out meh);
                    argument[index] = floatPointer;
                    floatPointer += 2;
                }
                else if (obj3 is string)
                {
                    byte[] buffer8 = Encoding.ASCII.GetBytes(Convert.ToString(obj3));
                    xbCon.DebugTarget.SetMemory(stringPointer, (uint) buffer8.Length, buffer8, out meh);
                    argument[index] = stringPointer;
                    string str = Convert.ToString(obj3);
                    stringPointer += (uint) (str.Length + 1);
                }
                else
                {
                    argument[index] = Convert.ToInt64(obj3);
                }
                num4++;
                index++;
            }
            byte[] data = getData(argument);
            xbCon.DebugTarget.SetMemory(bufferAddress + 8, (uint) data.Length, data, out meh);
            byte[] bytes = BitConverter.GetBytes(num4);
            Array.Reverse(bytes);
            xbCon.DebugTarget.SetMemory(bufferAddress + 4, 4, bytes, out meh);
            Thread.Sleep(0);
            byte[] array = BitConverter.GetBytes(address);
            Array.Reverse(array);
            xbCon.DebugTarget.SetMemory(bufferAddress, 4, array, out meh);
            Thread.Sleep(50);
            byte[] buffer12 = new byte[4];
            xbCon.DebugTarget.GetMemory(bufferAddress + 0xffc, 4, buffer12, out meh);
            xbCon.DebugTarget.InvalidateMemoryCache(true, bufferAddress + 0xffc, 4);
            Array.Reverse(buffer12);
            return BitConverter.ToUInt32(buffer12, 0);
        }

        public static uint CallSysFunction(this XDevkit.IXboxConsole xbCon, uint address, params object[] arg)
        {
            long[] argument = new long[9];
            if (firstRan == 0)
            {
                byte[] buffer = new byte[4];
                xbCon.DebugTarget.GetMemory(0x91c088ae, 4, buffer, out meh);
                xbCon.DebugTarget.InvalidateMemoryCache(true, 0x91c088ae, 4);
                Array.Reverse(buffer);
                bufferAddress = BitConverter.ToUInt32(buffer, 0);
                firstRan = 1;
                stringPointer = bufferAddress + 0x5dc;
                floatPointer = bufferAddress + 0xa8c;
                bytePointer = bufferAddress + 0xc80;
                xbCon.DebugTarget.SetMemory(bufferAddress, 100, nulled, out meh);
                xbCon.DebugTarget.SetMemory(stringPointer, 100, nulled, out meh);
            }
            if (bufferAddress == 0)
            {
                byte[] buffer2 = new byte[4];
                xbCon.DebugTarget.GetMemory(0x91c088ae, 4, buffer2, out meh);
                xbCon.DebugTarget.InvalidateMemoryCache(true, 0x91c088ae, 4);
                Array.Reverse(buffer2);
                bufferAddress = BitConverter.ToUInt32(buffer2, 0);
            }
            stringPointer = bufferAddress + 0x5dc;
            floatPointer = bufferAddress + 0xa8c;
            bytePointer = bufferAddress + 0xc80;
            int num = 0;
            int index = 0;
            argument[index] = address;
            index++;
            foreach (object obj2 in arg)
            {
                if (obj2 is byte)
                {
                    byte[] buffer3 = (byte[]) obj2;
                    argument[index] = BitConverter.ToUInt32(buffer3, 0);
                }
                else if (obj2 is byte[])
                {
                    byte[] buffer4 = (byte[]) obj2;
                    xbCon.DebugTarget.SetMemory(bytePointer, (uint) buffer4.Length, buffer4, out meh);
                    argument[index] = bytePointer;
                    bytePointer += (uint) (buffer4.Length + 2);
                }
                else if (obj2 is float)
                {
                    byte[] buffer5 = BitConverter.GetBytes(float.Parse(Convert.ToString(obj2)));
                    xbCon.DebugTarget.SetMemory(floatPointer, (uint) buffer5.Length, buffer5, out meh);
                    argument[index] = floatPointer;
                    floatPointer += (uint) (buffer5.Length + 2);
                }
                else if (obj2 is float[])
                {
                    byte[] dst = new byte[12];
                    int num3 = 0;
                    for (num3 = 0; num3 <= 2; num3++)
                    {
                        byte[] buffer7 = new byte[4];
                        Buffer.BlockCopy((Array) obj2, num3 * 4, buffer7, 0, 4);
                        Array.Reverse(buffer7);
                        Buffer.BlockCopy(buffer7, 0, dst, 4 * num3, 4);
                    }
                    xbCon.DebugTarget.SetMemory(floatPointer, (uint) dst.Length, dst, out meh);
                    argument[index] = floatPointer;
                    floatPointer += 2;
                }
                else if (obj2 is string)
                {
                    byte[] buffer8 = Encoding.ASCII.GetBytes(Convert.ToString(obj2));
                    xbCon.DebugTarget.SetMemory(stringPointer, (uint) buffer8.Length, buffer8, out meh);
                    argument[index] = stringPointer;
                    string str = Convert.ToString(obj2);
                    stringPointer += (uint) (str.Length + 1);
                }
                else
                {
                    argument[index] = Convert.ToInt64(obj2);
                }
                num++;
                index++;
            }
            byte[] data = getData(argument);
            xbCon.DebugTarget.SetMemory(bufferAddress + 8, (uint) data.Length, data, out meh);
            byte[] bytes = BitConverter.GetBytes(num);
            Array.Reverse(bytes);
            xbCon.DebugTarget.SetMemory(bufferAddress + 4, 4, bytes, out meh);
            Thread.Sleep(0);
            byte[] array = BitConverter.GetBytes((uint) 0x82000000);
            Array.Reverse(array);
            xbCon.DebugTarget.SetMemory(bufferAddress, 4, array, out meh);
            Thread.Sleep(50);
            byte[] buffer12 = new byte[4];
            xbCon.DebugTarget.GetMemory(bufferAddress + 0xffc, 4, buffer12, out meh);
            xbCon.DebugTarget.InvalidateMemoryCache(true, bufferAddress + 0xffc, 4);
            Array.Reverse(buffer12);
            return BitConverter.ToUInt32(buffer12, 0);
        }

        public static void clearBuffer(this XDevkit.IXboxConsole xbCon)
        {
            byte[] data = new byte[4];
            byte[] buffer2 = new byte[0x1000];
            xbCon.DebugTarget.GetMemory(0x91c088ae, 4, data, out meh);
            xbCon.DebugTarget.InvalidateMemoryCache(true, 0x91c088ae, 4);
            Array.Reverse(data);
            bufferAddress = BitConverter.ToUInt32(data, 0);
            xbCon.DebugTarget.SetMemory(bufferAddress, 0x1000, buffer2, out meh);
        }

        public static byte[] getData(long[] argument)
        {
            byte[] array = new byte[argument.Length * 8];
            int index = 0;
            foreach (long num2 in argument)
            {
                byte[] bytes = BitConverter.GetBytes(num2);
                Array.Reverse(bytes);
                bytes.CopyTo(array, index);
                index += 8;
            }
            return array;
        }

        public static float[] toFloatArray(double[] arr)
        {
            if (arr == null)
            {
                return null;
            }
            int length = arr.Length;
            float[] numArray = new float[length];
            for (int i = 0; i < length; i++)
            {
                numArray[i] = (float) arr[i];
            }
            return numArray;
        }

        public static byte[] WideChar(this XDevkit.IXboxConsole xbCon, string text)
        {
            byte[] buffer = new byte[(text.Length * 2) + 2];
            int index = 1;
            buffer[0] = 0;
            foreach (char ch in text)
            {
                buffer[index] = Convert.ToByte(ch);
                index += 2;
            }
            return buffer;
        }
    }
}

