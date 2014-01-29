using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XDevkit;
using XDevkitExt;
using Microsoft.Test.Xbox.Profiles;
using Microsoft.Test.Xbox.XDRPC;
using System.Threading;
using test;
using Tools;
using System.Windows.Forms;
using DevTool.Classes;

namespace Profiles
{
    class pUtil
    {
        static public byte[] HexStringToByteArray(string str)
        {
            if (str.Length % 2 == 1)
            {
                return null;
            }

            List<byte> ret = new List<byte>();
            for (int i = 0; i < str.Length; i += 2)
            {
                ret.Add(Convert.ToByte(str.Substring(i, 2), 16));
            }

            return ret.ToArray();
        }

        static public int Bitswap32(int i)
        {
            byte[] data = BitConverter.GetBytes(i);
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }
    }

    class DevUsers
    {
        static public string[] ListGamerTags = new string[32];
        static public string[] ListSXuid = new string[8];
        static public ulong[] ListLXuid = new ulong[8];

        static public string[] ListFGamerTags = new string[100];
        static public string[] ListFSXuid = new string[100];
        static public ulong[] ListFLXuid = new ulong[100];
        static public string[]  ListFGameID = new string[100];

        static public int PartyUsersCount = 0;
        static public int FUsersCount = 0;

        static public void SetGamertag(XDevkit.IXboxConsole xbc, string nGamertag, string nXUID)
        {
            byte[] data = ((XDevkit.IXboxConsole)xbc).WideChar(nGamertag);
            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_rguserinfoDEV + 0x20, data);

