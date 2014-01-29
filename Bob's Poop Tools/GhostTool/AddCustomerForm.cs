namespace GhostTool
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using XDevkit;

    public class AddCustomerForm : Form
    {
        private int accuracy = 0x1d;
        private int assists = 0x21;
        private Button button1;
        private Button button2;
        private int captures = 0x45;
        private CheckBox checkBox1;
        private IContainer components = null;
        private int confirmed = 0x49;
        private int deaths = 0x51;
        private int defends = 0x55;
        private int defuses = 0x59;
        private int denied = 0x5d;
        private int destructions = 0x61;
        private int gamesplayed = 0x69;
        private int[] GhostLevels = new int[] { 
            0, 0x3e8, 0x9c4, 0x1388, 0x2710, 0x4e20, 0x7530, 0x9c40, 0xc350, 0xea60, 0x11170, 0x13880, 0x15f90, 0x186a0, 0x1d4c0, 0x222e0, 
            0x27100, 0x2bf20, 0x30d40, 0x35b60, 0x3a980, 0x3f7a0, 0x445c0, 0x493e0, 0x4e200, 0x53020, 0x57e40, 0x5cc60, 0x61a80, 0x668a0, 0x6b6c0, 0x704e0, 
            0x75300, 0x7a120, 0x7ef40, 0x83d60, 0x88b80, 0x8d9a0, 0x927c0, 0x975e0, 0x9c400, 0xa1220, 0xa6040, 0xaae60, 0xafc80, 0xb4aa0, 0xb98c0, 0xbe6e0, 
            0xc3500, 0xc8320, 0xcd140, 0xd4670, 0xdbba0, 0xe30d0, 0xea600, 0xf1b30, 0xfb770, 0x1053b0, 0x111700, 0x11da50, 0x12c4b0
         };
        private int headshots = 0x6d;
        private int hits = 0x71;
        private int kdratio = 0x75;
        private int kills = 0x79;
        private int killstreak = 0x7d;
        private Label label1;
        private Label label10;
        private Label label11;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private int losses = 0x81;
        private int misses = 0x86;
        private NumericUpDown numericUpDown1;
        private NumericUpDown numericUpDown10;
        private NumericUpDown numericUpDown11;
        private NumericUpDown numericUpDown2;
        private NumericUpDown numericUpDown3;
        private NumericUpDown numericUpDown4;
        private NumericUpDown numericUpDown5;
        private NumericUpDown numericUpDown6;
        private NumericUpDown numericUpDown7;
        private NumericUpDown numericUpDown8;
        private NumericUpDown numericUpDown9;
        private int plants = 150;
        private int prestige = 0x4f1c;
        private int prestigeData = 0x42a2;
        private PropertyGrid propertyGrid1;
        private int returns = 0x9e;
        private int score = 0xa2;
        private int squadmemberlen = 0x564;
        private int squadmembers = 0xcb8;
        private int squadmemberxp = 0x322;
        private int squadpoints = 0x4c24;
        private GhostStats stat;
        private int suicides = 0xa6;
        private int ties = 0xae;
        private int timeplayed = 190;
        private int totalshots = 0xc2;
        private int winlossratio = 0xc6;
        private int wins = 0xca;
        private int winstreak = 0xce;

        public AddCustomerForm()
        {
            this.InitializeComponent();
            XDevkit.XboxManager manager = (XDevkit.XboxManager) Activator.CreateInstance(System.Type.GetTypeFromCLSID(new Guid("A5EB45D8-F3B6-49B9-984A-0D313AB60342")));
            this.Console = manager.OpenConsole(manager.DefaultConsole);
        }

        private int acumulatedLevel()
        {
            return ((((((((((((int) this.numericUpDown2.Value) + ((int) this.numericUpDown3.Value)) + ((int) this.numericUpDown4.Value)) + ((int) this.numericUpDown5.Value)) + ((int) this.numericUpDown6.Value)) + ((int) this.numericUpDown7.Value)) + ((int) this.numericUpDown8.Value)) + ((int) this.numericUpDown9.Value)) + ((int) this.numericUpDown10.Value)) + ((int) this.numericUpDown11.Value)) - (this.curPrestige() * 60));
        }

        private void AddStat(List<byte> StatDeltas, byte[] Delta)
        {
            StatDeltas.AddRange(Delta);
            StatDeltas.AddRange(new byte[2]);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] array = new byte[4];
            Array.Reverse(array);
            uint num = BitConverter.ToUInt32(array, 0);
            uint bytesRead = 0;
            this.Console.DebugTarget.GetMemory(0x83238958, 4, array, out bytesRead);
            Array.Reverse(array);
            num = BitConverter.ToUInt32(array, 0);
            byte[] data = new byte[1];
            this.Console.DebugTarget.SetMemory(num + 12, 1, data, out bytesRead);
            data = new byte[4];
            data[0] = 0x60;
            this.Console.DebugTarget.SetMemory(0x8225a714, 4, data, out bytesRead);
            this.Console.DebugTarget.GetMemory(0x82a44c1c, 4, array, out bytesRead);
            Array.Reverse(array);
            num = BitConverter.ToUInt32(array, 0);
            this.Console.DebugTarget.SetMemory(num + 12, 1, new byte[] { 1 }, out bytesRead);
            this.Console.DebugTarget.GetMemory(0x82a4ae20, 4, array, out bytesRead);
            Array.Reverse(array);
            num = BitConverter.ToUInt32(array, 0);
            this.Console.DebugTarget.SetMemory(num + 12, 4, new byte[] { 60, 0xcc, 0xcc, 0xcd }, out bytesRead);
            this.Console.DebugTarget.GetMemory(0x82a4addc, 4, array, out bytesRead);
            Array.Reverse(array);
            num = BitConverter.ToUInt32(array, 0);
            data = new byte[4];
            data[0] = 0x48;
            this.Console.DebugTarget.SetMemory(num + 12, 4, data, out bytesRead);
            this.Console.DebugTarget.GetMemory(0x82a4addc, 4, array, out bytesRead);
            Array.Reverse(array);
            num = BitConverter.ToUInt32(array, 0);
            data = new byte[4];
            data[0] = 0x48;
            this.Console.DebugTarget.SetMemory(num + 12, 4, data, out bytesRead);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BinaryWriter writer = new BinaryWriter(new FileStream(Application.StartupPath + @"\Customers\" + this.stat.Gamertag + ".bin", FileMode.Create));
            writer.Write((long) 0L);
            if (this.checkBox1.Checked)
            {
                writer.Write((short) this.UnlockAllStatDelta().Length);
                writer.Write(this.UnlockAllStatDelta());
                writer.BaseStream.Position = 0x40aL;
            }
            writer.Write((short) this.statDelta1(this.stat).Length);
            writer.Write(this.statDelta1(this.stat));
            writer.Close();
            base.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (this.acumulatedLevel() != 10)
            {
                this.FixLevels();
                this.stat.Kills = new Random().Next((0x1770 * this.curPrestige()) + (60 * this.acumulatedLevel()), (0x1f40 * this.curPrestige()) + (140 * this.acumulatedLevel()));
                this.stat.Deaths = new Random().Next((0x7d0 * this.curPrestige()) + (20 * this.acumulatedLevel()), (0x1388 * this.curPrestige()) + (60 * this.acumulatedLevel()));
                this.stat.Assists = new Random().Next((100 * this.curPrestige()) + this.acumulatedLevel(), (200 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                this.stat.KillStreak = new Random().Next(2, 30);
                this.stat.Suicides = new Random().Next(1, 5);
                this.stat.Hits = new Random().Next((0x1770 * this.curPrestige()) + (60 * this.acumulatedLevel()), (0x1f40 * this.curPrestige()) + (140 * this.acumulatedLevel()));
                this.stat.Misses = new Random().Next((0xea60 * this.curPrestige()) + (600 * this.acumulatedLevel()), (0x13880 * this.curPrestige()) + (0x578 * this.acumulatedLevel()));
                this.stat.Headshots = new Random().Next((200 * this.curPrestige()) + (5 * this.acumulatedLevel()), (400 * this.curPrestige()) + (10 * this.acumulatedLevel()));
                this.stat.TotalShots = this.stat.Hits + this.stat.Misses;
                this.stat.Wins = new Random().Next((100 * this.curPrestige()) + (3 * this.acumulatedLevel()), (140 * this.curPrestige()) + (10 * this.acumulatedLevel()));
                this.stat.Losses = new Random(3).Next((40 * this.curPrestige()) + (3 * this.acumulatedLevel()), (60 * this.curPrestige()) + (10 * this.acumulatedLevel()));
                this.stat.Ties = new Random().Next(0, 10);
                this.stat.WinStreak = new Random().Next(3, 30);
                this.stat.GamesPlayed = this.stat.Wins + this.stat.Losses;
                this.stat.HoursPlayed = new Random().Next(12 * this.curPrestige(), 0x12 * this.curPrestige());
                this.stat.MinutesPlayed = new Random().Next(1, 60);
                this.stat.Score = new Random().Next((0xc350 * this.curPrestige()) + (700 * this.acumulatedLevel()), (0x11170 * this.curPrestige()) + (0x4b0 * this.acumulatedLevel()));
                this.stat.Captures = new Random(1).Next((20 * this.curPrestige()) + this.acumulatedLevel(), (30 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                this.stat.Defends = new Random(2).Next((10 * this.curPrestige()) + this.acumulatedLevel(), (20 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                this.stat.Destructions = new Random(3).Next((20 * this.curPrestige()) + this.acumulatedLevel(), (30 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                this.stat.Defuses = new Random(4).Next((10 * this.curPrestige()) + this.acumulatedLevel(), (20 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                this.stat.Confirmed = new Random(5).Next((20 * this.curPrestige()) + this.acumulatedLevel(), (30 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                this.stat.Denied = new Random(6).Next((10 * this.curPrestige()) + this.acumulatedLevel(), (20 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                this.stat.Plants = new Random(7).Next((20 * this.curPrestige()) + this.acumulatedLevel(), (30 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                this.stat.Returns = new Random(8).Next((10 * this.curPrestige()) + this.acumulatedLevel(), (20 * this.curPrestige()) + (4 * this.acumulatedLevel()));
                base.Controls.Remove(this.propertyGrid1);
                PropertyGrid grid2 = new PropertyGrid {
                    Location = this.propertyGrid1.Location,
                    Size = this.propertyGrid1.Size,
                    Dock = this.propertyGrid1.Dock
                };
                this.propertyGrid1 = grid2;
                base.Controls.Add(this.propertyGrid1);
                this.propertyGrid1.SelectedObject = this.stat;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
        }

        private byte[] CreateStatDelta(int StatOffset, byte[] StatValue)
        {
            List<byte> list = new List<byte>();
            StatOffset += 12;
            StatOffset -= 4;
            byte[] bytes = BitConverter.GetBytes((short) StatOffset);
            Array.Reverse(bytes);
            list.AddRange(bytes);
            list.Add((byte) StatValue.Length);
            list.AddRange(StatValue);
            return list.ToArray();
        }

        private byte[] CreateStatDelta(int StatOffset, int StatValue)
        {
            byte[] bytes = BitConverter.GetBytes(StatValue);
            return this.CreateStatDelta(StatOffset, bytes);
        }

        private int curPrestige()
        {
            return (int) this.numericUpDown1.Value;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private byte[] FinalizeStatDelta(byte[] Stats)
        {
            List<byte> list = new List<byte> { 0x47 };
            list.AddRange(new byte[2]);
            list.AddRange(Stats);
            return list.ToArray();
        }

        private void FixLevels()
        {
            switch (this.curPrestige())
            {
                case 1:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 1M;
                    this.numericUpDown4.Value = 1M;
                    this.numericUpDown5.Value = 1M;
                    this.numericUpDown6.Value = 1M;
                    this.numericUpDown7.Value = 1M;
                    this.numericUpDown8.Value = 1M;
                    this.numericUpDown9.Value = 1M;
                    this.numericUpDown10.Value = 1M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 2:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 1M;
                    this.numericUpDown5.Value = 1M;
                    this.numericUpDown6.Value = 1M;
                    this.numericUpDown7.Value = 1M;
                    this.numericUpDown8.Value = 1M;
                    this.numericUpDown9.Value = 1M;
                    this.numericUpDown10.Value = 1M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 3:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 60M;
                    this.numericUpDown5.Value = 1M;
                    this.numericUpDown6.Value = 1M;
                    this.numericUpDown7.Value = 1M;
                    this.numericUpDown8.Value = 1M;
                    this.numericUpDown9.Value = 1M;
                    this.numericUpDown10.Value = 1M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 4:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 60M;
                    this.numericUpDown5.Value = 60M;
                    this.numericUpDown6.Value = 1M;
                    this.numericUpDown7.Value = 1M;
                    this.numericUpDown8.Value = 1M;
                    this.numericUpDown9.Value = 1M;
                    this.numericUpDown10.Value = 1M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 5:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 60M;
                    this.numericUpDown5.Value = 60M;
                    this.numericUpDown6.Value = 60M;
                    this.numericUpDown7.Value = 1M;
                    this.numericUpDown8.Value = 1M;
                    this.numericUpDown9.Value = 1M;
                    this.numericUpDown10.Value = 1M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 6:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 60M;
                    this.numericUpDown5.Value = 60M;
                    this.numericUpDown6.Value = 60M;
                    this.numericUpDown7.Value = 60M;
                    this.numericUpDown8.Value = 1M;
                    this.numericUpDown9.Value = 1M;
                    this.numericUpDown10.Value = 1M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 7:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 60M;
                    this.numericUpDown5.Value = 60M;
                    this.numericUpDown6.Value = 60M;
                    this.numericUpDown7.Value = 60M;
                    this.numericUpDown8.Value = 60M;
                    this.numericUpDown9.Value = 1M;
                    this.numericUpDown10.Value = 1M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 8:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 60M;
                    this.numericUpDown5.Value = 60M;
                    this.numericUpDown6.Value = 60M;
                    this.numericUpDown7.Value = 60M;
                    this.numericUpDown8.Value = 60M;
                    this.numericUpDown9.Value = 60M;
                    this.numericUpDown10.Value = 1M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 9:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 60M;
                    this.numericUpDown5.Value = 60M;
                    this.numericUpDown6.Value = 60M;
                    this.numericUpDown7.Value = 60M;
                    this.numericUpDown8.Value = 60M;
                    this.numericUpDown9.Value = 60M;
                    this.numericUpDown10.Value = 60M;
                    this.numericUpDown11.Value = 1M;
                    break;

                case 10:
                    this.numericUpDown2.Value = 60M;
                    this.numericUpDown3.Value = 60M;
                    this.numericUpDown4.Value = 60M;
                    this.numericUpDown5.Value = 60M;
                    this.numericUpDown6.Value = 60M;
                    this.numericUpDown7.Value = 60M;
                    this.numericUpDown8.Value = 60M;
                    this.numericUpDown9.Value = 60M;
                    this.numericUpDown10.Value = 60M;
                    this.numericUpDown11.Value = 60M;
                    break;
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.stat = new GhostStats();
            this.propertyGrid1.SelectedObject = this.stat;
        }

        private byte[] GetPrestigeBytes(int prestige)
        {
            MemoryStream output = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(output);
            for (int i = 0; i < prestige; i++)
            {
                writer.Write((int) (i + 1));
            }
            writer.Close();
            return output.ToArray();
        }

        private byte[] GetStatDeltaCommandBuffer(byte[] FinalStatDelta)
        {
            List<byte> list = new List<byte>();
            byte[] bytes = BitConverter.GetBytes(FinalStatDelta.Length);
            Array.Reverse(bytes);
            list.AddRange(new byte[8]);
            list.AddRange(new byte[] { 0x83, 0x8f, 0x44, 0x20 });
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
            this.propertyGrid1 = new PropertyGrid();
            this.button1 = new Button();
            this.button2 = new Button();
            this.numericUpDown1 = new NumericUpDown();
            this.numericUpDown2 = new NumericUpDown();
            this.label1 = new Label();
            this.label2 = new Label();
            this.checkBox1 = new CheckBox();
            this.label3 = new Label();
            this.numericUpDown3 = new NumericUpDown();
            this.label4 = new Label();
            this.numericUpDown4 = new NumericUpDown();
            this.label5 = new Label();
            this.numericUpDown5 = new NumericUpDown();
            this.label6 = new Label();
            this.numericUpDown6 = new NumericUpDown();
            this.label7 = new Label();
            this.numericUpDown7 = new NumericUpDown();
            this.label8 = new Label();
            this.numericUpDown8 = new NumericUpDown();
            this.label9 = new Label();
            this.numericUpDown9 = new NumericUpDown();
            this.label10 = new Label();
            this.numericUpDown10 = new NumericUpDown();
            this.label11 = new Label();
            this.numericUpDown11 = new NumericUpDown();
            this.numericUpDown1.BeginInit();
            this.numericUpDown2.BeginInit();
            this.numericUpDown3.BeginInit();
            this.numericUpDown4.BeginInit();
            this.numericUpDown5.BeginInit();
            this.numericUpDown6.BeginInit();
            this.numericUpDown7.BeginInit();
            this.numericUpDown8.BeginInit();
            this.numericUpDown9.BeginInit();
            this.numericUpDown10.BeginInit();
            this.numericUpDown11.BeginInit();
            base.SuspendLayout();
            this.propertyGrid1.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.propertyGrid1.Location = new Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new Size(230, 0x19d);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.Click += new EventHandler(this.propertyGrid1_Click);
            this.button1.Location = new Point(240, 0x180);
            this.button1.Name = "button1";
            this.button1.Size = new Size(0x7f, 0x17);
            this.button1.TabIndex = 1;
            this.button1.Text = "Save Customer";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click_1);
            this.button2.Location = new Point(240, 0x163);
            this.button2.Name = "button2";
            this.button2.Size = new Size(0x7f, 0x17);
            this.button2.TabIndex = 2;
            this.button2.Text = "Legit Stats";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click_1);
            this.numericUpDown1.Location = new Point(330, 0x1a);
            int[] bits = new int[4];
            bits[0] = 10;
            this.numericUpDown1.Maximum = new decimal(bits);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(0x22, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown2.Location = new Point(330, 0x34);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown2.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown2.Minimum = new decimal(bits);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new Size(0x22, 20);
            this.numericUpDown2.TabIndex = 4;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown2.Value = new decimal(bits);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0xea, 0x1c);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x33, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Prestige: ";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(0xea, 0x36);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x59, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Member 1 Level: ";
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new Point(0xec, 0x143);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new Size(80, 0x11);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Unlock All?";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new EventHandler(this.checkBox1_CheckedChanged);
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xea, 80);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x59, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Member 2 Level: ";
            this.numericUpDown3.Location = new Point(330, 0x4e);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown3.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown3.Minimum = new decimal(bits);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new Size(0x22, 20);
            this.numericUpDown3.TabIndex = 8;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown3.Value = new decimal(bits);
            this.label4.AutoSize = true;
            this.label4.Location = new Point(0xea, 0x6a);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x53, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Member 3 Level";
            this.numericUpDown4.Location = new Point(330, 0x68);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown4.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown4.Minimum = new decimal(bits);
            this.numericUpDown4.Name = "numericUpDown4";
            this.numericUpDown4.Size = new Size(0x22, 20);
            this.numericUpDown4.TabIndex = 10;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown4.Value = new decimal(bits);
            this.label5.AutoSize = true;
            this.label5.Location = new Point(0xea, 0x84);
            this.label5.Name = "label5";
            this.label5.Size = new Size(0x59, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Member 4 Level: ";
            this.numericUpDown5.Location = new Point(330, 130);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown5.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown5.Minimum = new decimal(bits);
            this.numericUpDown5.Name = "numericUpDown5";
            this.numericUpDown5.Size = new Size(0x22, 20);
            this.numericUpDown5.TabIndex = 12;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown5.Value = new decimal(bits);
            this.label6.AutoSize = true;
            this.label6.Location = new Point(0xea, 0x9e);
            this.label6.Name = "label6";
            this.label6.Size = new Size(0x59, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Member 5 Level: ";
            this.numericUpDown6.Location = new Point(330, 0x9c);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown6.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown6.Minimum = new decimal(bits);
            this.numericUpDown6.Name = "numericUpDown6";
            this.numericUpDown6.Size = new Size(0x22, 20);
            this.numericUpDown6.TabIndex = 14;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown6.Value = new decimal(bits);
            this.label7.AutoSize = true;
            this.label7.Location = new Point(0xea, 0xb8);
            this.label7.Name = "label7";
            this.label7.Size = new Size(0x59, 13);
            this.label7.TabIndex = 0x11;
            this.label7.Text = "Member 6 Level: ";
            this.numericUpDown7.Location = new Point(330, 0xb6);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown7.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown7.Minimum = new decimal(bits);
            this.numericUpDown7.Name = "numericUpDown7";
            this.numericUpDown7.Size = new Size(0x22, 20);
            this.numericUpDown7.TabIndex = 0x10;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown7.Value = new decimal(bits);
            this.label8.AutoSize = true;
            this.label8.Location = new Point(0xea, 210);
            this.label8.Name = "label8";
            this.label8.Size = new Size(0x59, 13);
            this.label8.TabIndex = 0x13;
            this.label8.Text = "Member 7 Level: ";
            this.numericUpDown8.Location = new Point(330, 0xd0);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown8.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown8.Minimum = new decimal(bits);
            this.numericUpDown8.Name = "numericUpDown8";
            this.numericUpDown8.Size = new Size(0x22, 20);
            this.numericUpDown8.TabIndex = 0x12;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown8.Value = new decimal(bits);
            this.label9.AutoSize = true;
            this.label9.Location = new Point(0xea, 0xec);
            this.label9.Name = "label9";
            this.label9.Size = new Size(0x59, 13);
            this.label9.TabIndex = 0x15;
            this.label9.Text = "Member 8 Level: ";
            this.numericUpDown9.Location = new Point(330, 0xea);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown9.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown9.Minimum = new decimal(bits);
            this.numericUpDown9.Name = "numericUpDown9";
            this.numericUpDown9.Size = new Size(0x22, 20);
            this.numericUpDown9.TabIndex = 20;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown9.Value = new decimal(bits);
            this.label10.AutoSize = true;
            this.label10.Location = new Point(0xea, 0x106);
            this.label10.Name = "label10";
            this.label10.Size = new Size(0x59, 13);
            this.label10.TabIndex = 0x17;
            this.label10.Text = "Member 9 Level: ";
            this.numericUpDown10.Location = new Point(330, 260);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown10.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown10.Minimum = new decimal(bits);
            this.numericUpDown10.Name = "numericUpDown10";
            this.numericUpDown10.Size = new Size(0x22, 20);
            this.numericUpDown10.TabIndex = 0x16;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown10.Value = new decimal(bits);
            this.label11.AutoSize = true;
            this.label11.Location = new Point(0xea, 0x120);
            this.label11.Name = "label11";
            this.label11.Size = new Size(0x5f, 13);
            this.label11.TabIndex = 0x19;
            this.label11.Text = "Member 10 Level: ";
            this.numericUpDown11.Location = new Point(330, 0x11e);
            bits = new int[4];
            bits[0] = 60;
            this.numericUpDown11.Maximum = new decimal(bits);
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown11.Minimum = new decimal(bits);
            this.numericUpDown11.Name = "numericUpDown11";
            this.numericUpDown11.Size = new Size(0x22, 20);
            this.numericUpDown11.TabIndex = 0x18;
            bits = new int[4];
            bits[0] = 1;
            this.numericUpDown11.Value = new decimal(bits);
            base.ClientSize = new Size(0x177, 0x19d);
            base.Controls.Add(this.label11);
            base.Controls.Add(this.numericUpDown11);
            base.Controls.Add(this.label10);
            base.Controls.Add(this.numericUpDown10);
            base.Controls.Add(this.label9);
            base.Controls.Add(this.numericUpDown9);
            base.Controls.Add(this.label8);
            base.Controls.Add(this.numericUpDown8);
            base.Controls.Add(this.label7);
            base.Controls.Add(this.numericUpDown7);
            base.Controls.Add(this.label6);
            base.Controls.Add(this.numericUpDown6);
            base.Controls.Add(this.label5);
            base.Controls.Add(this.numericUpDown5);
            base.Controls.Add(this.label4);
            base.Controls.Add(this.numericUpDown4);
            base.Controls.Add(this.label3);
            base.Controls.Add(this.numericUpDown3);
            base.Controls.Add(this.checkBox1);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.numericUpDown2);
            base.Controls.Add(this.numericUpDown1);
            base.Controls.Add(this.button2);
            base.Controls.Add(this.button1);
            base.Controls.Add(this.propertyGrid1);
            base.Name = "AddCustomerForm";
            this.Text = "Add Customer";
            base.Shown += new EventHandler(this.Form1_Shown);
            this.numericUpDown1.EndInit();
            this.numericUpDown2.EndInit();
            this.numericUpDown3.EndInit();
            this.numericUpDown4.EndInit();
            this.numericUpDown5.EndInit();
            this.numericUpDown6.EndInit();
            this.numericUpDown7.EndInit();
            this.numericUpDown8.EndInit();
            this.numericUpDown9.EndInit();
            this.numericUpDown10.EndInit();
            this.numericUpDown11.EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private byte[] nullBytes(int count)
        {
            byte[] buffer = new byte[count];
            for (int i = 0; i < count; i++)
            {
                buffer[i] = 0xff;
            }
            return buffer;
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {
        }

        private void SetStat(byte[] stat)
        {
            uint bytesWritten = 0;
            this.Console.DebugTarget.SetMemory(0x838f4420, (uint) stat.Length, stat, out bytesWritten);
        }

        private byte[] statDelta1(GhostStats stats)
        {
            List<byte> statDeltas = new List<byte>();
            this.AddStat(statDeltas, this.CreateStatDelta(this.gamesplayed, stats.GamesPlayed));
            this.AddStat(statDeltas, this.CreateStatDelta(this.wins, stats.Wins));
            this.AddStat(statDeltas, this.CreateStatDelta(this.losses, stats.Losses));
            this.AddStat(statDeltas, this.CreateStatDelta(this.ties, stats.Ties));
            this.AddStat(statDeltas, this.CreateStatDelta(this.winstreak, stats.WinStreak));
            this.AddStat(statDeltas, this.CreateStatDelta(this.kills, stats.Kills));
            this.AddStat(statDeltas, this.CreateStatDelta(this.deaths, stats.Deaths));
            this.AddStat(statDeltas, this.CreateStatDelta(this.assists, stats.Assists));
            this.AddStat(statDeltas, this.CreateStatDelta(this.suicides, stats.Suicides));
            this.AddStat(statDeltas, this.CreateStatDelta(this.killstreak, stats.KillStreak));
            this.AddStat(statDeltas, this.CreateStatDelta(this.hits, stats.Hits));
            this.AddStat(statDeltas, this.CreateStatDelta(this.misses, stats.Misses));
            this.AddStat(statDeltas, this.CreateStatDelta(this.headshots, stats.Headshots));
            this.AddStat(statDeltas, this.CreateStatDelta(this.totalshots, stats.TotalShots));
            this.AddStat(statDeltas, this.CreateStatDelta(this.captures, stats.Captures));
            this.AddStat(statDeltas, this.CreateStatDelta(this.defends, stats.Defends));
            this.AddStat(statDeltas, this.CreateStatDelta(this.destructions, stats.Destructions));
            this.AddStat(statDeltas, this.CreateStatDelta(this.defuses, stats.Defuses));
            this.AddStat(statDeltas, this.CreateStatDelta(this.confirmed, stats.Confirmed));
            this.AddStat(statDeltas, this.CreateStatDelta(this.denied, stats.Denied));
            this.AddStat(statDeltas, this.CreateStatDelta(this.plants, stats.Plants));
            this.AddStat(statDeltas, this.CreateStatDelta(this.returns, stats.Returns));
            this.AddStat(statDeltas, this.CreateStatDelta(this.squadpoints, stats.SquadPoints));
            this.AddStat(statDeltas, this.CreateStatDelta(this.score, stats.Score));
            this.AddStat(statDeltas, this.CreateStatDelta(this.timeplayed, (int) ((stats.MinutesPlayed * 60) + (stats.HoursPlayed * 0xe10))));
            if (stats.Kills != 0)
            {
                this.AddStat(statDeltas, this.CreateStatDelta(this.kdratio, (int) ((stats.Kills * 0x3e8) / stats.Deaths)));
                this.AddStat(statDeltas, this.CreateStatDelta(this.winlossratio, (int) ((stats.Wins * 0x3e8) / stats.Losses)));
                this.AddStat(statDeltas, this.CreateStatDelta(this.accuracy, (int) ((stats.Hits * 0x2710) / stats.TotalShots)));
            }
            this.AddStat(statDeltas, this.CreateStatDelta(this.prestige, (int) this.numericUpDown1.Value));
            this.AddStat(statDeltas, this.CreateStatDelta(this.prestigeData, this.GetPrestigeBytes((int) this.numericUpDown1.Value)));
            this.AddStat(statDeltas, this.CreateStatDelta(this.squadmembers + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown2.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + this.squadmemberlen) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown3.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + (this.squadmemberlen * 2)) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown4.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + (this.squadmemberlen * 3)) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown5.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + (this.squadmemberlen * 4)) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown6.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + (this.squadmemberlen * 5)) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown7.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + (this.squadmemberlen * 6)) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown8.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + (this.squadmemberlen * 7)) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown9.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + (this.squadmemberlen * 8)) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown10.Value] + 500)));
            this.AddStat(statDeltas, this.CreateStatDelta((this.squadmembers + (this.squadmemberlen * 9)) + this.squadmemberxp, (int) (this.GhostLevels[(int) this.numericUpDown11.Value] + 500)));
            return this.FinalizeStatDelta(statDeltas.ToArray());
        }

        private byte[] UnlockAllStatDelta()
        {
            List<byte> statDeltas = new List<byte>();
            this.AddStat(statDeltas, this.CreateStatDelta(0x42ce, this.nullBytes(0x7f)));
            this.AddStat(statDeltas, this.CreateStatDelta(0x434d, this.nullBytes(0x7f)));
            this.AddStat(statDeltas, this.CreateStatDelta(0x43cc, this.nullBytes(0x7f)));
            this.AddStat(statDeltas, this.CreateStatDelta(0x444b, this.nullBytes(0x7f)));
            this.AddStat(statDeltas, this.CreateStatDelta(0x44ca, this.nullBytes(0x7f)));
            this.AddStat(statDeltas, this.CreateStatDelta(0x4549, this.nullBytes(0x4e)));
            return this.FinalizeStatDelta(statDeltas.ToArray());
        }

        public XDevkit.XboxConsole Console { get; set; }
    }
}

