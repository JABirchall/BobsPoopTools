using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XDevkit;
using System.Threading;
using XDevkitExt;
using System.IO;
using Microsoft.Test.Xbox.Profiles;
using System.Runtime.InteropServices;
using Microsoft.Test.Xbox.XDRPC;
using System.Net;
using Profiles;
using Tools;
using Suave.Classes;
using LuaInterface;
using System.Reflection;
using System.Collections;
using XRPCLib;

namespace test
{
    public partial class Form1 : Form
    {
        public XDevkit.IXboxManager xbm = ((XDevkit.XboxManager)Activator.CreateInstance(System.Type.GetTypeFromCLSID(new Guid("A5EB45D8-F3B6-49B9-984A-0D313AB60342"))));
        public static XDevkit.IXboxConsole xbc;
        public IniFile myini = new IniFile(Application.StartupPath + "\\info.ini");
        public static bool IsDevKit = false;
        private Suave.Classes.SuaveLib SLib = null;
        private Suave.Classes.SuaveLib.Client SuaveClient = null;
        public static bool Connected = false;
        private GhostTool.Tools MyGhost = null;

        public Form1()
        {
            InitializeComponent();
            textBox14.Text = myini.IniReadValue("MyInfo", "XboxIP");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            console.Connect(null);
            textBox10.Text = "Not Connected";
            Connected = false;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string IP = textBox14.Text;
            if (console.Connect(IP))
            {
                xbc = (XDevkit.IXboxConsole)console.Xbox_Console;
                textBox7.Text = myini.IniReadValue("MyInfo", "MyGT");
                textBox8.Text = myini.IniReadValue("MyInfo", "MyXUID");
                //SLib = new Suave.Classes.SuaveLib(xbc, richTextBox1);
                MyGhost = new GhostTool.Tools(xbc);
                textBox10.Text = "Connected";
                myini.IniWriteValue("MyInfo", "XboxIP", IP);
                Connected = true;
                MessageBox.Show("Connected To: " + IP);
            }
            else
            {
                MessageBox.Show("Not Connected To: " + IP);
            }
        }