            byte[] XUID = pUtil.HexStringToByteArray(nXUID);
            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_rguserinfoDEV + 0x40, XUID);
        }

        static public string GetGamertag(XDevkit.IXboxConsole xbc)
        {
            byte[] addrdata = ((XDevkit.IXboxConsole)xbc).getMemory(Tools.Addresses.g_rguserinfoDEV + 0x20, 16 * 2);
            return Encoding.BigEndianUnicode.GetString(addrdata);
        }

        static public string GetXUID(XDevkit.IXboxConsole xbc)
        {
            byte[] XUIDdata = ((XDevkit.IXboxConsole)xbc).getMemory(Addresses.g_rguserinfoDEV + 0x40, 8);
            return BitConverter.ToString(XUIDdata).Replace("-", "");
        }

        static public string XamUserGetXUID(XDevkit.IXboxConsole xbc)
        {
            byte[] ee = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_freememory + 0x10, ee);

            uint result = ((XDevkit.IXboxConsole)xbc).Call(Addresses.g_XamUserGetXUIDDEV, new object[] { 0, 2, Addresses.g_freememory + 0x10 });
            byte[] data = ((XDevkit.IXboxConsole)xbc).getMemory(0x81AA2010, 8);
            string rat = BitConverter.ToString(data).Replace("-", "");

            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_freememory + 0x10, ee);
            return rat;
        }

        static public ulong GetXUID(XDevkit.IXboxConsole xbc, string gamertag)
        {
            XDKUtilities.FIND_USER_INFO_RESPONSE test = XDKUtilities.XUserFindUser(xbc, (ulong)0x0009000006F93463L, gamertag);
            return test.OnlineXUID;
        }

        //static public string GetXUID(XDevkit.IXboxConsole xbc, string gamertag)
        //{
        //    XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, Addresses.g_XUserFindUserAddressDEV);


        //    //((XDevkit.IXboxConsole)xbc).Call(0x81824DF8, new object[] { 0x0009000006F93463, 0, gamertag, (int)0x18, Addresses.g_freememory + 0x20, 0 });
        //    XDRPCStringArgumentInfo GT = new XDRPCStringArgumentInfo(gamertag, Encoding.ASCII);
        //    XDRPCArgumentInfo<ulong> MyXUID = new XDRPCArgumentInfo<ulong>(0x0009000006F93463L);
        //    XDRPCArgumentInfo<int> idk = new XDRPCArgumentInfo<int>(0);
        //    XDRPCArgumentInfo<int> idk2 = new XDRPCArgumentInfo<int>((int)0x18);
        //    XDRPCArgumentInfo<ulong> XUID = new XDRPCArgumentInfo<ulong>(0L);
        //    uint num = ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { MyXUID, idk, GT, idk2, XUID, idk });

        //    return XUID.Value.ToString("X16");
        //}

        static public void JoinParty(XDevkit.IXboxConsole xbc, UserIndex userIndex, Xuid xuidContact)
        {
            try
            {
                XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0xb01);
                XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>((uint)userIndex);
                XDRPCArgumentInfo<ulong> info2 = new XDRPCArgumentInfo<ulong>((ulong)xuidContact);
                uint errorCode = ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info, info2 });
                if (errorCode != 0)
                {
                    CreateExceptionFromErrorCode(errorCode);
                }
                WaitForPartyState(xbc, PartyState.XPARTY_STATE_INPARTY, TimeSpan.FromSeconds(15.0));
            }
            catch (XDRPCException exception)
            {
                //throw new ProfilesException(exception);
            }
        }

        static public XDRPCStructArgumentInfo<XPARTY_USER_LIST> GetPartyUserList(XDevkit.IXboxConsole xbc)
        {
            XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0xaff);
            XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>(1);
            XDRPCStructArgumentInfo<XPARTY_USER_LIST> info2 = new XDRPCStructArgumentInfo<XPARTY_USER_LIST>(new XPARTY_USER_LIST(), ArgumentType.Out);
            uint errorCode = ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info, info2 });
            if (errorCode != 0)
            {
                //throw ProfilesExceptionFactory.CreateExceptionFromErrorCode(errorCode);
            }
            return info2;
        }

        //public IEnumerable<PartyMember> GetPartyMembers()
        //{
        //    IEnumerable<PartyMember> enumerable;
        //    try
        //    {
        //        XDRPCStructArgumentInfo<XPARTY_USER_LIST> partyUserList = this.GetPartyUserList();
        //        List<PartyMember> list = new List<PartyMember>();
        //        for (int i = 0; i < partyUserList.Value.dwUserCount; i++)
        //        {
        //            byte[] destinationArray = new byte[120];
        //            Array.Copy(partyUserList.Value.Users, i * 120, destinationArray, 0, destinationArray.Length);
        //            XPARTY_USER_INFO xparty_user_info = new XPARTY_USER_INFO();
        //            XDRPCStructArgumentInfo<XPARTY_USER_INFO> info2 = new XDRPCStructArgumentInfo<XPARTY_USER_INFO>(xparty_user_info, ArgumentType.Out);
        //            info2.UnpackBufferData(destinationArray);
        //            bool isLocal = (info2.Value.dwFlags & 1) > 0;
        //            PartyMember item = new PartyMember(info2.Value.GamerTag, info2.Value.Xuid, isLocal, (UserIndex)info2.Value.dwUserIndex);
        //            list.Add(item);
        //        }
        //        enumerable = list;
        //    }
        //    catch (XDRPCException exception)
        //    {
        //        throw new ProfilesException(exception);
        //    }
        //    return enumerable;
        //}

        static public void GetPartyMembers(XDevkit.IXboxConsole xbc)
        {
            //XPARTY_USER_LIST partyUserList = GetPartyUserList(xbc);
            XDRPCStructArgumentInfo<XPARTY_USER_LIST> partyUserList = GetPartyUserList(xbc);
            int Count = partyUserList.Value.dwUserCount; //pUtil.Bitswap32(partyUserList.Value.dwUserCount);

            if (Count > 8)
                return;

            PartyUsersCount = Count;
            for (int i = 0; i < Count; i++)
            {
                byte[] destinationArray = new byte[120];
                Array.Copy(partyUserList.Value.Users, i * 120, destinationArray, 0, destinationArray.Length);
                XPARTY_USER_INFO xparty_user_info = new XPARTY_USER_INFO();
                XDRPCStructArgumentInfo<XPARTY_USER_INFO> info2 = new XDRPCStructArgumentInfo<XPARTY_USER_INFO>(xparty_user_info, ArgumentType.Out);
                info2.UnpackBufferData(destinationArray);

                ListGamerTags[i] = info2.Value.GamerTag;
                ListSXuid[i] = info2.Value.Xuid.ToString("X16");
                ListLXuid[i] = info2.Value.Xuid;
            }
        }

        static public void KickUserFromParty(XDevkit.IXboxConsole xbc, Xuid xuidToKick)
        {
            try
            {
                XDRPCStructArgumentInfo<XPARTY_USER_LIST> partyUserList = GetPartyUserList(xbc);
                XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0xb02);
                XDRPCArgumentInfo<ulong> info2 = new XDRPCArgumentInfo<ulong>((ulong)xuidToKick);
                uint errorCode = ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info2 });
                if (errorCode != 0)
                {
                    CreateExceptionFromErrorCode(errorCode);
                }
                WaitForPartyState(xbc, PartyState.XPARTY_STATE_INPARTY, TimeSpan.FromSeconds(15.0));
            }
            catch (XDRPCException exception)
            {
                //throw new ProfilesException(exception);
            }
        }


        static public void WaitForPartyState(XDevkit.IXboxConsole xbc, PartyState state, TimeSpan timeout)
        {
            PartyState state2;
            PartyErrorCodes codes;
            TimeSpan span;
            DateTime now = DateTime.Now;
            do
            {
                Thread.Sleep(0x7d0);
                GetPartyState(xbc, out state2, out codes);
                span = (TimeSpan)(DateTime.Now - now);
            }
            while ((state2 != state) && (span < timeout));
            if (codes != PartyErrorCodes.XPARTY_ERROR_NONE)
            {

            }
            if ((span >= timeout) && (state2 != state))
            {
                throw new PartyTimeOutException();
            }
        }

        static public void CreateExceptionFromErrorCode(uint errorCode)
        {
            if (errorCode == 2)
                MessageBox.Show("NoSuchUserException");
            else if (errorCode == 0x80070525)
                MessageBox.Show("NoSuchUserException");
            else if (errorCode == 0x80151802)
                MessageBox.Show("ConnectionToLiveFailedException" + errorCode);
            else if (errorCode == 0x80151903)
                MessageBox.Show("ConnectionToLiveFailedException" + errorCode);
            else if (errorCode == 0x80151906)
                MessageBox.Show("ConnectionToLiveFailedException" + errorCode);
            else if (errorCode == 0x80151907)
                MessageBox.Show("ConnectionToLiveFailedException" + errorCode);
            else if (errorCode == 0x80155209)
                MessageBox.Show("UserNotConnectedToLiveException");
            else if (errorCode == 0x807d0003)
                MessageBox.Show("NotInPartyException");
            else if (errorCode == 0x80154000)
                MessageBox.Show("AccountNameTakenException");
        }

        static public void GetPartyState(XDevkit.IXboxConsole xbc, out PartyState state, out PartyErrorCodes errorCode)
        {
            XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0xb0f);
            XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>(0, ArgumentType.Out);
            XDRPCArgumentInfo<uint> info2 = new XDRPCArgumentInfo<uint>(0, ArgumentType.Out);
            ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info, info2 });
            state = (PartyState)info.Value;
            errorCode = (PartyErrorCodes)info2.Value;
        }

        static public void GetMyFriends(XDevkit.IXboxConsole xbc, UserIndex userIndex)
        {
            //try
            //{
                uint friendIndex = 0;
                while (true)
                {
                    XONLINE_FRIEND iteratorVariable0;
                    if (GetNextFriend(xbc, userIndex, friendIndex, out iteratorVariable0) != 0)
                    {
                        break;
                    }
                    FriendRequestStatus requestAccepted = FriendRequestStatus.RequestAccepted;
                    if ((iteratorVariable0.dwFriendState & 0x40000000) > 0)
                    {
                        requestAccepted = FriendRequestStatus.RequestSent;
                    }
                    else if ((iteratorVariable0.dwFriendState & 0x80000000) > 0)
                    {
                        requestAccepted = FriendRequestStatus.RequestReceived;
                    }
                    FriendStatus offline = FriendStatus.Offline;
                    if ((iteratorVariable0.dwFriendState & 1) > 0)
                    {
                        offline = ((FriendStatus)iteratorVariable0.dwFriendState) & ((FriendStatus)0xf0000);
                    }
                    ListFGamerTags[friendIndex] = iteratorVariable0.szGamertag;
                    ListFLXuid[friendIndex] = iteratorVariable0.xuid;
                    ListFSXuid[friendIndex] = iteratorVariable0.xuid.ToString("X16");
                    ListFGameID[friendIndex] = iteratorVariable0.dwTitleID.ToString("X8");
                    //offline;
                    friendIndex++;
                }
                FUsersCount = (int)friendIndex;
            //}
            //catch
            //{

            //}
        }

        //static public string[] ListFGamerTags = new string[32];
        //static public string[] ListFSXuid = new string[8];
        //static public ulong[] ListFLXuid = new ulong[8];

        //static public IEnumerable<Friend> EnumerateFriends(XDevkit.IXboxConsole xbc, UserIndex userIndex)
        //static public void GetMyFriends(XDevkit.IXboxConsole xbc, UserIndex userIndex)
        //{
        //    uint friendIndex = 0;
        //    while (true)
        //    {
        //        XONLINE_FRIEND iteratorVariable0;
        //        if (GetNextFriend(xbc, userIndex, friendIndex, out iteratorVariable0) != 0)
        //        {
        //            yield break;
        //        }
        //        FriendRequestStatus requestAccepted = FriendRequestStatus.RequestAccepted;
        //        if ((iteratorVariable0.dwFriendState & 0x40000000) > 0)
        //        {
        //            requestAccepted = FriendRequestStatus.RequestSent;
        //        }
        //        else if ((iteratorVariable0.dwFriendState & 0x80000000) > 0)
        //        {
        //            requestAccepted = FriendRequestStatus.RequestReceived;
        //        }
        //        FriendStatus offline = FriendStatus.Offline;
        //        if ((iteratorVariable0.dwFriendState & 1) > 0)
        //        {
        //            offline = ((FriendStatus)iteratorVariable0.dwFriendState) & ((FriendStatus)0xf0000);
        //        }
        //        //Friend iteratorVariable4 = new Friend(iteratorVariable0.szGamertag, iteratorVariable0.xuid, requestAccepted, offline, iteratorVariable0.wszRichPresence, iteratorVariable0.dwTitleID);
        //        //yield return iteratorVariable4;
        //        friendIndex++;
        //    }
        //}

        static private uint GetNextFriend(XDevkit.IXboxConsole xbc, UserIndex userIndex, uint friendIndex, out XONLINE_FRIEND friend)
        {
            uint num2;
            //try
            //{
                friend = new XONLINE_FRIEND();
                XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0x4ea);
                XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>((uint)userIndex);
                XDRPCArgumentInfo<uint> info2 = new XDRPCArgumentInfo<uint>(friendIndex);
                XDRPCArgumentInfo<uint> info3 = new XDRPCArgumentInfo<uint>(1);
                XDRPCArgumentInfo<uint> info4 = new XDRPCArgumentInfo<uint>(0, ArgumentType.Out);
                XDRPCArgumentInfo<uint> info5 = new XDRPCArgumentInfo<uint>(0, ArgumentType.Out);
                uint num = ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info, info2, info3, info4, info5 });
                if (num == 0)
                {
                    options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0x250);
                    info5 = new XDRPCArgumentInfo<uint>(info5.Value);
                    XDRPCArgumentInfo<uint> info6 = new XDRPCArgumentInfo<uint>(0);
                    XDRPCStructArgumentInfo<XONLINE_FRIEND> info7 = new XDRPCStructArgumentInfo<XONLINE_FRIEND>(new XONLINE_FRIEND(), ArgumentType.Out);
                    info4 = new XDRPCArgumentInfo<uint>(info4.Value);
                    XDRPCArgumentInfo<uint> info8 = new XDRPCArgumentInfo<uint>(0, ArgumentType.Out);
                    XDRPCNullArgumentInfo info9 = new XDRPCNullArgumentInfo();
                    num = ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info5, info6, info7, info4, info8, info9 });
                    friend = info7.Value;
                    options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0x414);
                    ((XDevkit.IXboxConsole)xbc).ExecuteRPC<bool>(options, new XDRPCArgumentInfo[] { info5 });
                }
                num2 = num;
            //}
            //catch (XDRPCException exception)
            //{
            //    //throw new ProfilesException(exception);
            //}
            return num2;
        }

        static public uint GetXuidFromIndex(XDevkit.IXboxConsole xbc, UserIndex userIndex, out ulong offlineXuid)
        {
            uint num2;
            //try
            //{
                XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0x20a);
                XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>((uint)userIndex);
                XDRPCArgumentInfo<uint> info2 = new XDRPCArgumentInfo<uint>(2);
                XDRPCArgumentInfo<ulong> info3 = new XDRPCArgumentInfo<ulong>(0L, ArgumentType.Out);
                uint num = ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info, info2, info3 });
                offlineXuid = info3.Value;
                num2 = num;
            //}
            //catch (XDRPCException exception)
            //{
            //}
            return num2;
        }

        static public void SendFriendRequest(XDevkit.IXboxConsole xbc, Xuid xuidFrom, Xuid xuidTo)
        {
            try
            {
                XDRPCExecutionOptions options = new XDRPCExecutionOptions(XDRPCMode.Title, "xam.xex", 0x4e6);
                XDRPCArgumentInfo<ulong> info = new XDRPCArgumentInfo<ulong>((ulong)xuidFrom);
                XDRPCArgumentInfo<ulong> info2 = new XDRPCArgumentInfo<ulong>((ulong)xuidTo);
                XDRPCNullArgumentInfo info3 = new XDRPCNullArgumentInfo();
                uint errorCode = ((XDevkit.IXboxConsole)xbc).ExecuteRPC<uint>(options, new XDRPCArgumentInfo[] { info, info2, info3 });
                if (errorCode != 0)
                {
                    //throw ProfilesExceptionFactory.CreateExceptionFromErrorCode(errorCode);
                }
            }
            catch (XDRPCException exception)
            {
                //throw new ProfilesException(exception);
            }
        }

        [StructLayout(LayoutKind.Sequential), XDRPCStruct]
        public struct XONLINE_FRIEND
        {
            public ulong xuid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10, ArraySubType = UnmanagedType.LPStr)]
            public string szGamertag;
            public uint dwFriendState;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] sessionID;
            public uint dwTitleID;
            public ulong ftUserTime;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] xnkidInvite;
            public ulong gameinviteTime;
            public uint cchRichPresence;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x40, ArraySubType = UnmanagedType.LPWStr)]
            public string wszRichPresence;
        }

        public enum PartyState
        {
            XPARTY_STATE_IDLE,
            XPARTY_STATE_CONNECTING,
            XPARTY_STATE_INPARTY,
            XPARTY_STATE_DISCONNECTING
        }

        [StructLayout(LayoutKind.Sequential), XDRPCStruct]
        public struct XPARTY_USER_LIST
        {
            public int dwUserCount;
            public uint placeHolder;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xf00)]
            public byte[] Users;
        }

        [StructLayout(LayoutKind.Sequential), XDRPCStruct]
        public struct XPARTY_USER_INFO
        {
            public ulong Xuid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10, ArraySubType = UnmanagedType.LPStr)]
            public string GamerTag;
            public uint dwUserIndex;
            public int NatType;
            public uint dwTitleId;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] SessionInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] CustomData;
        }

        [StructLayout(LayoutKind.Sequential), XDRPCStruct]
        public struct FIND_USER_INFO_RESPONSE
        {
            private ulong qwXuid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.LPStr)]
            private string szGamerTag;

            public ulong OnlineXUID
            {
                get { return qwXuid; }
                set
                {
                    if (!XuidTester.IsOnlineXUID(value)) throw new Exception("XDKUtilities.FIND_USER_INFO_RESPONSE: Invalid online XUID specified.");
                    qwXuid = value;
                }
            }

            public string Gamertag
            {
                get { return szGamerTag; }
                set
                {
                    if (value.Length > 15) throw new Exception("XDKUtilities.FIND_USER_INFO_RESPONSE: Invalid Gamertag specified. It must be less than or equal to 15 characters in length.");
                    szGamerTag = value;
                }
            }
        }
    }

    class Users
    {
        static public void SetGamertag(XDevkit.IXboxConsole xbc, string nGamertag, string nXUID)
        {
            byte[] data = ((XDevkit.IXboxConsole)xbc).WideChar(nGamertag);
            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_rguserinfo + 0x20, data);

            byte[] XUID = pUtil.HexStringToByteArray(nXUID);
            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_rguserinfo + 0x40, XUID);
        }

        static public string GetGamertag(XDevkit.IXboxConsole xbc)
        {
            byte[] addrdata = ((XDevkit.IXboxConsole)xbc).getMemory(Tools.Addresses.g_rguserinfo + 0x20, 16 * 2);
            return Encoding.BigEndianUnicode.GetString(addrdata);
        }

        static public string GetXUID(XDevkit.IXboxConsole xbc)
        {
            byte[] XUIDdata = ((XDevkit.IXboxConsole)xbc).getMemory(Addresses.g_rguserinfo + 0x40, 8);
            return BitConverter.ToString(XUIDdata).Replace("-", "");
        }

        static public string XamUserGetXUID(XDevkit.IXboxConsole xbc)
        {
            byte[] ee = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_freememory + 0x10, ee);

            uint result = ((XDevkit.IXboxConsole)xbc).Call(Addresses.g_XamUserGetXUID, new object[] { 0, 2, Addresses.g_freememory + 0x10 });
            byte[] data = ((XDevkit.IXboxConsole)xbc).getMemory(0x81AA2010, 8);
            string rat = BitConverter.ToString(data).Replace("-", "");

            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_freememory + 0x10, ee);
            return rat;
        }

        static public string GetXUID(XDevkit.IXboxConsole xbc, string gamertag)
        {
            byte[] ee = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_freememory + 0x20, ee);

            ((XDevkit.IXboxConsole)xbc).Call(Addresses.g_XUserFindUserAddress, new object[] { 0x0009000006F93463, 0, gamertag, (int)0x18, Addresses.g_freememory + 0x20, 0 });

            Thread.Sleep(1000);

            byte[] XUID = ((XDevkit.IXboxConsole)xbc).getMemory(Addresses.g_freememory + 0x20, 8);
            string rat = BitConverter.ToString(XUID).Replace("-", "");

            ((XDevkit.IXboxConsole)xbc).setMemory(Addresses.g_freememory + 0x20, ee);
            return rat;
        }

        [StructLayout(LayoutKind.Sequential), XDRPCStruct]
        public struct FIND_USER_INFO_RESPONSE
        {
            private ulong qwXuid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16, ArraySubType = UnmanagedType.LPStr)]
            private string szGamerTag;

            public ulong OnlineXUID
            {
                get { return qwXuid; }
                set
                {
                    if (!XuidTester.IsOnlineXUID(value)) throw new Exception("XDKUtilities.FIND_USER_INFO_RESPONSE: Invalid online XUID specified.");
                    qwXuid = value;
                }
            }

            public string Gamertag
            {
                get { return szGamerTag; }
                set
                {
                    if (value.Length > 15) throw new Exception("XDKUtilities.FIND_USER_INFO_RESPONSE: Invalid Gamertag specified. It must be less than or equal to 15 characters in length.");
                    szGamerTag = value;
                }
            }
        }
    }

    class Party
    {
         static public string[] ListGamerTags    = new string[32];
         static public string[] ListSXuid        = new string[8];
         static public ulong[]  ListLXuid        = new ulong[8];
         static public int PartyUsersCount = 0;

        static public XPARTY_USER_LIST GetPartyUserList(XDevkit.IXboxConsole xbc)
        {
            ((XDevkit.IXboxConsole)xbc).CallSysFunction(xbc.abcdresfunctxrpc("xam.xex", 0xaff), 1, Addresses.g_freememory + 0x80);

            byte[] data = ((XDevkit.IXboxConsole)xbc).getMemory(Addresses.g_freememory + 0x80, 2024);
            IntPtr ptr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
            XPARTY_USER_LIST user = (XPARTY_USER_LIST)Marshal.PtrToStructure(ptr, typeof(XPARTY_USER_LIST));

            return user;
        }

        static public void GetPartyMembers(XDevkit.IXboxConsole xbc)
        {
            XPARTY_USER_LIST partyUserList = GetPartyUserList(xbc);
            int Count = pUtil.Bitswap32(partyUserList.dwUserCount);

            if (Count > 8)
                return;

            PartyUsersCount = Count;
            for (int i = 0; i < Count; i++)
            {
                byte[] destinationArray = new byte[120];
                Array.Copy(partyUserList.Users, i * 120, destinationArray, 0, destinationArray.Length);
                XPARTY_USER_INFO xparty_user_info = new XPARTY_USER_INFO();
                XDRPCStructArgumentInfo<XPARTY_USER_INFO> info2 = new XDRPCStructArgumentInfo<XPARTY_USER_INFO>(xparty_user_info, ArgumentType.Out);
                info2.UnpackBufferData(destinationArray);

                ListGamerTags[i] = info2.Value.GamerTag;
                ListSXuid[i] = info2.Value.Xuid.ToString("X16");
                ListLXuid[i] = info2.Value.Xuid;
            }
        }

        static public void JoinSpecificParty(XDevkit.IXboxConsole xbc, string Gamertag, ulong MyOnlineXUID)
        {
            try
            {
                //ulong xuid = Users.GetXUID(xbc, Gamertag);
                //XDRPCArgumentInfo<uint> info = new XDRPCArgumentInfo<uint>((uint)MyOnlineXUID);
                //XDRPCArgumentInfo<ulong> info2 = new XDRPCArgumentInfo<ulong>((ulong)xuid);
                //((XDevkit.IXboxConsole)xbc).Call(xbc.abcdresfunctxrpc("xam.xex", 0xb01), new object[] { info, info2 });
            }
            catch (Exception ex)
            {
                //string.Format("Failed to join {0}'s party.{1}{2}", gt, Environment.NewLine, ex.Message), ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [StructLayout(LayoutKind.Sequential), XDRPCStruct]
        public struct XPARTY_USER_LIST
        {
            public int dwUserCount;
            public uint placeHolder;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0xf00)]
            public byte[] Users;
        }

        [StructLayout(LayoutKind.Sequential), XDRPCStruct]
        public struct XPARTY_USER_INFO
        {
            public ulong Xuid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10, ArraySubType = UnmanagedType.LPStr)]
            public string GamerTag;
            public uint dwUserIndex;
            public int NatType;
            public uint dwTitleId;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
            public byte[] SessionInfo;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10)]
            public byte[] CustomData;
        }
    }
}
