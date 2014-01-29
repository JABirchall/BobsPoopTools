namespace Microsoft.Test.Xbox.Profiles
{
    using System;
    using System.Runtime.CompilerServices;

    public class Friend : Gamer
    {
        internal Friend(string gamertag, Xuid onlineXuid, FriendRequestStatus requestStatus, FriendStatus friendStatus, string richPresence, uint titleId) : base(gamertag, onlineXuid)
        {
            this.RequestStatus = requestStatus;
            this.FriendState = friendStatus;
            this.RichPresence = richPresence;
            this.TitleId = titleId;
        }

        public FriendStatus FriendState { get; private set; }

        public FriendRequestStatus RequestStatus { get; set; }

        public string RichPresence { get; private set; }

        public uint TitleId { get; private set; }
    }
}

