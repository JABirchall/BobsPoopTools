namespace XDevkitExt
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using XDevkit;

    public static class Utils
    {
        public static void LaunchTitle(this XDevkit.IXboxConsole xbc, string path)
        {
            string mediaDirectory = path;
            mediaDirectory.Replace("default_mp.xex", "").Replace("default.xex", "");
            xbc.Reboot(path, mediaDirectory, null, XDevkit.XboxRebootFlags.Title);
        }

        public static string ReadNullTermString(this XDevkit.IXboxConsole xbc, uint Address)
        {
            byte[] buffer = new byte[1];
            uint num = 0;
            List<byte> list = new List<byte>();
            while (true)
            {
                buffer = xbc.getMemory(Address + num, 1);
                if (buffer[0] == 0)
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    return encoding.GetString(list.ToArray());
                }
                list.Add(buffer[0]);
                num++;
            }
        }

        public static void Reboot(this XDevkit.IXboxConsole xbc)
        {
            xbc.SendTextCommand("reboot");
        }

        public static byte[] ReverseBytes(byte[] inArray)
        {
            int index = inArray.Length - 1;
            for (int i = 0; i < (inArray.Length / 2); i++)
            {
                byte num = inArray[i];
                inArray[i] = inArray[index];
                inArray[index] = num;
                index--;
            }
            return inArray;
        }

        public static void Screenshot(this XDevkit.IXboxConsole xbc, string path)
        {
            xbc.ScreenShot(path);
        }

        public static string SendTextCommand(this XDevkit.IXboxConsole xbc, string Command)
        {
            uint connection = xbc.OpenConnection(null);
            string response = "";
            xbc.SendTextCommand(connection, Command, out response);
            if (!(response.Contains("202") | response.Contains("203")))
            {
                return response;
            }
            try
            {
                string line = "";
                xbc.ReceiveSocketLine(connection, out line);
                if (line.Length > 0)
                {
                    if (line[0] == '.')
                    {
                        return response;
                    }
                    response = response + Environment.NewLine + line;
                }
            }
            catch
            {
            }
            return "FAIL";
        }

        public static byte[] StringToByteArray(string hex)
        {
            return (from x in Enumerable.Range(0, hex.Length)
                where (x % 2) == 0
                select Convert.ToByte(hex.Substring(x, 2), 0x10)).ToArray<byte>();
        }

        public static void Unfreeze(this XDevkit.IXboxConsole xbc)
        {
            xbc.SendTextCommand("go");
        }
    }
}

