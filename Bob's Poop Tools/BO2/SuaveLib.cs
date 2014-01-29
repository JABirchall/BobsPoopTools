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

namespace Suave.Classes
{
    internal class SuaveLib
    {
        private Suave.Classes.Addresses Addresses = new Suave.Classes.Addresses();
        private static byte[] carrierUFO = new byte[] { 0xc5, 0x98, 0x35, 0x62, 0xc2, 0x9a, 0xf8, 0x59, 0x42, 0x22, 0xc1, 0x26 };
        private RichTextBox logBox;
        public uint numClients = 0;
        public uint player1 = 50;
        public uint player2 = 50;
        public string playerGamertag = null;
        public uint playerMe = 50;
        public uint playerSelected = 0;
        public string[] playersGamertags = new string[0x12];
        private static byte[] turbineUFO = new byte[] { 0x44, 0x16, 0x71, 0xc6, 0x45, 0x81, 0xf8, 0xa3, 0xc3, 0x7a, 0x27, 0xc2 };
        private Action<int, byte[]> ufoAct;
        public XDevkit.IXboxConsole xbc;

        public XDevkit.IXboxManager xbm = ((XDevkit.XboxManager) Activator.CreateInstance(System.Type.GetTypeFromCLSID(new Guid("A5EB45D8-F3B6-49B9-984A-0D313AB60342"))));