        private void BO2_Cbuf_AddText(string text)
        {
            if (Connected)
            {
                Suave.Classes.Addresses Addresses = new Suave.Classes.Addresses();
                ((XDevkit.IXboxConsole)xbc).Call(0x82400ad8, new object[] { 0, text });
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {
                    textBox3.Text = Users.XamUserGetXUID(xbc);
                }
                else
                {
                    ulong Xuid = 0L;
                    DevUsers.GetXuidFromIndex(xbc, 0, out Xuid);
                    textBox3.Text = Xuid.ToString("X16");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {
                    textBox1.Text = Users.GetGamertag(xbc);
                    textBox2.Text = Users.GetXUID(xbc);
                }
                else
                {
                    textBox1.Text = DevUsers.GetGamertag(xbc);
                    textBox2.Text = DevUsers.GetXUID(xbc);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox4.Text += "root@bob.com: " + Utils.SendTextCommand(xbc, textBox5.Text) + Environment.NewLine;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {
                    Users.SetGamertag(xbc, textBox1.Text, textBox2.Text);
                    MessageBox.Show("Gamertag Is Set to " + textBox1.Text);
                }
                else
                {
                    ulong Xuid = 0L;
                    DevUsers.SetGamertag(xbc, textBox1.Text, textBox2.Text);
                    DevUsers.GetXuidFromIndex(xbc, 0, out Xuid);
                    DevUsers.JoinParty(xbc, 0, Xuid);
                    MessageBox.Show("Gamertag Is Set to " + textBox1.Text);
                }
            }
        }

        private void karma_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {
                    Users.SetGamertag(xbc, "Major Nelson", "00092EEEEEEEFD31");
                    MessageBox.Show("Gamertag Is Set to Major Nelson");
                }
                else
                {
                    ulong Xuid = 0L;
                    DevUsers.SetGamertag(xbc, "Major Nelson", "00092EEEEEEEFD31");
                    DevUsers.GetXuidFromIndex(xbc, 0, out Xuid);
                    DevUsers.JoinParty(xbc, 0, Xuid);
                    MessageBox.Show("Gamertag Is Set to Major Nelson");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {
                    textBox2.Text = "";
                    string XUID = Users.GetXUID(xbc, textBox1.Text);
                    Users.SetGamertag(xbc, textBox1.Text, XUID);
                    textBox2.Text = XUID;
                }
                else
                {
                    //textBox2.Text = "";
                    //string XUID = DevUsers.GetXUID(xbc, textBox1.Text);
                    //Users.SetGamertag(xbc, textBox1.Text, XUID);
                    //textBox2.Text = XUID;
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {
                    Users.SetGamertag(xbc, myini.IniReadValue("MyInfo", "MyGT"), myini.IniReadValue("MyInfo", "MyXUID"));
                    MessageBox.Show("Gamertag Is Set to " + myini.IniReadValue("MyInfo", "MyGT"));
                }
                else
                {
                    DevUsers.SetGamertag(xbc, myini.IniReadValue("MyInfo", "MyGT"), myini.IniReadValue("MyInfo", "MyXUID"));
                    MessageBox.Show("Gamertag Is Set to " + myini.IniReadValue("MyInfo", "MyGT"));
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                    textBox9.Text = Users.GetXUID(xbc, textBox6.Text);
                else
                {
                    textBox9.Text = DevUsers.GetXUID(xbc, textBox6.Text).ToString("X16"); ;
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {
                    listView1.Items.Clear();
                    Party.GetPartyMembers(xbc);
                    for (int i = 0; i < Party.PartyUsersCount; i++)
                    {
                        if (Party.ListGamerTags[i] != "")
                        {
                            ListViewItem lvi = new ListViewItem(Party.ListSXuid[i]);
                            lvi.SubItems.Add(Party.ListGamerTags[i]);
                            lvi.SubItems.Add(i.ToString());
                            listView1.Items.Add(lvi);
                        }
                    }
                }
                else
                {
                    listView1.Items.Clear();
                    DevUsers.GetPartyMembers(xbc);
                    for (int i = 0; i < DevUsers.PartyUsersCount; i++)
                    {
                        if (DevUsers.ListGamerTags[i] != "")
                        {
                            ListViewItem lvi = new ListViewItem(DevUsers.ListSXuid[i]);
                            lvi.SubItems.Add(DevUsers.ListGamerTags[i]);
                            lvi.SubItems.Add(i.ToString());
                            listView1.Items.Add(lvi);
                        }
                    }
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {
                    try
                    {
                        string Gamer = listView1.SelectedItems[0].SubItems[1].Text;
                        string XUID = listView1.SelectedItems[0].SubItems[0].Text;
                        Users.SetGamertag(xbc, Gamer, XUID);
                        MessageBox.Show("Gamertag Is Set to " + Gamer);
                    }
                    catch
                    {

                    }
                }
                else
                {
                    try
                    {
                        string Gamer = listView1.SelectedItems[0].SubItems[1].Text;
                        string XUID = listView1.SelectedItems[0].SubItems[0].Text;
                        DevUsers.SetGamertag(xbc, Gamer, XUID);
                        MessageBox.Show("Gamertag Is Set to " + Gamer);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                IsDevKit = true;
                MessageBox.Show("Set to DevKit");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                IsDevKit = false;
                MessageBox.Show("Set to Retail");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {

                }
                else
                {
                    File.Delete(@"lua\JoinParty.lua");
                    string XUID = textBox12.Text;
                    logger(@"lua\JoinParty.lua", "LauJoinParty(" + XUID + ")", false);
                    lau Join = new lau(xbc);
                    Join.runJoinParty();
                    MessageBox.Show("You Joined the Party!!!!");
                }
            }
        }

        private void logger(string name, string strLogText, bool t)
        {
            StreamWriter log;

            if (!File.Exists(name))
            {
                log = new StreamWriter(name);
            }
            else
            {
                log = File.AppendText(name);
            }

            // Write to the file:
            log.WriteLine(strLogText);
            log.WriteLine();

            // Close the stream:
            log.Close();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {

                }
                else
                {
                    File.Delete(@"log\SendFriendRequest.lua");

                    DateTime date1 = DateTime.Now;
                    listView2.Items.Clear();
                    DevUsers.GetMyFriends(xbc, 0);
                    for (int i = 0; i < DevUsers.FUsersCount; i++)
                    {
                        if (DevUsers.ListGamerTags[i] != "")
                        {
                            ListViewItem lvi = new ListViewItem(DevUsers.ListFSXuid[i]);
                            lvi.SubItems.Add(DevUsers.ListFGamerTags[i]);
                            lvi.SubItems.Add(DevUsers.ListFGameID[i]);
                            lvi.SubItems.Add(i.ToString());
                            listView2.Items.Add(lvi);

                            logger(@"log\Friends-at-" + date1.ToString().Replace("/", "-").Replace(":", "-").Replace("AM", "").Replace("PM", "") + ".log", "-- ID: " + i.ToString() + Environment.NewLine + "GamerTag: " + DevUsers.ListFGamerTags[i] + Environment.NewLine + "Hex-Xuid: " + DevUsers.ListFSXuid[i].ToString() + Environment.NewLine + "Xuid: " + DevUsers.ListFLXuid[i].ToString() + Environment.NewLine, false);
                            logger(@"log\SendFriendRequest.lua", "-- ID: " + i.ToString() + " GamerTag: " + DevUsers.ListFGamerTags[i] + " Hex-Xuid: " + DevUsers.ListFSXuid[i].ToString() + " Xuid: " + DevUsers.ListFLXuid[i].ToString() + Environment.NewLine + "LauSendFriendRequest(" + DevUsers.ListFLXuid[i].ToString() + ")", false);
                        }
                    }
                }
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {

                }
                else
                {
                    string id = listView2.SelectedItems[0].SubItems[3].Text;
                    ulong XUID = DevUsers.ListFLXuid[Convert.ToInt32(id)];
                    string test = DevUsers.ListFSXuid[Convert.ToInt32(id)];

                    DevUsers.JoinParty(xbc, 0, XUID);
                    MessageBox.Show("You Joined the Party!!!!");
                }
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {

                }
                else
                {
                    ulong Xuid = 0L;
                    DevUsers.GetXuidFromIndex(xbc, 0, out Xuid);
                    string id = listView1.SelectedItems[0].SubItems[2].Text;
                    ulong XUID = DevUsers.ListLXuid[Convert.ToInt32(id)];
                    DevUsers.SendFriendRequest(xbc, Xuid, XUID);
                    MessageBox.Show("FriendRequest Go!!!!");
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {

                }
                else
                {
                    string Gamer = listView2.SelectedItems[0].SubItems[1].Text;
                    string XUID = listView2.SelectedItems[0].SubItems[0].Text;
                    Users.SetGamertag(xbc, Gamer, XUID);
                    MessageBox.Show("Gamertag Is Set to " + Gamer);
                }
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                //clientBox.Items.Clear();
                SLib.numClients = 0;
                for (uint i = 0; i <= 0x11; i++)
                {
                    SuaveClient = new Suave.Classes.SuaveLib.Client(xbc, i, SLib);
                    string clientName = SuaveClient.clientName;
                    if (!string.IsNullOrEmpty(clientName))
                    {
                        //clientBox.Items.Add(clientName);
                        SLib.playersGamertags[i] = clientName;
                        SLib.numClients = i;
                    }
                    else
                    {
                        //clientBox.Items.Add("");
                    }
                }
            }
        }

        private void button30_Click_1(object sender, EventArgs e)
        {
            if (Connected)
            {
                lau test = new lau(xbc);
                test.runSendFriendRequest();
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                lau Join = new lau(xbc);
                Join.runJoinParty();
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                lau Join = new lau(xbc);
                Join.run();
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                if (IsDevKit == false)
                {

                }
                else
                {
                    ulong XUID = DevUsers.GetXUID(xbc, textBox12.Text);
                    DevUsers.JoinParty(xbc, 0, XUID);
                    MessageBox.Show("You Joined the Party!!!!");
                }
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                myini.IniWriteValue("MyInfo", "MyGT", textBox7.Text);
                myini.IniWriteValue("MyInfo", "MyXUID", textBox8.Text);
            }
        }

        public System.Windows.Forms.Timer refreshTimer = new System.Windows.Forms.Timer();
        private void Form1_Load(object sender, EventArgs e)
        {
            refreshTimer.Interval = 0xbb8;
            refreshTimer.Tick += new EventHandler(refreshTimer_Tick);
        }

        //Ghost
        public void refreshTimer_Tick(object sender, EventArgs e)
        {
            this.ProcessCustomerOrders();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            new GhostTool.AddCustomerForm().ShowDialog();
        }

        public void ProcessCustomerOrders()
        {
            BinaryReader reader;
            listView3.Items.Clear();
            MyGhost.LoadPlayers();
            List<string> list = new List<string>();
            foreach (string str in Directory.GetFiles(Application.StartupPath + @"\Customers", "*.bin"))
            {
                string str2 = str.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries)[str.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Length - 1].Replace(".bin", "");
                reader = new BinaryReader(new FileStream(str, FileMode.Open));
                long dateData = reader.ReadInt64();
                reader.Close();
                ListViewItem item2 = new ListViewItem
                {
                    Text = str2
                };
                item2.SubItems.Add((dateData == 0L) ? "Not yet processed" : DateTime.FromBinary(dateData).ToString("g"));
                if (dateData == 0L)
                {
                    list.Add(str2.ToLower());
                }
                this.listView3.Items.Add(item2);
            }
            for (int i = 0; i < 0x12; i++)
            {
                MyGhost.run(i, list);
            }
        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            refreshTimer.Start();
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            refreshTimer.Stop();
        }

        //BO2
        private void button23_Click_1(object sender, EventArgs e)
        {
            if (Connected)
            {
                BO2_Cbuf_AddText("set party_connectToOthers 0");
                BO2_Cbuf_AddText("set partyMigrate_disabled 1");
                BO2_Cbuf_AddText("set party_mergingEnabled 0");
                SLib.print(false, "Force Host: On", new object[0]);
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                SLib.ToggleForceUAV();
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                SLib.ToggleGod();
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                SLib.ToggleInvisibility();
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                SLib.ToggleACUnlimitedAmmo();
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (Connected)
            {
                BO2_Cbuf_AddText("set ui_isDLCPopupEnabled 0");
                SLib.print(false, "DLCPopupEnabled: Off", new object[0]);
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            byte[] buffer2 = new byte[4];
            buffer2[0] = 0x60;
            byte[] data = buffer2;
            ((XDevkit.IXboxConsole)xbc).setMemory(0x8299bf00, data);
            ((XDevkit.IXboxConsole)xbc).setMemory(0x8293c78c, data);
        }

        //private void button21_Click(object sender, EventArgs e)
        //{
        //    if (Connected)
        //    {
        //        Suave.Classes.Addresses BO2addresses = new Suave.Classes.Addresses();
        //        if (((XDevkit.IXboxConsole)xbc).ReadUInt32(BO2addresses.CheatProtection) != 0x60000000)
        //        {
        //            byte[] buffer2 = new byte[4];
        //            buffer2[0] = 0x60;
        //            byte[] data = buffer2;
        //            ((XDevkit.IXboxConsole)xbc).setMemory(BO2addresses.CheatProtection, data);
        //            SLib.print(false, "Cheat protection: Off", new object[0]);
        //        }
        //        else
        //        {
        //            ((XDevkit.IXboxConsole)xbc).setMemory(BO2addresses.CheatProtection, new byte[] { 0x41, 0x9a, 0, 12 });
        //            SLib.print(false, "Cheat protection: On", new object[0]);
        //        }
        //    }
        //}

        //private void button26_Click(object sender, EventArgs e)
        //{
        //    if (Connected)
        //    {
        //        if (SuaveClient.RadarFlag != 3)
        //        {
        //            //SuaveClient.RadarFlag = 3;
        //            ((XDevkit.IXboxConsole)xbc).setMemory(SLib.getPlayerState(SLib.playerSelected) + 0x5604, new byte[] { 3 });
        //            SLib.print(false, "{0}: UAV On", new object[] { SLib.playerGamertag });
        //            SLib.iprintln(SLib.playerSelected, "^3Automatic UAV: ^2On");
        //        }
        //        else
        //        {
        //            //SuaveClient.RadarFlag = 1;
        //            ((XDevkit.IXboxConsole)xbc).setMemory(SLib.getPlayerState(SLib.playerSelected) + 0x5604, new byte[] { 1 });
        //            SLib.print(false, "{0}: UAV Off", new object[] { SLib.playerGamertag });
        //            SLib.iprintln(SLib.playerSelected, "^3Automatic UAV: ^1Off");
        //        }
        //    }
        //}

        //private void button30_Click(object sender, EventArgs e)
        //{
        //    if (Connected)
        //    {
        //        SuaveClient.GiveKillstreaks();
        //        SLib.print(false, "{0}: Gave killstreaks.", new object[] { SLib.playerGamertag });
        //    }
        //}
    }
}
