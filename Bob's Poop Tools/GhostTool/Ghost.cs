using Microsoft.Test.Xbox.XDRPC;
using Suave;
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using XDevkit;
using XDevkitExt;
using System.IO;
using System.Collections.Generic;
using test;

namespace GhostTool
{
    class Tools
    {
        public static string[] players = new string[0x12];
        public static uint playerStates = 0x8328F100;
        public static uint statAddr = 0x83917FA0;
        public static uint statDeltaAddress = 0x83917FA0;
        public static uint SV_GameSendClientCommand = 0x824C8F60;
        public static uint SV_GameSendServerCommand = 0x824C2748;
        public XDevkit.IXboxConsole xbc;
        public XDevkit.IXboxManager xbm = ((XDevkit.XboxManager)Activator.CreateInstance(System.Type.GetTypeFromCLSID(new Guid("A5EB45D8-F3B6-49B9-984A-0D313AB60342"))));

        public Tools(XDevkit.IXboxConsole lxbc)
        {
            this.xbc = lxbc;
            System.IO.File.ReadAllBytes(Application.StartupPath + @"\Settings.bin");
        }

        public void LoadPlayers()
        {
            players = new string[0x12];
            for (int i = 0; i < 0x12; i++)
            {
                byte[] memory = ((XDevkit.IXboxConsole)xbc).getMemory((uint)((playerStates + (i * 0x3600)) + ((long)0x2f9cL)), 0x20);
                players[i] = ((memory[0] == 0xff) || (memory[0] == 0)) ? string.Empty : Encoding.Default.GetString(memory).Replace("\0", "");
            }

        }

        public void SetStat(byte[] stat)
        {
            ((XDevkit.IXboxConsole)xbc).setMemory(statDeltaAddress, new byte[0x400]);
            ((XDevkit.IXboxConsole)xbc).setMemory(statDeltaAddress, stat);
        }

        public void RemovePlayer(int Index)
        {
            ((XDevkit.IXboxConsole)xbc).setMemory((uint)((playerStates + (Index * 0x3600)) + ((long)0x2f9cL)), new byte[0x20]);
        }

        public byte[] GetStatDeltaCommandBuffer(byte[] FinalStatDelta)
        {
            List<byte> list = new List<byte>();
            byte[] bytes = BitConverter.GetBytes((int)(FinalStatDelta.Length - 2));
            Array.Reverse(bytes);
            list.AddRange(new byte[8]);
            byte[] array = BitConverter.GetBytes(statAddr);
            Array.Reverse(array);
            list.AddRange(array);
            list.AddRange(new byte[4]);
            byte[] collection = new byte[4];
            collection[2] = 3;
            collection[3] = 0xfc;
            list.AddRange(collection);
            list.AddRange(bytes);
            return list.ToArray();
        }

        public void run(int i, List<string> list)
        {
            if (((list.Count == 0) && (i != 0)) && (players[i] != string.Empty))
            {
                ((XDevkit.IXboxConsole)xbc).Call(SV_GameSendServerCommand, new object[] { i, 1, "r \"Get the fuck out of here.\"" });
                RemovePlayer(i);
                Thread.Sleep(0x1f40);
                LoadPlayers();
            }
            if (list.Contains(players[i].ToLower()))
            {
                FileStream input = new FileStream(Application.StartupPath + @"\Customers\" + players[i] + ".bin", FileMode.Open);
                BinaryReader reader = new BinaryReader(input)
                {
                    BaseStream = { Position = 8L }
                };
                for (int j = 0; j < (((((int)input.Length) - 8) / 0x402) + 1); j++)
                {
                    reader.BaseStream.Position = 8 + (j * 0x402);
                    short count = reader.ReadInt16();
                    byte[] stat = reader.ReadBytes(count);
                   SetStat(stat);
                    byte[] statDeltaCommandBuffer = GetStatDeltaCommandBuffer(stat);
                    long num5 = ((long)0xbb170000L) + (0x6c600 * i);
                    ((XDevkit.IXboxConsole)xbc).Call(SV_GameSendClientCommand, new object[] { num5, 1, statDeltaCommandBuffer });
                }
                ((XDevkit.IXboxConsole)xbc).Call(SV_GameSendServerCommand, new object[] { i, 1, "r \"Fuck you nigga enjoy yo shit\"" });
                new BinaryWriter(input) { BaseStream = { Position = 0L } }.Write(DateTime.Now.ToBinary());
                input.Close();
                RemovePlayer(i);
                Thread.Sleep(0x1f40);
                list.Remove(players[i].ToLower());
            }
        }
    }
}