        public SuaveLib(XDevkit.IXboxConsole lxbc, RichTextBox richLogBox)
        {
            this.xbc = lxbc;
            this.ufoAct = delegate (int client, byte[] curOrg) {
                Thread.Sleep(200);
                SV_GameSendServerCommand((uint) client, "< \"pmovesingle Hook Called!\"");
                byte[] data = new byte[4];
                data[2] = 8;
                ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState((uint) client) + 0x54e8, data);
                data = new byte[4];
                data[3] = 2;
                ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState((uint) client) + 0x5410, data);
                ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState((uint) client) + 40, turbineUFO);
                ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState((uint) client) + 40, curOrg);
            };
            this.logBox = richLogBox;
        }

        public uint BG_GetViewmodelWeaponIndex(uint clientIndex)
        {
            return ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.BG_GetViewmodelWeaponIndex, new object[] { getPlayerState(clientIndex) });
        }

        public void BG_TakePlayerWeapon(uint clientIndex, uint weaponIndex)
        {
            ((XDevkit.IXboxConsole) xbc).Call(getPlayerState(clientIndex), new object[] { weaponIndex });
        }

        public void Cbuf_AddText(string text)
        {
            ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.Cbuf_AddText, new object[] { 0, text });
        }

        public bool Connect(string xboxName)
        {
            if (string.IsNullOrEmpty(xboxName))
            {
                //this.Suave_Error("Please choose an Xbox 360 from the list before connecting.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            if (console.Connect(xboxName))
            {
                xbc = (XDevkit.IXboxConsole) console.Xbox_Console;
                this.print(false, "Connected to {0}!", new object[] { xboxName });
                return true;
            }
            this.print(false, "Failed to Connect to {0}.", new object[] { xboxName });
            //this.Suave_Error("Suave cannot connect to " + xboxName + ". Please check the console's connection before trying to connect again.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
        }

        public bool Dvar_GetBool(uint dvarAddress)
        {
            XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.Dvar_GetBool);
            XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>(dvarAddress);
            return ((XDevkit.IXboxConsole) xbc).ExecuteRPC<bool>(options, new XDRPCArgumentInfo[] { info });
        }

        public void G_CallSpawnEntity(uint entity)
        {
            ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.G_CallSpawnEntity, new object[] { entity });
        }

        public void G_EntAttach(uint clientIndex, string modelName, string tagName, bool ignoreCollision)
        {
            ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.G_EntAttach, new object[] { this.getEntity(clientIndex), modelName, this.G_TagIndex(tagName), ignoreCollision });
        }

        public uint G_GetWeaponIndexForName(string weaponName)
        {
            return ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.G_GetWeaponIndexForName, new object[] { weaponName });
        }

        public void G_GivePlayerWeapon(uint clientIndex, uint weaponIndex, uint modelVariant, bool akimbo)
        {
            ((XDevkit.IXboxConsole) xbc).Call(getPlayerState(clientIndex), new object[] { weaponIndex, modelVariant, akimbo });
        }

        public void G_InitializeAmmo(uint clientIndex, uint weaponIndex, uint modelVariant)
        {
            ((XDevkit.IXboxConsole) xbc).Call(this.getEntity(clientIndex), new object[] { weaponIndex, modelVariant });
        }

        public short G_LocalizedStringIndex(string text)
        {
            XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.G_LocalizedStringIndex);
            XDRPCStringArgumentInfo info = new XDRPCStringArgumentInfo(text);
            return ((XDevkit.IXboxConsole) xbc).ExecuteRPC<short>(options, new XDRPCArgumentInfo[] { info });
        }

        public byte G_MaterialIndex(string material)
        {
            XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.G_MaterialIndex);
            XDRPCStringArgumentInfo info = new XDRPCStringArgumentInfo(material);
            return ((XDevkit.IXboxConsole) xbc).ExecuteRPC<byte>(options, new XDRPCArgumentInfo[] { info });
        }

        public short G_ModelIndex(uint modelAddress)
        {
            return ((XDevkit.IXboxConsole) xbc).ExecuteRPC<short>(new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.G_ModelIndex), new XDRPCArgumentInfo[] { new XDRPCArgumentInfo<uint>(modelAddress) });
        }

        public void G_SetModel(uint clientIndex, uint modelAddress)
        {
            ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.G_SetModel, new object[] { this.getEntity(clientIndex), modelAddress });
        }

        public uint G_Spawn()
        {
            return ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.G_Spawn, new object[0]);
        }

        public uint G_TagIndex(string tagName)
        {
            return ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.G_TagIndex, new object[] { tagName });
        }

        public string getClientClantag(uint clientIndex)
        {
            return ((XDevkit.IXboxConsole) xbc).ReadNullTermString((getPlayerState(clientIndex) + 0x55a0));
        }

        public string getClientName(uint clientIndex)
        {
            return ((XDevkit.IXboxConsole) xbc).ReadNullTermString((getPlayerState(clientIndex) + 0x5534));
        }

        public string getCurrentWeapon(uint clientIndex)
        {
            if (((((XDevkit.IXboxConsole) xbc).ReadUInt32((getPlayerState(clientIndex) + 0x5410)) != 0) || (((XDevkit.IXboxConsole) xbc).ReadUInt32((getPlayerState(clientIndex) + 0x5410)) == 0)) || (((XDevkit.IXboxConsole) xbc).ReadUInt32((getPlayerState(clientIndex) + 0x5410)) == 0xff))
            {
                return "none";
            }
            return "FINISH THIS";
        }

        public uint getEntity(uint clientIndex)
        {
            return (Suave.Classes.Addresses.G_Entites + (clientIndex * 0x31c));
        }

        public float[] getOrigin(uint clientIndex)
        {
            return new float[] { BitConverter.ToSingle(Utils.StringToByteArray(this.swap(BitConverter.ToString(((XDevkit.IXboxConsole) xbc).getMemory(getPlayerState(clientIndex) + 40, 4)).Replace("-", ""))), 0), BitConverter.ToSingle(Utils.StringToByteArray(this.swap(BitConverter.ToString(((XDevkit.IXboxConsole) xbc).getMemory(getPlayerState(clientIndex) + 0x2c, 4)).Replace("-", ""))), 0), BitConverter.ToSingle(Utils.StringToByteArray(this.swap(BitConverter.ToString(((XDevkit.IXboxConsole) xbc).getMemory(getPlayerState(clientIndex) + 0x30, 4)).Replace("-", ""))), 0) };
        }

        public uint getPlayerState(uint clientIndex)
        {
            byte[] array = ((XDevkit.IXboxConsole) xbc).getMemory((Suave.Classes.Addresses.G_Entites + (clientIndex * 0x31c)) + 340, 4);
            Array.Reverse(array);
            return BitConverter.ToUInt32(array, 0);
        }

        public uint getWeaponDef(uint weaponIndex)
        {
            return ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.BG_GetWeaponDef, new object[] { weaponIndex });
        }

        public void giveWeapon(uint clientIndex, string weaponName, uint modelVariant = 0, bool akimbo = false)
        {
            uint weaponIndex = this.G_GetWeaponIndexForName(weaponName);
            this.G_GivePlayerWeapon(clientIndex, weaponIndex, modelVariant, akimbo);
            this.G_InitializeAmmo(clientIndex, weaponIndex, modelVariant);
            SV_GameSendServerCommand(clientIndex, "h " + weaponIndex.ToString());
        }

        public uint HudElem_Alloc(uint clientNum)
        {
            XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.HudElem_Alloc);
            XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>(0);
            XDRPCArgumentInfo<uint> info2 = new XDRPCArgumentInfo<uint>(0);
            uint num = ((XDevkit.IXboxConsole) xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info, info2 });
            ((XDevkit.IXboxConsole) xbc).WriteUInt32(num + 0x7c, clientNum);
            return num;
        }

        public void iprintln(uint clientIndex, string text)
        {
            SV_GameSendServerCommand(clientIndex, string.Format("; \"{0}\"", text));
        }

        public void iprintlnbold(uint clientIndex, string text)
        {
            SV_GameSendServerCommand(clientIndex, string.Format("< \"{0}\"", text));
        }

        public bool isPlayerException(uint clientIndex)
        {
            return ((((clientIndex == this.playerSelected) || (clientIndex == this.player1)) || (clientIndex == this.player2)) || (clientIndex == this.playerMe));
        }

        public void print(bool clear, string text, params object[] args)
        {
            if (!clear)
            {
                this.logBox.Text = this.logBox.Text + string.Format(text, args) + Environment.NewLine;
            }
            else if (clear)
            {
                this.logBox.Text = string.Format(text, args) + Environment.NewLine;
            }
        }

        private uint R_RegisterModel(string modelName)
        {
            return ((XDevkit.IXboxConsole) xbc).Call(this.Addresses.R_RegisterModel, new object[] { modelName });
        }

        public void SetClanTagText(string clantag)
        {
            XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.SetClanTagText);
            XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>(0);
            XDRPCStringArgumentInfo info2 = new XDRPCStringArgumentInfo(clantag);
            ((XDevkit.IXboxConsole) xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info, info2 });
        }

        public void setClientClantag(uint clientIndex, string name)
        {
            string hex = BitConverter.ToString(Encoding.UTF8.GetBytes(name)).Replace("-", "");
            while ((hex.Length / 2) <= 4)
            {
                hex = hex + "00";
            }
            byte[] data = Utils.StringToByteArray(hex);
            ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState(clientIndex) + 0x55a0, data);
        }

        public void setClientName(uint clientIndex, string name)
        {
            string hex = BitConverter.ToString(Encoding.UTF8.GetBytes(name)).Replace("-", "");
            while ((hex.Length / 2) < 20)
            {
                hex = hex + "00";
            }
            byte[] data = Utils.StringToByteArray(hex);
            ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState(clientIndex) + 0x5534, data);
        }

        public void setFOV(uint fovScale)
        {
            SV_GameSendServerCommand(this.playerSelected, "^ 5 \"" + fovScale.ToString() + "\"");
            this.print(false, "{0}: FOV = {1}", new object[] { this.playerGamertag, fovScale });
        }

        public void SetJumpHeight(float value)
        {
            ((XDevkit.IXboxConsole) xbc).WriteFloat(this.Addresses.JumpHeight, value);
        }

        public void setModel(uint clientIndex, string modelName)
        {
            this.G_SetModel(clientIndex, ((XDevkit.IXboxConsole) xbc).ReadUInt32(this.R_RegisterModel(modelName)));
        }

        public void setOrigin(uint clientIndex, float[] origin)
        {
            ((XDevkit.IXboxConsole) xbc).WriteFloat(getPlayerState(clientIndex) + 40, origin[0]);
            ((XDevkit.IXboxConsole) xbc).WriteFloat(getPlayerState(clientIndex) + 0x2c, origin[1]);
            ((XDevkit.IXboxConsole) xbc).WriteFloat(getPlayerState(clientIndex) + 0x30, origin[2]);
        }

        public void setScore(uint score)
        {
            byte[] bytes = BitConverter.GetBytes(score);
            Array.Reverse(bytes);
            ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState(this.playerSelected) + 0x55c8, bytes);
            this.print(false, "{0}: Score = {1}", new object[] { this.playerGamertag, score });
        }

        public void SetXamGamertag(string gamertag)
        {
            byte[] data = ((XDevkit.IXboxConsole) xbc).WideChar(gamertag);
            ((XDevkit.IXboxConsole) xbc).setMemory(this.Addresses.g_rguserinfo + 0x1c, data);
        }

        public void SpawnModel()
        {
            byte[] data = ((XDevkit.IXboxConsole) xbc).getMemory(getPlayerState(0) + 40, 12);
            uint entity = this.G_Spawn();
            ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState(0) + 40, data);
            ((XDevkit.IXboxConsole) xbc).setMemory(entity + 0x134, data);
            uint num2 = entity + 0x176;
            uint num3 = ((XDevkit.IXboxConsole) xbc).Call(0x82531528, new object[] { num2, (short) 0xb94 });
            this.G_CallSpawnEntity(entity);
            ((XDevkit.IXboxConsole) xbc).WriteInt16(entity + 0x16c, this.G_ModelIndex(((XDevkit.IXboxConsole) xbc).ReadUInt32(this.R_RegisterModel("veh_t6_air_fa38_killstreak"))));
            ((XDevkit.IXboxConsole) xbc).WriteInt16(entity + 0x2ac, this.G_ModelIndex(((XDevkit.IXboxConsole) xbc).ReadUInt32(this.R_RegisterModel("veh_t6_air_fa38_killstreak"))));
        }

        //public DialogResult Suave_Error(string error, MessageBoxButtons buttons = 0, MessageBoxIcon icon = 0x10)
        //{
        //    return MessageBox.Show("Error: " + error, new SuaveFrm().Text, buttons, icon);
        //}

        public void SV_GameSendServerCommand(uint clientIndex, string text)
        {
            ((XDevkit.IXboxConsole)xbc).Call(Suave.Classes.Addresses.SV_GameSendServerCommand, new object[] { clientIndex, 1, text });
        }

        public string swap(string text)
        {
            string[] strArray = new string[] { text.Substring(0, 2), text.Substring(2, 2), text.Substring(4, 2), text.Substring(6, 2) };
            return (strArray[3] + strArray[2] + strArray[1] + strArray[0]);
        }

        public void takeAllWeapons(uint clientIndex)
        {
            ((XDevkit.IXboxConsole) xbc).WriteUInt32(getPlayerState(clientIndex) + 440, 0);
            ((XDevkit.IXboxConsole) xbc).WriteUInt32(getPlayerState(clientIndex) + 0x5444, 0);
            for (uint i = 0; i < 420; i += 0x1c)
            {
                this.BG_TakePlayerWeapon(clientIndex, ((XDevkit.IXboxConsole) xbc).ReadUInt32(getPlayerState(clientIndex) + (i + 0x248)));
            }
        }

        public void takeWeapon(uint clientIndex, string weaponName)
        {
            uint weaponIndex = this.G_GetWeaponIndexForName(weaponName);
            this.BG_TakePlayerWeapon(clientIndex, weaponIndex);
        }

        public void takeWeapon(uint clientIndex, uint weaponIndex)
        {
            this.BG_TakePlayerWeapon(clientIndex, weaponIndex);
        }

        public void TeleportAll(uint toClient, bool freeze)
        {
            float[] origin = this.getOrigin(toClient);
            for (uint i = 0; i <= this.numClients; i++)
            {
                if (!(this.isPlayerException(i) || (i == toClient)))
                {
                    uint input = ((XDevkit.IXboxConsole) xbc).ReadUInt32(getPlayerState(i) + 0x5684);
                    input = freeze ? 4 : input;
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(getPlayerState(i) + 0x5684, input);
                    this.setOrigin(i, origin);
                }
            }
        }

        public void testSpawnEnt(uint clientIndex)
        {
            uint num = getPlayerState(clientIndex);
            uint entity = this.G_Spawn();
            ((XDevkit.IXboxConsole) xbc).WriteFloat(entity + 0x134, ((XDevkit.IXboxConsole) xbc).ReadFloat(num + 40));
            ((XDevkit.IXboxConsole) xbc).WriteFloat(entity + 0x138, ((XDevkit.IXboxConsole) xbc).ReadFloat(num + 0x2c));
            ((XDevkit.IXboxConsole) xbc).WriteFloat(entity + 0x13c, ((XDevkit.IXboxConsole) xbc).ReadFloat((num + 0x30)) + 50f);
            ((XDevkit.IXboxConsole) xbc).setMemory(entity + 0x176, new byte[] { 0x11, 0xe4 });
            this.G_CallSpawnEntity(entity);
            ((XDevkit.IXboxConsole) xbc).WriteInt16(entity + 0x2ac, this.G_ModelIndex(((XDevkit.IXboxConsole) xbc).ReadUInt32(this.R_RegisterModel("veh_t6_air_fa38_killstreak"))));
            byte[] data = new byte[4];
            data[0] = 0x40;
            ((XDevkit.IXboxConsole) xbc).setMemory(entity + 0x36c, data);
            this.print(false, "Ent Address {0}", new object[] { entity.ToString("X") });
        }

        public void testUFO(uint clientIndex)
        {
            ((XDevkit.IXboxConsole) xbc).WriteUInt32(getPlayerState(clientIndex) + 0x5684, 3);
            byte[] buffer3 = new byte[4];
            buffer3[0] = 0x60;
            byte[] data = buffer3;
            ((XDevkit.IXboxConsole) xbc).setMemory(0x826980fc, data);
            buffer3 = new byte[4];
            buffer3[0] = 0x39;
            buffer3[1] = 0x40;
            byte[] buffer2 = buffer3;
            ((XDevkit.IXboxConsole) xbc).setMemory(0x82697fd0, buffer2);
            ((XDevkit.IXboxConsole) xbc).setMemory(0x826980dc, buffer2);
            ((XDevkit.IXboxConsole) xbc).setMemory(0x82697eb8, buffer2);
            ((XDevkit.IXboxConsole) xbc).setMemory(0x826980e8, new byte[] { 0x41, 0x9a, 0, 12 });
        }

        public void testUFO1(uint client)
        {
            byte[] curOrg = ((XDevkit.IXboxConsole) xbc).getMemory(getPlayerState(client) + 40, 12);
            ((XDevkit.IXboxConsole) xbc).setMemory(getPlayerState(client) + 40, turbineUFO);
            Task task = Task.Factory.StartNew(() => this.ufoAct((int) client, curOrg), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void ToggleACGodMode()
        {
            if (((XDevkit.IXboxConsole) xbc).getMemory(this.Addresses.GodMode, 1)[0] == 0x91)
            {
                byte[] data = new byte[4];
                data[0] = 0x60;
                ((XDevkit.IXboxConsole) xbc).setMemory(this.Addresses.GodMode, data);
                this.print(false, "All Client God Mode: On", new object[0]);
            }
            else
            {
                ((XDevkit.IXboxConsole) xbc).setMemory(this.Addresses.GodMode, new byte[] { 0x91, 0x1f, 1, 0xa8 });
                this.print(false, "All Client God Mode: Off", new object[0]);
            }
        }

        public void ToggleACUnlimitedAmmo()
        {
            byte[] buffer;
            if (((XDevkit.IXboxConsole) xbc).getMemory(this.Addresses.UnlimitedAmmo, 1)[0] == 0x90)
            {
                buffer = new byte[4];
                buffer[0] = 0x60;
                ((XDevkit.IXboxConsole) xbc).setMemory(this.Addresses.UnlimitedAmmo, buffer);
                this.print(false, "All Client Unlimited Ammo: On", new object[0]);
            }
            else
            {
                buffer = new byte[4];
                buffer[0] = 0x90;
                buffer[1] = 0xe3;
                ((XDevkit.IXboxConsole) xbc).setMemory(this.Addresses.UnlimitedAmmo, buffer);
                this.print(false, "All Client Unlimited Ammo: Off", new object[0]);
            }
        }

        public void ToggleAds(Client client)
        {
            if (client.AllowAds)
            {
                client.AllowAds = false;
                this.print(false, "{0}: Allow Ads = FALSE", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Allow Ads: ^1FALSE");
            }
            else
            {
                client.AllowAds = true;
                this.print(false, "{0}: Allow Ads = TRUE", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Allow Ads: ^2TRUE");
            }
        }

        public void ToggleCrosshairs(Client client)
        {
            uint playerState = client.PlayerState;
            if (((XDevkit.IXboxConsole) xbc).ReadUInt32((playerState + 0x1ec)) == 2)
            {
                ((XDevkit.IXboxConsole) xbc).WriteUInt32(playerState + 0x1ec, 1);
                ((XDevkit.IXboxConsole) xbc).WriteFloat(playerState + 0x588, 255f);
                client.SpreadOverride = 20;
                this.print(false, "{0}: Small Crosshairs Off", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Small Crosshairs: ^1Off");
            }
            else
            {
                client.SpreadOverride = 0;
                ((XDevkit.IXboxConsole) xbc).WriteUInt32(playerState + 0x1ec, 2);
                this.print(false, "{0}: Small Crosshairs On", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Small Crosshairs: ^2On");
            }
        }

        public void ToggleEMP()
        {
            byte[] buffer2;
            uint address = getPlayerState(this.playerSelected) + 0x18;
            if (((XDevkit.IXboxConsole) xbc).getMemory(address, 4)[3] == 4)
            {
                buffer2 = new byte[4];
                buffer2[3] = 0x40;
                ((XDevkit.IXboxConsole) xbc).setMemory(address, buffer2);
                this.print(false, "{0}: EMP On", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3EMP Jammed: ^2On");
            }
            else
            {
                buffer2 = new byte[4];
                buffer2[3] = 4;
                ((XDevkit.IXboxConsole) xbc).setMemory(address, buffer2);
                this.print(false, "{0}: EMP Off", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3EMP Jammed: ^1Off");
            }
        }

        public void ToggleForceUAV()
        {
            if (((XDevkit.IXboxConsole) xbc).getMemory(this.Addresses.ForceUAV, 1)[0] == 0x54)
            {
                byte[] data = new byte[4];
                data[0] = 0x38;
                data[1] = 0x60;
                ((XDevkit.IXboxConsole) xbc).setMemory(this.Addresses.ForceUAV, data);
                this.print(false, "Off-Host UAV: On", new object[0]);
            }
            else
            {
                ((XDevkit.IXboxConsole) xbc).setMemory(this.Addresses.ForceUAV, new byte[] { 0x54, 0x6b, 6, 0x3e });
                this.print(false, "Off-Host UAV: Off", new object[0]);
            }
        }

        public void ToggleFreeze(Client client)
        {
            if (client.MovementFlag != 4)
            {
                client.MovementFlag = 4;
                this.print(false, "{0}: Frozen On", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Frozen: ^2On");
            }
            else
            {
                client.MovementFlag = 0;
                this.print(false, "{0}: Frozen Off", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Frozen: ^1Off");
            }
        }

        public void ToggleGod()
        {
            uint playerSelected = this.playerSelected;
            uint address = this.getEntity(playerSelected) + 0x1a8;
            byte[] array = ((XDevkit.IXboxConsole) xbc).getMemory(address, 4);
            Array.Reverse(array);
            uint num3 = BitConverter.ToUInt32(array, 0);
            if ((num3 <= 100) && (num3 > 0))
            {
                ((XDevkit.IXboxConsole) xbc).setMemory(address, new byte[4]);
                this.print(false, "{0}: Godmode On", new object[] { this.playerGamertag });
                this.iprintln(playerSelected, "^3Godmode: ^2On");
            }
            else
            {
                byte[] data = new byte[4];
                data[3] = 100;
                ((XDevkit.IXboxConsole) xbc).setMemory(address, data);
                this.print(false, "{0}: Godmode Off", new object[] { this.playerGamertag });
                this.iprintln(playerSelected, "^3Godmode: ^1Off");
            }
        }

        public void ToggleHardcore(Client client)
        {
            if (client.RadarFlag != 0)
            {
                client.RadarFlag = 0;
                this.print(false, "{0}: Hardcore On", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Hardcore: ^2On");
            }
            else
            {
                client.RadarFlag = 1;
                this.print(false, "{0}: Hardcore Off", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Hardcore: ^1Off");
            }
        }

        public void ToggleInvisibility()
        {
            uint playerSelected = this.playerSelected;
            uint[] numArray = new uint[] { getPlayerState(playerSelected) + 0xfc, this.getEntity(playerSelected) + 0x100 };
            if (((XDevkit.IXboxConsole) xbc).getMemory(numArray[0], 4)[3] != 0x20)
            {
                byte[] data = new byte[4];
                data[3] = 0x20;
                ((XDevkit.IXboxConsole) xbc).setMemory(numArray[0], data);
                ((XDevkit.IXboxConsole) xbc).setMemory(numArray[1], new byte[4]);
                this.print(false, "{0}: Invisibility On", new object[] { this.playerGamertag });
                this.iprintln(playerSelected, "^3Invisibility: ^2On");
            }
            else
            {
                ((XDevkit.IXboxConsole) xbc).setMemory(numArray[0], new byte[4]);
                ((XDevkit.IXboxConsole) xbc).setMemory(numArray[1], new byte[4]);
                this.print(false, "{0}: Invisibility Off", new object[] { this.playerGamertag });
                this.iprintln(playerSelected, "^3Invisibility: ^1Off");
            }
        }

        public void ToggleJump(Client client)
        {
            if (client.AllowJump)
            {
                client.AllowJump = false;
                this.print(false, "{0}: Allow Jump = FALSE", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Allow Jump: ^1FALSE");
            }
            else
            {
                client.AllowJump = true;
                this.print(false, "{0}: Allow Jump = TRUE", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Allow Jump: ^2TRUE");
            }
        }

        public void ToggleSpectator()
        {
            uint address = getPlayerState(this.playerSelected) + 0x1b;
            byte num2 = ((XDevkit.IXboxConsole) xbc).getMemory(address, 1)[0];
            if (num2 == 4)
            {
                ((XDevkit.IXboxConsole) xbc).setMemory(address, new byte[] { 0xe7 });
                this.print(false, "{0}: Spectator On", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Spectator: ^2On");
            }
            else
            {
                ((XDevkit.IXboxConsole) xbc).setMemory(address, new byte[] { 4 });
                this.print(false, "{0}: Spectator Off", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Spectator: ^1Off");
            }
        }

        public void ToggleSprint(Client client)
        {
            if (client.AllowSprint)
            {
                client.AllowSprint = false;
                this.print(false, "{0}: Allow Sprint = FALSE", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Allow Sprint: ^1FALSE");
            }
            else
            {
                client.AllowSprint = true;
                this.print(false, "{0}: Allow Sprint = TRUE", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Allow Sprint: ^2TRUE");
            }
        }

        public void ToggleThirdPerson(Client client)
        {
            if (client.ThirdPerson)
            {
                client.ThirdPerson = false;
                this.print(false, "{0}: Third Person Off", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Third Person: ^1Off");
            }
            else
            {
                client.ThirdPerson = true;
                this.print(false, "{0}: Third Person On", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Third Person: ^2On");
            }
        }

        public void ToggleUAV(Client client)
        {
            if (client.RadarFlag != 3)
            {
                client.RadarFlag = 3;
                this.print(false, "{0}: UAV On", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Automatic UAV: ^2On");
            }
            else
            {
                client.RadarFlag = 1;
                this.print(false, "{0}: UAV Off", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Automatic UAV: ^1Off");
            }
        }

        public void ToggleWeapons(Client client)
        {
            if (client.WeaponFlag != 0x80L)
            {
                client.WeaponFlag = 0x80L;
                this.print(false, "{0}: Weapons Disabled", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Weapons: ^1Off");
            }
            else
            {
                client.WeaponFlag = 0L;
                this.print(false, "{0}: Weapons Enabled", new object[] { this.playerGamertag });
                this.iprintln(this.playerSelected, "^3Weapons: ^2On");
            }
        }

        public class Client
        {
            private uint clientID = 0;
            private Suave.Classes.SuaveLib SuaveLib;
            private XDevkit.IXboxConsole xbc;

            public Client(XDevkit.IXboxConsole Oxbc, uint clientIndex, Suave.Classes.SuaveLib _SuaveLib)
            {
                this.xbc = Oxbc;
                this.clientID = clientIndex;
                this.SuaveLib = _SuaveLib;
            }

            public void EmptyAmmo()
            {
                for (uint i = 0; i <= 11; i++)
                {
                    ((XDevkit.IXboxConsole)xbc).setMemory((this.PlayerState + 0x3fc) + (i * 4), new byte[4]);
                }
                for (uint j = 0; j <= 14; j++)
                {
                    ((XDevkit.IXboxConsole)xbc).setMemory((this.PlayerState + 0x438) + (j * 4), new byte[4]);
                }
                this.SuaveLib.iprintln(this.clientID, "^1Emptied Ammo");
            }

            public void GiveKillstreaks()
            {
                for (uint i = 0; i <= 3; i++)
                {
                    ((XDevkit.IXboxConsole) xbc).setMemory((this.PlayerState + 0x42c) + (i * 4), new byte[] { 0xff, 0xff, 0xff, 0xff });
                }
                this.SuaveLib.iprintln(this.clientID, "^3Killstreaks ^2Given");
            }

            public void giveWeapon(string weaponName, uint modelVariant = 0, bool akimbo = false)
            {
                this.SuaveLib.giveWeapon(this.clientID, weaponName, modelVariant, akimbo);
            }

            public void MaxAmmo()
            {
                for (uint i = 0; i <= 11; i++)
                {
                    ((XDevkit.IXboxConsole) xbc).setMemory((this.PlayerState + 0x3fc) + (i * 4), new byte[] { 0x7f, 0xff, 0xff, 0xff });
                }
                for (uint j = 0; j <= 14; j++)
                {
                    ((XDevkit.IXboxConsole) xbc).setMemory((this.PlayerState + 0x438) + (j * 4), new byte[] { 0x7f, 0xff, 0xff, 0xff });
                }
                this.SuaveLib.iprintln(this.clientID, "^3Max Ammo: ^2On");
            }

            public void setModel(string modelName)
            {
                this.SuaveLib.setModel(this.clientID, modelName);
            }

            public void SetPrimaryCamo(uint index)
            {
                ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x2d8, index);
                this.SuaveLib.iprintln(this.clientID, string.Format("^3Primary Camo [{0}] ^2Given", index));
            }

            public void SetSecondaryCamo(uint index)
            {
                ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 700, index);
                this.SuaveLib.iprintln(this.clientID, string.Format("^3Secondary Camo [{0}] ^2Given", index));
            }

            public void StartAmmo()
            {
                this.SuaveLib.G_InitializeAmmo(this.clientID, this.SuaveLib.BG_GetViewmodelWeaponIndex(this.clientID), 0);
                this.SuaveLib.iprintln(this.clientID, "^3Gave Start Ammo");
            }

            public void takeAllWeapons()
            {
                this.SuaveLib.takeAllWeapons(this.clientID);
            }

            public void TakeKillstreaks()
            {
                for (uint i = 0; i <= 3; i++)
                {
                    ((XDevkit.IXboxConsole) xbc).setMemory((this.PlayerState + 0x42c) + (i * 4), new byte[4]);
                }
                this.SuaveLib.iprintln(this.clientID, "^3Killstreaks ^1Taken");
            }

            public void visionSetNaked(string vision, int fadeInTime = 2)
            {
                //Suave.Classes.SuaveLib.SV_GameSendServerCommand(this.clientID, string.Format("2 1060 \"{0}\" {1}", vision, fadeInTime * 0x3e8));
            }

            public bool AllowAds
            {
                get
                {
                    if (this.WeaponFlag == 0x20L)
                    {
                        return false;
                    }
                    return true;
                }
                set
                {
                    if (value)
                    {
                        this.WeaponFlag = 0L;
                    }
                    else
                    {
                        this.WeaponFlag = 0x20L;
                    }
                }
            }

            public bool AllowJump
            {
                get
                {
                    if (((XDevkit.IXboxConsole) xbc).ReadUInt16((this.PlayerState + 12)) == 8)
                    {
                        return false;
                    }
                    return true;
                }
                set
                {
                    if (value)
                    {
                        ((XDevkit.IXboxConsole) xbc).WriteUInt16(this.PlayerState + 12, 0);
                    }
                    else
                    {
                        ((XDevkit.IXboxConsole) xbc).WriteUInt16(this.PlayerState + 12, 8);
                    }
                }
            }

            public bool AllowMelee
            {
                get
                {
                    if (((XDevkit.IXboxConsole) xbc).ReadUInt16((this.PlayerState + 12)) == 0x2000)
                    {
                        return false;
                    }
                    return true;
                }
                set
                {
                    if (value)
                    {
                        ((XDevkit.IXboxConsole) xbc).WriteUInt16(this.PlayerState + 12, 0);
                    }
                    else
                    {
                        ((XDevkit.IXboxConsole) xbc).WriteUInt16(this.PlayerState + 12, 0x2000);
                    }
                }
            }

            public uint AllowSpectateTeamFlag
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x54e8));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x54e8, value);
                }
            }

            public bool AllowSprint
            {
                get
                {
                    if (((XDevkit.IXboxConsole) xbc).ReadUInt16((this.PlayerState + 12)) == 4)
                    {
                        return false;
                    }
                    return true;
                }
                set
                {
                    if (value)
                    {
                        ((XDevkit.IXboxConsole) xbc).WriteUInt16(this.PlayerState + 12, 0);
                    }
                    else
                    {
                        ((XDevkit.IXboxConsole) xbc).WriteUInt16(this.PlayerState + 12, 4);
                    }
                }
            }

            public string clientClantag
            {
                get
                {
                    return this.SuaveLib.getClientClantag(this.clientID);
                }
                set
                {
                    this.SuaveLib.setClientClantag(this.clientID, value);
                }
            }

            public string clientName
            {
                get
                {
                    return this.SuaveLib.getClientName(this.clientID);
                }
                set
                {
                    this.SuaveLib.setClientName(this.clientID, value);
                }
            }

            public uint Deaths
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x55d4));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x55d4, value);
                }
            }

            public uint Entity
            {
                get
                {
                    return this.SuaveLib.getEntity(this.clientID);
                }
            }

            public uint Kills
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x55cc));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x55cc, value);
                }
            }

            public uint MovementFlag
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x5684));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x5684, value);
                }
            }

            public uint PlayerState
            {
                get
                {
                    return 0; //Suave.Classes.SuaveLib.getPlayerState(this.clientID);
                }
            }

            public uint Prestige
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x555c));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x555c, value);
                }
            }

            public uint RadarFlag
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x5604));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x5604, value);
                }
            }

            public uint Rank
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x5558));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x5558, value);
                }
            }

            public uint Score
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x55c8));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x55c8, value);
                }
            }

            public uint SessionState
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x5410));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x5410, value);
                }
            }

            public uint SpreadOverride
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 0x1e8));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 0x1e8, value);
                }
            }

            public bool ThirdPerson
            {
                get
                {
                    return (((XDevkit.IXboxConsole) xbc).ReadByte((this.PlayerState + 0x84)) == 1);
                }
                set
                {
                    if (value)
                    {
                        ((XDevkit.IXboxConsole) xbc).WriteByte(this.PlayerState + 0x84, 1);
                    }
                    else
                    {
                        ((XDevkit.IXboxConsole) xbc).WriteByte(this.PlayerState + 0x84, 0);
                    }
                }
            }

            public bool UFOMode
            {
                get
                {
                    return (this.SessionState == 2);
                }
                set
                {
                    if (value)
                    {
                        float[] origin = this.SuaveLib.getOrigin(this.clientID);
                        this.SuaveLib.setOrigin(this.clientID, new float[] { -4870f, -77f, 40f });
                        Thread.Sleep(200);
                        this.AllowSpectateTeamFlag = 0x800;
                        this.SessionState = 2;
                        this.SuaveLib.setOrigin(this.clientID, new float[] { -4870f, -77f, 40f });
                        this.SuaveLib.setOrigin(this.clientID, origin);
                    }
                    else
                    {
                        this.SessionState = 0;
                    }
                }
            }

            public uint ViewmodelIndex
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt32((this.PlayerState + 440));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt32(this.PlayerState + 440, value);
                }
            }

            public uint WeaponDef
            {
                get
                {
                    return this.SuaveLib.getWeaponDef(this.ViewmodelIndex);
                }
            }

            public ulong WeaponFlag
            {
                get
                {
                    return ((XDevkit.IXboxConsole) xbc).ReadUInt64((this.PlayerState + 0x10));
                }
                set
                {
                    ((XDevkit.IXboxConsole) xbc).WriteUInt64(this.PlayerState + 0x10, value);
                }
            }
        }

        public class HudElem
        {
            public uint elem = 0;
            public Suave.Classes.SuaveLib SuaveLib;
            public XDevkit.IXboxConsole xbc;

            public HudElem(uint elemAddress, Suave.Classes.SuaveLib _SuaveLib)
            {
                this.elem = elemAddress;
                this.SuaveLib = _SuaveLib;
                this.xbc = xbc;
            }

            public void fadeOverTime(Suave.Classes.SuaveLib.rgba FromColor, short FadeTime)
            {
                this.fadeStartTime = this.getLevelTime();
                this.fadeTime = FadeTime;
                this.fromColor = FromColor.getRGBA();
            }

            public int getLevelTime()
            {
                return ((XDevkit.IXboxConsole) this.xbc).ReadInt32(new Addresses().LevelTime);
            }

            public void moveOverTime(float endX, float endY, short Duration, bool scrollText = false)
            {
                byte[] data = ((XDevkit.IXboxConsole) this.xbc).getMemory(this.elem, 8);
                byte[] buffer2 = ((XDevkit.IXboxConsole) this.xbc).getMemory(this.elem + 0x72, 2);
                this.moveStartTime = this.getLevelTime();
                this.moveTime = Duration;
                if (scrollText)
                {
                    byte[] buffer3 = new byte[8];
                    buffer3[0] = 0x44;
                    buffer3[1] = 250;
                    ((XDevkit.IXboxConsole) this.xbc).setMemory(this.elem + 40, buffer3);
                    ((XDevkit.IXboxConsole) this.xbc).setMemory(this.elem + 0x76, new byte[] { 4, 0x23 });
                }
                else
                {
                    ((XDevkit.IXboxConsole) this.xbc).setMemory(this.elem + 40, data);
                    ((XDevkit.IXboxConsole) this.xbc).setMemory(this.elem + 0x76, buffer2);
                }
                this.x = endX;
                this.y = endY;
            }

            public void scaleOverTime(float FromFontScale, short ScaleTime)
            {
                this.scaleStartTime = this.getLevelTime();
                this.fromFontScale = FromFontScale;
                this.scaleTime = ScaleTime;
            }

            public void setFlagForeground(bool on)
            {
                int flags = this.flags;
                int num2 = on ? (flags |= 1) : (flags &= -2);
                this.flags = num2;
            }

            public void setFlagHideWhenDead(bool on)
            {
                int flags = this.flags;
                int num2 = on ? (flags |= 2) : (flags &= -3);
                this.flags = num2;
            }

            public void setFlagHideWhenInMenu(bool on)
            {
                int flags = this.flags;
                int num2 = on ? (flags |= 4) : (flags &= -5);
                this.flags = num2;
            }

            public void setObjectiveText(string Text, short FxLetterTime, short FxDecayStartTime, short FxDecayDuration)
            {
                this.fxBirthTime = this.getLevelTime();
                this.fxLetterTime = FxLetterTime;
                this.fxDecayStartTime = FxDecayStartTime;
                this.fxDecayDuration = FxDecayDuration;
                this.flags = 0x18800;
                this.text = this.SuaveLib.G_LocalizedStringIndex(Text);
            }

            public void setShader(string MaterialName, float X, float Y, short Width, short Height, byte AlignOrg, byte AlignScreen, Suave.Classes.SuaveLib.rgba RGBA, float Sort = 0f)
            {
                this.type = 8;
                this.alignOrg = AlignOrg;
                this.alignScreen = AlignScreen;
                this.width = Width;
                this.height = Height;
                this.x = X;
                this.y = Y;
                this.color = RGBA.getRGBA();
                this.sort = Sort;
                this.materialIndex = this.SuaveLib.G_MaterialIndex(MaterialName);
            }

            public void setText(string Text, float X, float Y, float FontScale, byte AlignOrg, byte AlignScreen, Suave.Classes.SuaveLib.rgba RGBA, float Sort = 1f)
            {
                this.type = 1;
                this.alignOrg = AlignOrg;
                this.alignScreen = AlignScreen;
                this.fontScale = FontScale;
                this.x = X;
                this.y = Y;
                this.color = RGBA.getRGBA();
                this.sort = Sort;
                this.text = this.SuaveLib.G_LocalizedStringIndex(Text);
            }

            public int abilityFlag
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x84));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x84, value);
                }
            }

            public byte alignOrg
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x72));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x72, value);
                }
            }

            public byte alignScreen
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x73));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x73, value);
                }
            }

            public int clientIndex
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x7c));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x7c, value);
                }
            }

            public byte[] color
            {
                get
                {
                    byte[] buffer = ((XDevkit.IXboxConsole) this.xbc).getMemory(this.elem + 0x18, 4);
                    Suave.Classes.SuaveLib.rgba rgba = new Suave.Classes.SuaveLib.rgba(buffer[0], buffer[1], buffer[2], buffer[3]);
                    return rgba.getRGBA();
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).setMemory(this.elem + 0x18, value);
                }
            }

            public int duration
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x38));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x38, value);
                }
            }

            public int fadeStartTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x20));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x20, value);
                }
            }

            public short fadeTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x54));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x54, value);
                }
            }

            public int flags
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x4c));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x4c, value);
                }
            }

            public float fontScale
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat((this.elem + 12));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem + 12, value);
                }
            }

            public byte fontStyle
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x71));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x71, value);
                }
            }

            public byte fromAlignOrg
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x76));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x76, value);
                }
            }

            public byte fromAlignScreen
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x77));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x77, value);
                }
            }

            public byte[] fromColor
            {
                get
                {
                    byte[] buffer = ((XDevkit.IXboxConsole) this.xbc).getMemory(this.elem + 0x1c, 4);
                    Suave.Classes.SuaveLib.rgba rgba = new Suave.Classes.SuaveLib.rgba(buffer[0], buffer[1], buffer[2], buffer[3]);
                    return rgba.getRGBA();
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).setMemory(this.elem + 0x1c, value);
                }
            }

            public float fromFontScale
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat((this.elem + 0x10));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem + 0x10, value);
                }
            }

            public short fromHeight
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x5e));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x5e, value);
                }
            }

            public short fromWidth
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x5c));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x5c, value);
                }
            }

            public float fromX
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat((this.elem + 40));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem + 40, value);
                }
            }

            public float fromY
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat((this.elem + 0x2c));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem + 0x2c, value);
                }
            }

            public int fxBirthTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x48));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x48, value);
                }
            }

            public short fxDecayDuration
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x6a));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x6a, value);
                }
            }

            public short fxDecayStartTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x68));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x68, value);
                }
            }

            public short fxLetterTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x66));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x66, value);
                }
            }

            public short fxRedactDecayDuration
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 110));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 110, value);
                }
            }

            public short fxRedactDecayStartTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x6c));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x6c, value);
                }
            }

            public byte[] glowColor
            {
                get
                {
                    byte[] buffer = ((XDevkit.IXboxConsole) this.xbc).getMemory(this.elem + 0x44, 4);
                    Suave.Classes.SuaveLib.rgba rgba = new Suave.Classes.SuaveLib.rgba(buffer[0], buffer[1], buffer[2], buffer[3]);
                    return rgba.getRGBA();
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).setMemory(this.elem + 0x44, value);
                }
            }

            public short height
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 90));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 90, value);
                }
            }

            public short label
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x56));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x56, value);
                }
            }

            public byte materialIndex
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x74));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x74, value);
                }
            }

            public int moveStartTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x30));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x30, value);
                }
            }

            public short moveTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x60));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x60, value);
                }
            }

            public byte offscreenMaterialIdx
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x75));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x75, value);
                }
            }

            public int scaleStartTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 20));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 20, value);
                }
            }

            public short scaleTime
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 80));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 80, value);
                }
            }

            public float sort
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat((this.elem + 0x40));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem + 0x40, value);
                }
            }

            public byte soundID
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 120));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 120, value);
                }
            }

            public int team
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x80));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x80, value);
                }
            }

            public short text
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 100));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 100, value);
                }
            }

            public int time
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt32((this.elem + 0x34));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt32(this.elem + 0x34, value);
                }
            }

            public byte type
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x70));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x70, value);
                }
            }

            public byte ui3dWindow
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadByte((this.elem + 0x79));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteByte(this.elem + 0x79, value);
                }
            }

            public float value
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat((this.elem + 60));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem + 60, value);
                }
            }

            public short width
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadInt16((this.elem + 0x58));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteInt16(this.elem + 0x58, value);
                }
            }

            public float x
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat(this.elem);
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem, value);
                }
            }

            public float y
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat((this.elem + 4));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem + 4, value);
                }
            }

            public float z
            {
                get
                {
                    return ((XDevkit.IXboxConsole) this.xbc).ReadFloat((this.elem + 8));
                }
                set
                {
                    ((XDevkit.IXboxConsole) this.xbc).WriteFloat(this.elem + 8, value);
                }
            }
        }

        public class mpdata
        {
            private Suave.Classes.Addresses Addresses = new Suave.Classes.Addresses();
            private Suave.Classes.SuaveLib SuaveLib;
            private XDevkit.IXboxConsole xbc;
            public uint[] XP_RANGES = new uint[] { 
                0, 800, 0x76c, 0xdac, 0x157c, 0x1f40, 0x2cec, 0x3a98, 0x55f0, 0x7530, 0x88b8, 0x9c40, 0xafc8, 0xcd14, 0xf424, 0x124f8, 
                0x14244, 0x17318, 0x1a964, 0x1d4c0, 0x20f58, 0x249f0, 0x28e4c, 0x2bf20, 0x3037c, 0x35b60, 0x3a980, 0x41eb0, 0x46cd0, 0x4baf0, 0x50910, 0x55730, 
                0x5cc60, 0x61a80, 0x6b6c0, 0x72bf0, 0x7a120, 0x81650, 0x86470, 0x8ed28, 0x975e0, 0x9eb10, 0xa6040, 0xafc80, 0xb71b0, 0xc0df0, 0xcaa30, 0xd59f8, 
                0xe7ef0, 0xf4240, 0xfa3e8, 0x100590, 0x10c8e0, 0x11edd8, 0x124f80
             };

            public mpdata(Suave.Classes.SuaveLib _SuaveLib, XDevkit.IXboxConsole _xbc)
            {
                this.SuaveLib = _SuaveLib;
                this.xbc = _xbc;
            }

            public byte[] getCamoId(string camo)
            {
                byte[] buffer2;
                switch (camo)
                {
                    case "None":
                        return new byte[2];

                    case "DEVGRU":
                        buffer2 = new byte[2];
                        buffer2[0] = 0x40;
                        return buffer2;

                    case "A-TACS AU":
                        buffer2 = new byte[2];
                        buffer2[0] = 0x80;
                        return buffer2;

                    case "ERDL":
                        buffer2 = new byte[2];
                        buffer2[0] = 0xc0;
                        return buffer2;

                    case "Siberia":
                        buffer2 = new byte[2];
                        buffer2[1] = 1;
                        return buffer2;

                    case "Choco":
                        return new byte[] { 0x40, 1 };

                    case "Blue Tiger":
                        return new byte[] { 0x80, 1 };

                    case "Bloodshot":
                        return new byte[] { 0xc0, 1 };

                    case "Ghostex":
                        buffer2 = new byte[2];
                        buffer2[1] = 2;
                        return buffer2;

                    case "Typhon":
                        return new byte[] { 0x40, 2 };

                    case "Carbon Fiber":
                        return new byte[] { 0x80, 2 };

                    case "Cherry Blossom":
                        return new byte[] { 0xc0, 2 };

                    case "Art of War":
                        buffer2 = new byte[2];
                        buffer2[1] = 3;
                        return buffer2;

                    case "Ronin":
                        return new byte[] { 0x40, 3 };

                    case "Gold":
                        return new byte[] { 0xc0, 3 };

                    case "Diamond":
                        buffer2 = new byte[2];
                        buffer2[1] = 4;
                        return buffer2;

                    case "Elite":
                        return new byte[] { 0x40, 4 };

                    case "Digital":
                        return new byte[] { 0x80, 4 };
                }
                return new byte[2];
            }

            public byte[] getLethalId(string lethal)
            {
                switch (lethal)
                {
                    case "Grenade":
                        return new byte[] { 240, 0x13 };

                    case "Semtex":
                        return new byte[] { 0x10, 20 };

                    case "Combat Axe":
                    {
                        byte[] buffer2 = new byte[2];
                        buffer2[1] = 20;
                        return buffer2;
                    }
                    case "Bouncing Betty":
                        return new byte[] { 0x30, 20 };

                    case "C4":
                        return new byte[] { 0x20, 20 };

                    case "Claymore":
                        return new byte[] { 0x40, 20 };
                }
                return new byte[2];
            }

            public byte[] getPerkId(string perk)
            {
                if ((((perk == "Extreme Condition") || (perk == "Tactical Mask")) || (perk == "Dead Silence")) || (perk == "Awareness"))
                {
                    //this.SuaveLib.Suave_Error("Extreme Conditioning, Tactical Mask, and Dead Silence are currently not working. Please choose another option", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return new byte[1];
                }
                switch (perk)
                {
                    case "Lightweight":
                        return new byte[] { 0x40 };

                    case "Hardline":
                        return new byte[] { 0x70 };

                    case "Blind Eye":
                        return new byte[] { 0x60 };

                    case "Flak Jacket":
                        return new byte[] { 80 };

                    case "Ghost":
                        return new byte[] { 0x80 };

                    case "Toughness":
                        return new byte[] { 0xd0 };

                    case "Cold Blooded":
                        return new byte[] { 0xb0 };

                    case "Fast Hands":
                        return new byte[] { 0xc0 };

                    case "Hard Wired":
                        return new byte[] { 0x90 };

                    case "Scavenger":
                        return new byte[] { 160 };

                    case "Dexterity":
                        return new byte[] { 0xe0 };

                    case "Extreme Conditioning":
                        return new byte[1];

                    case "Engineer":
                        return new byte[] { 240 };

                    case "Tactical Mask":
                        return new byte[1];

                    case "Dead Silence":
                        return new byte[1];

                    case "Awareness":
                        return new byte[1];
                }
                return new byte[1];
            }

            public decimal getStat(string statName)
            {
                XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.GetIntPlayerStat);
                XDRPCArgumentInfo<int> info = new XDRPCArgumentInfo<int>(0);
                XDRPCStringArgumentInfo info2 = new XDRPCStringArgumentInfo(statName);
                return ((XDevkit.IXboxConsole) this.xbc).ExecuteRPC<int>(options, new XDRPCArgumentInfo[] { info, info2 });
            }

            public int getStatIndex(string statName)
            {
                switch (statName)
                {
                    case "plevel":
                        return 310;

                    case "rankxp":
                        return 0x13a;

                    case "rank":
                        return 0x139;

                    case "score":
                        return 320;

                    case "kills":
                        return 0x52;

                    case "deaths":
                        return 0x20;

                    case "kdratio":
                        return 0x4f;

                    case "TEAMKILLS":
                        return 0x156;

                    case "SUICIDES":
                        return 0x152;

                    case "HEADSHOTS":
                        return 0x4a;

                    case "hits":
                        return 0x4c;

                    case "misses":
                        return 280;

                    case "accuracy":
                        return 0;

                    case "assists":
                        return 6;

                    case "wins":
                        return 0x16b;

                    case "losses":
                        return 0x93;

                    case "ties":
                        return 0x158;

                    case "TIME_PLAYED_ALLIES":
                        return 0x15b;

                    case "TIME_PLAYED_AXIS":
                        return 0x15c;

                    case "TIME_PLAYED_OTHER":
                        return 350;

                    case "TIME_PLAYED_TOTAL":
                        return 0x160;
                }
                return -1;
            }

            public byte[] getTacticalId(string tactical)
            {
                switch (tactical)
                {
                    case "Concussion":
                        return new byte[] { 0x90, 0x23, 0x81, 1 };

                    case "Smoke Grenade":
                        return new byte[] { 0x10, 0xa3, 0x80, 0 };

                    case "Sensor Grenade":
                        return new byte[] { 0x90, 0x24, 0x81, 1 };

                    case "EMP Grenade":
                        return new byte[] { 0x10, 0x24, 0x81, 1 };

                    case "Shock Charge":
                        return new byte[] { 0x90, 0x25, 0x81, 1 };

                    case "Black Hat":
                        return new byte[] { 0x10, 0x26, 0x81, 1 };

                    case "Flashbang":
                        return new byte[] { 0x10, 0x25, 0x81, 1 };

                    case "Trophy System":
                        return new byte[] { 0x10, 0x27, 0x81, 1 };

                    case "Tactical Insertion":
                        return new byte[] { 0x90, 0xa6, 0x80, 0 };
                }
                return new byte[4];
            }

            public byte[] getWeaponIdForClasses(int classNum, string weapon)
            {
                byte[] buffer3;
                byte[] buffer = new byte[6];
                if (weapon == "MTAR")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 8;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 8, 0x80, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 8;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x80;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[5] = 8;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Type 25")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 7;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 8, 0x70, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 7;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x70;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[5] = 7;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "SWAT-556")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0xc0;
                        buffer3[3] = 6;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0xc0, 0x6c, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 6;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 0x6c;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xc0;
                        buffer3[5] = 6;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "FAL OSW")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x40;
                        buffer3[3] = 7;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x40, 0x74, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 7;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 0x74;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x40;
                        buffer3[5] = 7;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "M27")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0xc0;
                        buffer3[3] = 7;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0xc0, 0x7c, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 7;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 0x7c;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xc0;
                        buffer3[5] = 7;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Scar-H")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x40;
                        buffer3[3] = 6;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x40, 100, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 6;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 100;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x40;
                        buffer3[5] = 6;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "SMR")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x80;
                        buffer3[3] = 7;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x80, 120, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 7;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 120;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x80;
                        buffer3[5] = 7;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "M8A1")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 6;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 8, 0x60, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 6;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x60;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[5] = 6;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "AN-94")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x80;
                        buffer3[3] = 6;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x80, 0x68, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 6;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 0x68;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x80;
                        buffer3[5] = 6;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "MP7")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x40;
                        buffer3[3] = 3;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x40, 0x34, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 3;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 0x34;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x40;
                        buffer3[5] = 3;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "POW-57")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0xc0;
                        buffer3[3] = 3;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0xc0, 60, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 3;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 60;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xc0;
                        buffer3[5] = 3;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Vector KIO")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x80;
                        buffer3[3] = 4;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x80, 0x48, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 4;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 0x48;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x80;
                        buffer3[5] = 4;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "MSMC")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x40;
                        buffer3[3] = 4;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x40, 0x44, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 4;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 0x44;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x40;
                        buffer3[5] = 4;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Chicom CQ8")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 4;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 8, 0x40, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 4;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x40;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[5] = 4;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Skorpion EVO")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x80;
                        buffer3[3] = 3;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x80, 0x38, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 3;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 0x38;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x80;
                        buffer3[5] = 3;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "R870 MCS")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0xc0;
                        buffer3[3] = 11;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0xc0, 0xbc, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 11;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 0xbc;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xc0;
                        buffer3[5] = 11;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "M1216")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 12;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 8, 0xc0, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 12;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xc0;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[5] = 12;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Mk 48")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 9;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 8, 0x90, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 9;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x90;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[5] = 9;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "D88 LSW")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x40;
                        buffer3[3] = 9;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x40, 0x94, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 9;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 0x94;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x40;
                        buffer3[5] = 9;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "LSAT")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x80;
                        buffer3[3] = 9;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x80, 0x98, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 9;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 0x98;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x80;
                        buffer3[5] = 9;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "HAMR")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0xc0;
                        buffer3[3] = 9;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0xc0, 0x9c, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 9;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 0x9c;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xc0;
                        buffer3[5] = 9;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "SVU-AS")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0xc0;
                        buffer3[3] = 10;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0xc0, 0xac, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 10;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc0;
                        buffer3[4] = 0xac;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xc0;
                        buffer3[5] = 10;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "DSR 50")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 11;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 8, 0xb0, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 11;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xb0;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[5] = 11;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Ballista")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x80;
                        buffer3[3] = 10;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x80, 0xa8, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 10;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 0xa8;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x80;
                        buffer3[5] = 10;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "XPR-50")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x40;
                        buffer3[3] = 11;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        return new byte[] { 0, 0, 0x40, 180, 8, 0 };
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 11;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x40;
                        buffer3[4] = 180;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x40;
                        buffer3[5] = 11;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Five-seven")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0xc7;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 12;
                        return buffer3;
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xc7;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 12;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xc7;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Tac-45")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x67;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 6;
                        return buffer3;
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x67;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 6;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x67;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "B23R")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x87;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 8;
                        return buffer3;
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x87;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 8;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x87;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Executioner")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0xa7;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 10;
                        return buffer3;
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0xa7;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 10;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0xa7;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "KAP-40")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x47;
                        return buffer3;
                    }
                    if (classNum == 2)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 4;
                        return buffer3;
                    }
                    if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x47;
                        return buffer3;
                    }
                    if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 4;
                        return buffer3;
                    }
                    if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x47;
                        buffer = buffer3;
                    }
                    return buffer;
                }
                if (weapon == "Death Machine")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x80;
                        buffer3[3] = 20;
                        buffer = buffer3;
                    }
                    else if (classNum == 2)
                    {
                        buffer = new byte[] { 0, 0, 0x80, 20, 8, 0 };
                    }
                    else if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 20;
                        buffer = buffer3;
                    }
                    else if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x80;
                        buffer3[4] = 20;
                        buffer = buffer3;
                    }
                    else if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x80;
                        buffer3[5] = 20;
                        buffer = buffer3;
                    }
                    if ((classNum == 2) || (classNum == 4))
                    {
                        //this.SuaveLib.Suave_Error("Death Machine will not work on classes 2 or 4. Please select class 1, 3, or 5.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    return buffer;
                }
                if (weapon == "War Machine")
                {
                    if (classNum == 1)
                    {
                        buffer3 = new byte[6];
                        buffer3[2] = 0x48;
                        buffer3[3] = 15;
                        buffer = buffer3;
                    }
                    else if (classNum == 2)
                    {
                        buffer = new byte[] { 0, 0, 0x48, 15, 8, 0 };
                    }
                    else if (classNum == 3)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x48;
                        buffer3[4] = 15;
                        buffer = buffer3;
                    }
                    else if (classNum == 4)
                    {
                        buffer3 = new byte[6];
                        buffer3[3] = 0x48;
                        buffer3[4] = 15;
                        buffer = buffer3;
                    }
                    else if (classNum == 5)
                    {
                        buffer3 = new byte[6];
                        buffer3[4] = 0x48;
                        buffer3[5] = 15;
                        buffer = buffer3;
                    }
                    if ((classNum == 2) || (classNum == 4))
                    {
                        //this.SuaveLib.Suave_Error("War Machine will not work on classes 2 or 4. Please select class 1, 3, or 5.", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                return buffer;
            }

            public void MakeStableStatsBuffer()
            {
                XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.SetIntPlayerStat);
                XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>(((XDevkit.IXboxConsole) this.xbc).ExecuteRPC<uint>(new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.StableBufferHelper), new XDRPCArgumentInfo[] { new XDRPCArgumentInfo<uint>(0) }));
                ((XDevkit.IXboxConsole) this.xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info });
            }

            public void setClassName(int classNum, string className)
            {
                this.SuaveLib.Cbuf_AddText(string.Format("setStatFromLocString cacLoadouts customClassName {0} {1}", classNum, className));
                this.SuaveLib.Cbuf_AddText("updategamerprofile");
            }

            public void setLethal(int classNum, string lethal)
            {
                byte[] data = this.getLethalId(lethal);
                uint address = (this.Addresses.MPDATA + 0xae69) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void setPerk1(int classNum, string perk)
            {
                byte[] data = this.getPerkId(perk);
                uint address = (this.Addresses.MPDATA + 0xae63) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void setPerk2(int classNum, string perk)
            {
                byte[] data = this.getPerkId(perk);
                uint address = (this.Addresses.MPDATA + 0xae64) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void setPerk3(int classNum, string perk)
            {
                byte[] data = this.getPerkId(perk);
                uint address = (this.Addresses.MPDATA + 0xae65) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void setPrimaryAttachment1(int classNum, byte index)
            {
                uint offset = (this.Addresses.MPDATA + 0xae75) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).WriteByte(offset, (byte) (index * 4));
            }

            public void setPrimaryAttachment2(int classNum, byte index)
            {
                uint offset = (this.Addresses.MPDATA + 0xae76) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).WriteByte(offset, (byte) (index * 4));
            }

            public void setPrimaryCamo(int classNum, string camo)
            {
                byte[] data = this.getCamoId(camo);
                uint address = (this.Addresses.MPDATA + 0xae4d) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void setPrimaryWeapon(int classNum, string weapon)
            {
                byte[] data = this.getWeaponIdForClasses(classNum, weapon);
                uint address = (uint) (((this.Addresses.MPDATA + 0xae47) + ((classNum - 1) * 0x34)) - 2);
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void setSecondaryAttachment(int classNum, byte index)
            {
                uint offset = (this.Addresses.MPDATA + 0xae78) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).WriteByte(offset, (byte) (index * 4));
            }

            public void setSecondaryCamo(int classNum, string camo)
            {
                byte[] data = this.getCamoId(camo);
                uint address = (this.Addresses.MPDATA + 0xae5b) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void setSecondaryWeapon(int classNum, string weapon)
            {
                byte[] data = this.getWeaponIdForClasses(classNum, weapon);
                uint address = (uint) (((this.Addresses.MPDATA + 0xae55) + ((classNum - 1) * 0x34)) - 2);
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void setStat(string statName, int value)
            {
                XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, this.Addresses.SetIntPlayerStat);
                XDRPCArgumentInfo<int> info = new XDRPCArgumentInfo<int>(0);
                XDRPCStringArgumentInfo info2 = new XDRPCStringArgumentInfo(statName);
                XDRPCArgumentInfo<int> info3 = new XDRPCArgumentInfo<int>(value);
                ((XDevkit.IXboxConsole) this.xbc).ExecuteRPC<int>(options, new XDRPCArgumentInfo[] { info, info2, info3 });
            }

            public void setTactical(int classNum, string tactical)
            {
                byte[] data = this.getTacticalId(tactical);
                uint address = (this.Addresses.MPDATA + 0xae6b) + ((uint) ((classNum - 1) * 0x34));
                ((XDevkit.IXboxConsole) this.xbc).setMemory(address, data);
            }

            public void UnlockAll()
            {
                ((XDevkit.IXboxConsole) this.xbc).setMemory(this.Addresses.UnlockAll, new UnlockAllData().UnlockAllData_);
            }
        }

        public class rgba
        {
            public byte A = 0;
            public byte B = 0;
            public byte G = 0;
            public byte R = 0;

            public rgba(byte r, byte g, byte b, byte a)
            {
                this.R = r;
                this.B = g;
                this.G = b;
                this.A = a;
            }

            public byte[] getRGBA()
            {
                return new byte[] { this.R, this.B, this.G, this.A };
            }
        }

        public class WeaponDef
        {
            public uint address = 0;
            public Suave.Classes.SuaveLib SuaveLib;
            public XDevkit.IXboxConsole xbc;

            public WeaponDef(uint WeaponDefAddress, Suave.Classes.SuaveLib _SuaveLib)
            {
                this.address = WeaponDefAddress;
                this.SuaveLib = _SuaveLib;
                this.xbc = xbc;
            }
        }
    }
}

