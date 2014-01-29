namespace GhostTool
{
    using Microsoft.Test.Xbox.XDRPC;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Net;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;
    using XDevkit;
    using XRPCLib;

    public class Main : Form
    {
        private ToolStripMenuItem aboutToolStripMenuItem;
        private ToolStripMenuItem addCustomerToolStripMenuItem;
        private ToolStripMenuItem beginTimerToolStripMenuItem;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private IContainer components = null;
        private ToolStripMenuItem endTimerToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ListView listView1;
        private MenuStrip menuStrip1;
        private string[] players = new string[0x12];
        private uint playerStates = 0x8328b700;
        private System.Windows.Forms.Timer refreshTimer = new System.Windows.Forms.Timer();
        private uint statAddr = 0x83901da0;
        private uint statDeltaAddress = 0x83901da0;
        private uint SV_GameSendClientCommand = 0x824C44B0;
        private uint SV_GameSendServerCommand = 0x824BDC98;
        private ToolStripMenuItem toolsToolStripMenuItem;

        public Main()
        {
            this.InitializeComponent();
            try
            {
                System.IO.File.ReadAllBytes(Application.StartupPath + @"\Settings.bin");
                this.Devkit = false;
            }
            catch
            {
                this.Devkit = true;
            }
            this.Connect();
            this.refreshTimer.Interval = 0xbb8;
            this.refreshTimer.Tick += new EventHandler(this.refreshTimer_Tick);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Created by Supermodder911. Fuck all of you guys, except CraigChrist =)");
        }

        private void addCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AddCustomerForm().ShowDialog();
        }

        private void beginTimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.refreshTimer.Start();
        }

        private uint CallFunction(uint address, params object[] args)
        {
            if (this.Devkit)
            {
                ((XDevkit.IXboxConsole) this.Console).ExecuteRPC<uint>(XDRPCMode.Title, address, args);
                return 0;
            }
            return this.JtagConsole.Call(address, args);
        }

        private void Connect()
        {
            if (this.Devkit)
            {
                XDevkit.XboxManager manager = (XDevkit.XboxManager) Activator.CreateInstance(System.Type.GetTypeFromCLSID(new Guid("A5EB45D8-F3B6-49B9-984A-0D313AB60342")));
                this.Console = manager.OpenConsole(manager.DefaultConsole);
            }
            else
            {
                if (this.JtagConsole == null)
                {
                    this.JtagConsole = new XRPC();
                }
                this.JtagConsole.Connect();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void endTimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.refreshTimer.Stop();
        }

        private string GetIP()
        {
            StreamReader reader = new StreamReader(WebRequest.Create("http://icanhazip.com/").GetResponse().GetResponseStream());
            return reader.ReadToEnd();
        }

        private byte[] GetMemory(uint address, uint size)
        {
            byte[] buffer = new byte[size];
            if (this.Devkit)
            {
                return buffer;
            }
            return this.JtagConsole.GetMemory(address, size);
        }

        private byte[] GetStatDeltaCommandBuffer(byte[] FinalStatDelta)
        {
            List<byte> list = new List<byte>();
            byte[] bytes = BitConverter.GetBytes((int) (FinalStatDelta.Length - 2));
            Array.Reverse(bytes);
            list.AddRange(new byte[8]);
            byte[] array = BitConverter.GetBytes(this.statAddr);
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

        private void InitializeComponent()
        {
            this.listView1 = new ListView();
            this.columnHeader1 = new ColumnHeader();
            this.columnHeader2 = new ColumnHeader();
            this.menuStrip1 = new MenuStrip();
            this.toolsToolStripMenuItem = new ToolStripMenuItem();
            this.beginTimerToolStripMenuItem = new ToolStripMenuItem();
            this.endTimerToolStripMenuItem = new ToolStripMenuItem();
            this.addCustomerToolStripMenuItem = new ToolStripMenuItem();
            this.helpToolStripMenuItem = new ToolStripMenuItem();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            base.SuspendLayout();
            this.listView1.Columns.AddRange(new ColumnHeader[] { this.columnHeader1, this.columnHeader2 });
            this.listView1.Dock = DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new Point(0, 0x18);
            this.listView1.Name = "listView1";
            this.listView1.Size = new Size(0x209, 0x166);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.Details;
            this.listView1.SelectedIndexChanged += new EventHandler(this.listView1_SelectedIndexChanged);
            this.columnHeader1.Text = "Customer Gamertag";
            this.columnHeader1.Width = 0x137;
            this.columnHeader2.Text = "DateTime Processed";
            this.columnHeader2.Width = 0xa8;
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.toolsToolStripMenuItem, this.helpToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(0x209, 0x18);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.beginTimerToolStripMenuItem, this.endTimerToolStripMenuItem, this.addCustomerToolStripMenuItem });
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new Size(0x30, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            this.beginTimerToolStripMenuItem.Name = "beginTimerToolStripMenuItem";
            this.beginTimerToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.beginTimerToolStripMenuItem.Text = "Begin Timer";
            this.beginTimerToolStripMenuItem.Click += new EventHandler(this.beginTimerToolStripMenuItem_Click);
            this.endTimerToolStripMenuItem.Name = "endTimerToolStripMenuItem";
            this.endTimerToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.endTimerToolStripMenuItem.Text = "End Timer";
            this.endTimerToolStripMenuItem.Click += new EventHandler(this.endTimerToolStripMenuItem_Click);
            this.addCustomerToolStripMenuItem.Name = "addCustomerToolStripMenuItem";
            this.addCustomerToolStripMenuItem.Size = new Size(0x98, 0x16);
            this.addCustomerToolStripMenuItem.Text = "Add Customer";
            this.addCustomerToolStripMenuItem.Click += new EventHandler(this.addCustomerToolStripMenuItem_Click);
            this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.aboutToolStripMenuItem });
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new Size(0x2c, 20);
            this.helpToolStripMenuItem.Text = "Help";
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(0x6b, 0x16);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
            base.ClientSize = new Size(0x209, 0x17e);
            base.Controls.Add(this.listView1);
            base.Controls.Add(this.menuStrip1);
            base.MainMenuStrip = this.menuStrip1;
            base.Name = "Main";
            this.Text = "CoD: Ghosts Stat Tool";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void LoadPlayers()
        {
            this.Connect();
            this.listView1.Items.Clear();
            this.players = new string[0x12];
            for (int i = 0; i < 0x12; i++)
            {
                byte[] memory = this.GetMemory((uint) ((this.playerStates + (i * 0x3600)) + ((long) 0x2f9cL)), 0x20);
                this.players[i] = ((memory[0] == 0xff) || (memory[0] == 0)) ? string.Empty : Encoding.Default.GetString(memory).Replace("\0", "");
            }
        }

        private void ProcessCustomerOrders()
        {
            BinaryReader reader;
            this.LoadPlayers();
            List<string> list = new List<string>();
            foreach (string str in Directory.GetFiles(Application.StartupPath + @"\Customers", "*.bin"))
            {
                string str2 = str.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries)[str.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Length - 1].Replace(".bin", "");
                reader = new BinaryReader(new FileStream(str, FileMode.Open));
                long dateData = reader.ReadInt64();
                reader.Close();
                ListViewItem item2 = new ListViewItem {
                    Text = str2
                };
                item2.SubItems.Add((dateData == 0L) ? "Not yet processed" : DateTime.FromBinary(dateData).ToString("g"));
                if (dateData == 0L)
                {
                    list.Add(str2.ToLower());
                }
                this.listView1.Items.Add(item2);
            }
            for (int i = 0; i < 0x12; i++)
            {
                if (((list.Count == 0) && (i != 0)) && (this.players[i] != string.Empty))
                {
                    this.CallFunction(this.SV_GameSendServerCommand, new object[] { i, 1, "r \"Get the fuck out of here.\"" });
                    this.RemovePlayer(i);
                    Thread.Sleep(0x1f40);
                    this.LoadPlayers();
                    break;
                }
                if (list.Contains(this.players[i].ToLower()))
                {
                    FileStream input = new FileStream(Application.StartupPath + @"\Customers\" + this.players[i] + ".bin", FileMode.Open);
                    reader = new BinaryReader(input) {
                        BaseStream = { Position = 8L }
                    };
                    for (int j = 0; j < (((((int) input.Length) - 8) / 0x402) + 1); j++)
                    {
                        reader.BaseStream.Position = 8 + (j * 0x402);
                        short count = reader.ReadInt16();
                        byte[] stat = reader.ReadBytes(count);
                        this.SetStat(stat);
                        byte[] statDeltaCommandBuffer = this.GetStatDeltaCommandBuffer(stat);
                        long num5 = ((long) 0xbb170000L) + (0x6c600 * i);
                        this.CallFunction(this.SV_GameSendClientCommand, new object[] { num5, 1, statDeltaCommandBuffer });
                    }
                    this.CallFunction(this.SV_GameSendServerCommand, new object[] { i, 1, "r \"Fuck you nigga enjoy yo shit\"" });
                    new BinaryWriter(input) { BaseStream = { Position = 0L } }.Write(DateTime.Now.ToBinary());
                    input.Close();
                    this.RemovePlayer(i);
                    Thread.Sleep(0x1f40);
                    list.Remove(this.players[i].ToLower());
                }
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.ProcessCustomerOrders();
        }

        private void RemovePlayer(int Index)
        {
            this.SetMemory((uint) ((this.playerStates + (Index * 0x3600)) + ((long) 0x2f9cL)), 0x20, new byte[0x20]);
        }

        private void SetMemory(uint address, uint size, byte[] data)
        {
            uint bytesWritten = 0;
            if (this.Devkit)
            {
                this.Console.DebugTarget.SetMemory(address, size, data, out bytesWritten);
            }
            else
            {
                this.JtagConsole.SetMemory(address, data);
            }
        }

        private void SetStat(byte[] stat)
        {
            this.SetMemory(this.statDeltaAddress, 0x400, new byte[0x400]);
            this.SetMemory(this.statDeltaAddress, (uint) stat.Length, stat);
        }

        public XDevkit.XboxConsole Console { get; set; }

        public bool Devkit { get; set; }

        public XRPC JtagConsole { get; set; }
    }
}

